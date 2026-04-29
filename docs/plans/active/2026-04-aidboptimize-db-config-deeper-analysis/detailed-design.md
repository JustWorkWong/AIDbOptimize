# AIDbOptimize 数据库配置优化 workflow 二期详细设计

## 1. 总体策略

本轮不新增第二条 workflow，而是在现有 `DbConfigOptimization` 基线内部扩展：

1. 采集协议
2. evidence 模型
3. 规则引擎
4. agent prompt
5. 前端结果展示

现有控制面模型、review、history、checkpoint、SSE 全部继续复用。

## 2. 采集设计

## 2.1 采集分层

采集结果分 4 层：

1. `configuration snapshot`
   参数配置快照
2. `runtime metrics snapshot`
   运行指标快照
3. `host context snapshot`
   宿主资源上下文
4. `observability snapshot`
   观测能力是否具备以及慢 SQL/statement 摘要

四层都进入统一 `EvidencePack`，但必须保留来源标签、采集时间和缺失原因。

## 2.2 MySQL 采集合约

建议新增固定模板采集语句或只读工具请求：

1. `show variables like ...`
2. `show global status like ...`
3. `select @@...`
4. `select ... from performance_schema...`

统一归一后端结构：

```json
{
  "engine": "MySql",
  "configuration": {
    "innodb_buffer_pool_size": "134217728",
    "max_connections": "151"
  },
  "runtimeMetrics": {
    "threads_connected": "12",
    "threads_running": "2",
    "slow_queries": "5"
  },
  "hostContext": {
    "resource_scope": "container",
    "memory_limit_bytes": "8589934592"
  },
  "observability": {
    "slow_query_log_enabled": true,
    "performance_schema_enabled": true
  }
}
```

## 2.3 PostgreSQL 采集合约

建议新增固定模板采集语句：

1. `select current_setting(...)`
2. `select * from pg_stat_database`
3. `select * from pg_stat_bgwriter`
4. `select * from pg_stat_statements` 的 TopN 摘要

统一归一后端结构：

```json
{
  "engine": "PostgreSql",
  "configuration": {
    "shared_buffers": "128MB",
    "work_mem": "4MB"
  },
  "runtimeMetrics": {
    "blks_hit": "100000",
    "blks_read": "8000",
    "temp_files": "25"
  },
  "hostContext": {
    "resource_scope": "managed-service",
    "memory_total_bytes": "17179869184"
  },
  "observability": {
    "pg_stat_statements_enabled": false
  }
}
```

## 2.4 HostContext MCP 设计

### 2.4.1 工具集合

建议新增只读 `HostContext MCP`，至少包括：

1. `resolve_runtime_target`
2. `get_container_limits`
3. `get_container_stats`
4. `get_disk_usage`
5. `get_host_memory`
6. `get_host_cpu`
7. `get_process_limits`
8. `get_managed_service_profile`

### 2.4.2 运行目标解析

`resolve_runtime_target` 输入：

```json
{
  "connectionId": "uuid",
  "engine": "MySql",
  "host": "127.0.0.1",
  "port": 3306,
  "metadata": {
    "serviceName": "mysql",
    "containerName": "aidboptimize-mysql-1"
  }
}
```

输出：

```json
{
  "resourceScope": "container",
  "targetType": "docker-container",
  "targetId": "aidboptimize-mysql-1",
  "targetName": "aidboptimize-mysql-1",
  "confidence": "high"
}
```

如果无法识别：

```json
{
  "resourceScope": "unknown",
  "targetType": "unknown",
  "confidence": "low",
  "reason": "No container mapping found for the current connection."
}
```

### 2.4.3 容器资源字段

`get_container_limits` 统一输出：

```json
{
  "containerId": "abc",
  "containerName": "aidboptimize-mysql-1",
  "memoryLimitBytes": 8589934592,
  "cpuLimitCores": 4,
  "mounts": [
    {
      "path": "/var/lib/mysql",
      "source": "mysql-data"
    }
  ],
  "capturedAt": "2026-04-29T10:00:00Z"
}
```

`get_container_stats` 统一输出：

