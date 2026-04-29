# AIDbOptimize 数据库配置优化 workflow 二期总体方案

## 1. 目标

在当前已经完成的 `DbConfigOptimization` workflow 基线之上，新增一轮“更深规则与更多真实指标采集”增强，用于把当前“方向正确但偏保守”的建议，提升到“证据更强、上下文更完整、可解释性更高”的水平。

本轮聚焦四件事：

1. 扩展数据库内部采集内容  
   不再只采 `max_connections / innodb_buffer_pool_size / shared_buffers / work_mem` 这类少量配置项，而是补齐实例内存、缓存命中、连接压力、慢查询背景、关键引擎状态等真实指标。
2. 新增宿主资源上下文采集  
   结合数据库所在容器、主机或托管环境的资源边界，避免只看数据库参数值本身。
3. 扩展 engine-specific 规则引擎  
   在 `MySQL / PostgreSQL` 两个引擎上引入更细粒度的规则输出，包含置信度、适用前提、证据链和降级原因。
4. 增强前端和 history 展示  
   把“建议为什么成立、缺了哪些上下文、当前结论是否保守”完整展示出来。

## 2. 目标边界

本轮仍然只做“建议生成”，不做自动配置下发。

本轮不做：

1. 自动执行 `SET / ALTER SYSTEM / 参数热更新 / 实例重启`
2. 自动直接给出唯一推荐值并下发到数据库
3. 泛化成通用 DBA 诊断平台
4. 全数据库引擎铺开  
   本轮继续只深挖 `MySQL / PostgreSQL`

## 3. 核心结论

### 3.1 继续复用当前 workflow，不新开第二条执行链

继续复用当前 `DbConfigOptimization` workflow，不新建第二条 workflow。  
增强点全部落在现有节点内部：

1. `DbConfigSnapshotCollectorExecutor`
2. `DbConfigRuleAnalysisExecutor`
3. `DbConfigDiagnosisAgentExecutor`
4. `DbConfigGroundingExecutor`

### 3.2 采集增强优先于 prompt 增强

当前建议偏保守的根因，不是 agent “不会说”，而是 evidence 不够。

本轮优先级固定为：

1. 先补采集
2. 再补规则
3. 最后补 prompt 和展示

### 3.3 建议必须继续保持“证据可追溯”

新增规则不能只输出更多文案，必须继续满足：

1. 每条建议都能追到具体 evidence key
2. 每条建议都能解释“为什么现在提”
3. 每条建议都能标注“是否缺少前置上下文”
4. 每条建议都能区分“参数调优建议”和“先补观测能力建议”

### 3.4 本轮把“建议正确性”定义得更明确

本轮不追求“一步到位自动给出唯一正确值”，而追求：

1. 建议方向正确
2. 建议触发条件明确
3. 建议证据链完整
4. 缺少上下文时显式降级为“建议继续评估”

## 4. 采集增强方向

### 4.1 数据库内部指标

#### MySQL

新增重点采集：

1. `innodb_buffer_pool_size`
2. `innodb_buffer_pool_instances`
3. `max_connections`
4. `thread_cache_size`
5. `tmp_table_size / max_heap_table_size`
6. `slow_query_log / long_query_time`
7. `performance_schema` 关键状态
8. `Questions / Threads_connected / Threads_running / Slow_queries / Connections / Aborted_connects`
9. `Innodb_buffer_pool_reads / Innodb_buffer_pool_read_requests`
10. `Created_tmp_disk_tables / Created_tmp_tables`
11. `table_open_cache / open_files_limit`
12. `wait_timeout / interactive_timeout`

#### PostgreSQL

新增重点采集：

1. `shared_buffers`
2. `work_mem`
3. `maintenance_work_mem`
4. `effective_cache_size`
5. `max_connections`
6. `checkpoint_timeout / checkpoint_completion_target`
7. `wal_buffers`
8. `random_page_cost / seq_page_cost`
9. `track_io_timing / shared_preload_libraries`
10. `pg_stat_database` 关键字段
11. `blks_hit / blks_read / xact_commit / xact_rollback / temp_files / deadlocks`
12. `pg_stat_bgwriter`
13. `pg_stat_statements` 可用性与 TopN 慢 SQL 背景摘要

### 4.2 宿主资源上下文采集

数据库参数不能脱离运行边界解读。本轮新增“宿主资源上下文”采集，包含：

1. 实例资源作用域  
   `container / host / managed-service / unknown`
2. 实例可用内存边界  
   容器 memory limit、cgroup memory、托管规格内存或主机总内存
3. 当前可用内存与内存压力
4. CPU 可用核数与当前使用摘要
5. 数据目录所在卷的总容量、可用容量、压力级别
6. 运行边界来源  
   当前字段来自容器、宿主、云托管元数据还是无法识别

### 4.3 宿主资源获取方式结论

本轮建议新增一组只读 `HostContext MCP` 工具，而不是把宿主信息采集塞进数据库 MCP 里。

建议工具最少包括：

1. `get_container_limits`
2. `get_container_stats`
3. `get_disk_usage`
4. `get_host_memory`
5. `get_host_cpu`
6. `get_process_limits`
7. `resolve_runtime_target`

### 4.4 宿主资源来源优先级