```json
{
  "memoryUsageBytes": 536870912,
  "memoryAvailableBytes": 8053063680,
  "cpuUsagePercent": 17.5,
  "capturedAt": "2026-04-29T10:00:10Z"
}
```

### 2.4.4 托管数据库资源字段

云托管数据库或 PaaS 场景，允许采集“实例规格元数据”替代主机指标：

```json
{
  "resourceScope": "managed-service",
  "profileName": "db.r6.large",
  "memoryTotalBytes": 17179869184,
  "cpuTotalCores": 4,
  "storageTotalBytes": 107374182400,
  "source": "managed-service-profile"
}
```

### 2.4.5 字段清单与单位

| 字段 | 含义 | 单位 | 优先来源 |
| --- | --- | --- | --- |
| `resource_scope` | `container / host / managed-service / unknown` | none | collector |
| `memory_limit_bytes` | 实例可用内存上限 | bytes | container |
| `memory_total_bytes` | 所在环境总内存 | bytes | container -> managed -> host |
| `memory_available_bytes` | 当前可用内存 | bytes | container -> host |
| `memory_pressure_level` | `low / medium / high / unknown` | none | derived |
| `cpu_limit_cores` | 实例可用 CPU 上限 | cores | container / managed |
| `cpu_total_cores` | 所在环境总核数 | cores | managed -> host |
| `cpu_usage_percent` | 当前 CPU 使用摘要 | percent | container -> host |
| `disk_total_bytes` | 数据目录所在卷总容量 | bytes | mount -> host |
| `disk_available_bytes` | 数据目录所在卷可用容量 | bytes | mount -> host |
| `disk_pressure_level` | `low / medium / high / unknown` | none | derived |
| `container_name` | 数据库实例所在容器名 | none | container |
| `container_id` | 容器标识 | none | container |
| `host_name` | 宿主机标识 | none | host |
| `captured_at` | 本次采集时间 | ISO-8601 | collector |
| `expires_at` | 缓存过期时间 | ISO-8601 | collector |
| `is_cached` | 是否来自缓存 | bool | collector |

约束：

1. 采集器必须把原始值和标准化值都保留下来
2. 无法标准化的字段，保留原始值并标记 `normalizedValue = null`
3. 任一关键字段缺失时，必须进入 `MissingContextItems`

## 2.5 来源优先级与降级策略

### 2.5.1 内存类字段

优先级：

1. 容器 memory limit
2. 容器内 `/proc/meminfo`
3. 托管实例规格内存
4. 宿主机内存

降级规则：

1. 若只有宿主总内存，没有容器 limit 或托管规格，则 `requiresMoreContext = true`
2. 若上述都没有，只允许输出“建议进一步评估”

### 2.5.2 CPU 类字段

优先级：

1. 容器 cpu quota / cpuset
2. 托管实例 vCPU
3. 容器内核数
4. 宿主机核数

### 2.5.3 磁盘类字段

优先级：

1. 数据目录所在挂载卷
2. 容器根卷
3. 托管存储配额
4. 宿主磁盘

### 2.5.4 远程与不可见场景

如果数据库不在本地容器、也无法通过远程工具读取宿主环境，则：

1. 不把宿主上下文缺失视为 workflow 失败
2. 在 evidence 中记录 `unavailable_reason`
3. 相关规则自动降级为保守建议

## 2.6 采集新鲜度与缓存策略

### 2.6.1 缓存规则

1. 配置与运行指标  
   每次 workflow 实时采
2. 宿主静态字段  
   允许 5 分钟 TTL
3. 宿主动态字段  
   每次 workflow 实时采

### 2.6.2 超时与重试

1. 数据库 query 单项超时  
   5 秒到 10 秒，按现有引擎能力调整
2. HostContext MCP 单项超时  
   2 秒到 5 秒
3. 超时后不做无限重试  
   最多 1 次轻量重试

## 2.7 采集器改造点

`DbConfigSnapshotCollectorExecutor` 扩展为：