本轮明确规定“先看实例真实可用边界，再看宿主全局资源”，优先级如下：

1. 容器 / 沙箱资源边界  
   例如 memory limit、cpu quota、数据卷挂载点空间
2. 容器内只读系统信息  
   例如 `/proc/meminfo`、`nproc`、`df`
3. 托管服务实例规格  
   例如云数据库规格、vCPU、内存、存储配额
4. 宿主机只读系统信息

含义：

1. 内存类建议优先使用容器 limit 或托管规格，而不是宿主总内存
2. CPU 并发类建议优先使用实例 CPU 限额，而不是宿主总核数
3. 磁盘类建议优先使用数据库数据卷挂载点的剩余空间

### 4.5 运行目标识别必须显式设计

宿主资源采集是否可行，取决于“当前数据库连接对应哪个运行目标”。本轮必须明确：

1. 本地 Aspire / Docker 场景  
   通过连接记录、服务名、容器标签、端口映射或手工配置映射到具体容器
2. 远程自建数据库场景  
   如果 HostContext MCP 能远程读取主机，只读采；否则记录缺失上下文
3. 云托管数据库场景  
   不试图伪装成主机可见；优先采托管规格元数据，不可得则降级

### 4.6 采集新鲜度与缓存

宿主上下文不是所有字段都要实时采。本轮建议：

1. 配置类字段  
   每次 workflow 重新采
2. 运行指标类字段  
   每次 workflow 重新采
3. 宿主静态字段  
   允许 5 分钟 TTL 缓存，例如总内存、CPU 限额、容器限制
4. 宿主动态字段  
   当前 workflow 内实时采，例如可用内存、CPU usage、磁盘剩余量

所有缓存字段必须带上：

1. `capturedAt`
2. `expiresAt`
3. `isCached`

### 4.7 失败与缺失处理原则

宿主上下文采不到时，不抛工作流级错误，按“结构化缺失上下文”处理。

必须输出：

1. 缺失的是哪个字段
2. 为什么缺失  
   权限不足、托管环境不可见、工具未配置、目标无法识别、命令超时
3. 对建议造成的影响  
   `requiresMoreContext = true`

## 5. 规则增强方向

### 5.1 从“单点参数判断”升级为“参数 + 运行指标 + 宿主上下文联合判断”

示例：

1. MySQL `innodb_buffer_pool_size`  
   不仅看当前值，还结合实例可用内存、buffer pool miss 指标、查询压力
2. PostgreSQL `shared_buffers`  
   不仅看当前值，还结合缓存命中、实例可用内存和连接规模
3. `max_connections`  
   不只看参数值，还看当前连接高水位、运行中线程、超时配置

### 5.2 规则输出必须带“判断类型”

每条规则结果至少标注：

1. `findingType`  
   `capacity / cache / concurrency / checkpoint / temp-io / slow-query / observability-gap`
2. `confidence`  
   `high / medium / low`
3. `requiresMoreContext`  
   `true / false`
4. `recommendationClass`  
   `tuning / observability / capacity-planning / hygiene`
5. `appliesWhen`  
   说明这条建议成立的前提

### 5.3 增加“观测缺口”类建议

如果缺少 `pg_stat_statements / slow_query_log / performance_schema` 等关键可观测能力，也要生成建议。

这类建议不是“参数调优建议”，而是“先补观测能力”的建议。

### 5.4 规则阈值要版本化

规则判断中的阈值、经验区间、TopN 行数、压力分级算法，都必须可版本追踪。

本轮建议至少做到：

1. 每个规则有 `RuleId`
2. 每个规则输出带 `RuleVersion`
3. history detail 能看到命中的规则版本
4. 规则阈值变更时能通过测试与文档一起演进

## 6. 安全与权限边界

### 6.1 宿主上下文工具必须只读

`HostContext MCP` 只允许：

1. 读取容器限制
2. 读取容器状态
3. 读取磁盘使用量
4. 读取系统资源摘要

不允许：

1. 修改容器
2. 执行任意写 shell
3. 拉取敏感环境变量全文
4. 把完整进程列表、完整环境变量或凭据文本塞进 evidence

### 6.2 脱敏边界要前置

任何宿主采集结果在进入 evidence 前都要经过标准化和脱敏。

至少避免：

1. 连接串明文
2. 用户名密码
3. 环境变量中的 token / key / secret
4. 绝对路径中不必要的个人目录暴露

## 7. agent 输出要求

agent 继续只做解释，不做自由采集。

本轮 agent 输出增强为：

1. 每条建议附带 evidence refs
2. 每条建议附带“为什么现在触发”
3. 每条建议附带“如果不处理，会出现什么影响”
4. 如上下文不足，明确写“当前只能给出保守建议”

## 8. 验收结论

本轮完成后，应满足：

1. 最新 `MySQL / PostgreSQL` workflow 结果不再只是泛化建议
2. 结果能体现真实采集值、运行指标和宿主上下文
3. 对缺失上下文有明确降级说明
4. history detail 能回放新增采集项、规则版本和上下文缺失原因
5. 至少有一条 MySQL 会话和一条 PostgreSQL 会话能人工确认“建议方向正确”
6. 至少有一条云托管或不可见宿主场景能人工确认“降级行为正确”