1. 识别 `query / execute_query / get_config`
2. 支持 `structuredContent`、`content[].text`、直接 JSON 对象三种返回形态
3. 对表格型结果做 TopN 摘要，而不是把原始全文塞入 evidence
4. 对采不到的数据输出“结构化缺失项”
5. 区分 `configuration / runtimeMetric / hostContext / observability` 四类 evidence
6. 对宿主资源工具采集结果做标准化单位转换
7. 为每个采集项附带 `capturedAt / isCached / sourceScope / collectionMethod`

## 3. evidence 模型扩展

## 3.1 `DbConfigEvidenceItem`

建议扩展字段：

1. `sourceType`
2. `reference`
3. `description`
4. `category`
   `configuration / runtimeMetric / hostContext / observability`
5. `rawValue`
6. `normalizedValue`
7. `unit`
8. `sourceScope`
   `db / host / container / managed-service / workflow-derived`
9. `capturedAt`
10. `isCached`
11. `collectionMethod`

## 3.2 `DbConfigEvidencePack`

建议扩展：

1. `ConfigurationItems`
2. `RuntimeMetricItems`
3. `HostContextItems`
4. `ObservabilityItems`
5. `MissingContextItems`
6. `Warnings`
7. `CollectionMetadata`

## 3.3 `DbConfigRecommendation`

建议扩展字段：

1. `Key`
2. `Suggestion`
3. `Severity`
4. `FindingType`
5. `Confidence`
6. `RequiresMoreContext`
7. `EvidenceReferences`
8. `ImpactSummary`
9. `RecommendationClass`
10. `AppliesWhen`
11. `RuleId`
12. `RuleVersion`

## 4. 规则引擎设计

## 4.1 规则分组

### MySQL

1. `buffer-pool`
2. `connections`
3. `threading`
4. `tmp-table`
5. `slow-query`
6. `observability`

### PostgreSQL

1. `shared-memory`
2. `planner-cost`
3. `checkpoint`
4. `temp-io`
5. `connections`
6. `observability`

## 4.2 规则执行接口

建议新增：

```csharp
public interface IDbConfigRule
{
    string RuleId { get; }
    string RuleVersion { get; }
    string Engine { get; }
    ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(
        DbConfigCollectedContext context,
        CancellationToken cancellationToken = default);
}
```

`DbConfigRuleAnalysisExecutor` 负责：

1. 构建 `DbConfigCollectedContext`
2. 选择匹配当前引擎的规则集
3. 合并 recommendation
4. 构建 expanded evidence pack
5. 把 `requiresMoreContext` 和 `missingContextItems` 一起回写到 recommendation

## 4.3 规则示例

### MySQL `innodb_buffer_pool_size`

触发条件：

1. 已采到当前值
2. 已采到实例可用内存
3. 当前值明显偏低

输出：

1. `findingType = cache`
2. `confidence = high`
3. `requiresMoreContext = false`
4. `recommendationClass = tuning`
5. `appliesWhen = 已确认实例可用内存明显高于当前 buffer pool`

如果缺少总内存，只输出：

1. `confidence = medium`
2. `requiresMoreContext = true`
3. `recommendationClass = capacity-planning`

### PostgreSQL `shared_buffers`

触发条件：

1. 已采到 `shared_buffers`
2. 已采到总内存或缓存命中指标

输出：

1. 如果缓存命中长期偏低且值明显偏小  
   输出“建议上调”
2. 如果缺少缓存命中  
   输出“建议结合命中率继续评估”

### MySQL `max_connections`

触发条件：

1. 已采到 `max_connections`
2. 已采到 `Threads_connected / Threads_running / Connections`

输出思路：

1. 如果参数上限较高但实际使用长期很低  
   输出“上限偏保守，可评估是否收敛”
2. 如果实际连接高水位逼近上限  
   输出“连接容量风险”
3. 如果缺少压力指标  
   只输出保守建议

### PostgreSQL `checkpoint`

触发条件：

1. 已采到 `checkpoint_timeout / checkpoint_completion_target`
2. 已采到 `pg_stat_bgwriter` 摘要

输出思路：

1. checkpoint 过于频繁时输出写放大风险
2. 缺少 bgwriter 指标时降级为保守建议

### `observability-gap`

如果：

1. MySQL 没有有效 slow query 背景
2. PostgreSQL 没有 `pg_stat_statements`

则输出：

1. `findingType = observability-gap`
2. `severity = warning`
3. `requiresMoreContext = false`
4. `recommendationClass = observability`

## 5. grounding 设计

`DbConfigGroundingExecutor` 要从“建议 key 只匹配单个 reference”升级为：

1. 支持 `EvidenceReferences` 数组
2. 支持配置项 + 运行指标联合引用
3. 支持 `requiresMoreContext = true` 的保守建议
4. 支持 `RuleId / RuleVersion` 透传到最终结果

校验规则：

1. 每条 recommendation 至少引用一个 evidence
2. `requiresMoreContext = true` 时允许证据链不闭合到“最终值”，但必须闭合到“触发背景”
3. `observability-gap` 建议允许引用“缺失上下文项”
4. 对宿主资源类建议，优先引用标准化字段而不是原始文本块

## 6. agent 设计

## 6.1 Prompt 输入

`PromptInputBuilder` 扩展为分块输入：

1. configuration summary
2. runtime metrics summary
3. host context summary
4. observability summary
5. rule candidates
6. missing context summary

建议增加两条提示约束：

1. 缺少 host context 时，不得给出“推荐值”类结论
2. 对同一建议必须同时解释“触发原因”和“适用前提”

## 6.2 输出合约

agent 输出 JSON 至少包含：

```json
{
  "title": "...",
  "summary": "...",
  "recommendations": [
    {
      "key": "...",
      "suggestion": "...",
      "severity": "medium",
      "findingType": "cache",
      "confidence": "high",
      "requiresMoreContext": false,
      "recommendationClass": "tuning",
      "appliesWhen": "实例可用内存充足，且当前 buffer pool 明显偏低",
      "impactSummary": "...",
      "ruleId": "mysql.buffer-pool.v1",
      "ruleVersion": "2026-04-29",
      "evidenceReferences": ["innodb_buffer_pool_size", "memory_limit_bytes"]
    }
  ],
  "evidenceItems": [],
  "warnings": []
}
```

## 6.3 模型约束

继续沿用：

1. DeepSeek / Qwen 使用 `ChatResponseFormat.Json`
2. 手动反序列化
3. schema validator 二次校验
4. 不允许 fallback 到确定性报告

## 7. 前端设计

## 7.1 Result 展示增强

新增展示：

1. recommendation evidence refs
2. confidence
3. findingType
4. requiresMoreContext
5. impact summary
6. appliesWhen
7. recommendationClass
8. ruleId / ruleVersion

## 7.2 History 展示增强

新增展示：

1. 采集到的 configuration / runtime / host / observability 摘要
2. 缺失上下文项
3. 规则输出原始结构
4. 宿主资源上下文摘要
5. 每个 evidence 的采集时间与是否缓存

## 8. 测试设计

## 8.1 单元测试

新增覆盖：

1. MySQL `execute_query` 真实结构解析
2. PostgreSQL `query` 真实结构解析
3. `content[].text` 中嵌套 JSON 解析
4. host context 缺失时的降级规则
5. `observability-gap` 规则
6. grounding 对多 evidence refs 的校验
7. 容器 limit 优先于宿主总内存的规则选择
8. 规则版本透传

## 8.2 集成测试

新增覆盖：

1. MySQL 真实 query -> review -> approve -> completed
2. PostgreSQL 真实 query -> review -> approve -> completed
3. slow-query / pg_stat_statements 缺失时仍能输出保守建议
4. history detail 能看到扩展 evidence
5. 宿主上下文缺失时 recommendation 标记 `requiresMoreContext = true`
6. 托管数据库场景能输出 `managed-service` 作用域

## 8.3 人工验收

重点看：

1. 建议是否仍然只是泛化文案
2. 是否已带上真实采集值
3. 是否对缺失上下文做明确降级说明
4. 是否明确区分“参数调优建议”和“观测缺口建议”
5. 是否能解释宿主资源来源与可信度
