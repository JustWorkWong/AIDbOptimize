# AIDbOptimize 数据库配置优化 workflow 二期任务拆解

## 阶段 1：方案落档与契约扩展

- [x] 新增二期 `design.md`
- [x] 新增二期 `detailed-design.md`
- [x] 新增二期 `tasks.md`
- [x] 确认二期继续复用现有 `DbConfigOptimization` workflow，不新开第二条 workflow
- [x] 明确宿主资源上下文字段合约、单位和来源优先级
- [x] 明确 recommendation 扩展字段：`recommendationClass / appliesWhen / ruleId / ruleVersion`
- [x] 明确远程自建数据库、云托管数据库、容器场景的目标解析与降级策略
- [x] 在 `Domain / Application` 层补齐扩展 evidence / recommendation 合同

验收：
- [x] 方案、详细方案、任务拆解三件套齐全
- [x] 宿主资源采集来源、优先级、降级方式写清楚
- [x] 输出合同不与当前前后端契约冲突

## 阶段 2：采集增强

- [ ] 扩展 `DbConfigSnapshotCollectorExecutor` 支持更多 MySQL 配置项
- [ ] 扩展 `DbConfigSnapshotCollectorExecutor` 支持更多 PostgreSQL 配置项
- [x] 支持 runtime metrics 采集
- [x] 支持 observability snapshot 采集
- [x] 支持 host context 采集
- [x] 新增只读 `HostContext MCP` 工具组设计与接入
- [x] 新增 `resolve_runtime_target`，解决连接到容器/主机/托管实例的映射
- [x] 明确容器资源边界优先于宿主总量的采集优先级
- [x] 支持 `content[].text / structuredContent / JSON object` 多返回形态统一解析
- [ ] 对表格类结果做 TopN 摘要而不是原样全文入库
- [ ] 为 host context 采集增加 `capturedAt / expiresAt / isCached`
- [x] 对 host context 采集失败输出结构化缺失原因：`permission / timeout / unsupported / unresolved-target`

验收：
- [ ] MySQL 能采到 `max_connections / innodb_buffer_pool_size / thread / slow-query` 相关值
- [ ] PostgreSQL 能采到 `shared_buffers / work_mem / checkpoint / cache-hit` 相关值
- [x] 缺失上下文时以结构化缺失项进入 evidence
- [x] 宿主上下文字段能稳定输出 `resource_scope / memory_limit / memory_available / cpu_limit / disk_available`
- [x] 至少一条容器场景被验证
- [ ] 至少一条托管/不可见场景被验证

## 阶段 3：规则引擎增强

- [ ] 新增 MySQL engine-specific 规则集
- [ ] 新增 PostgreSQL engine-specific 规则集
- [ ] 新增 `observability-gap` 类规则
- [ ] 新增 MySQL `max_connections / thread / tmp-table / slow-query` 规则
- [ ] 新增 PostgreSQL `checkpoint / planner-cost / temp-io` 规则
- [ ] 规则结果补齐 `findingType / confidence / requiresMoreContext / impactSummary / evidenceReferences`
- [ ] 规则结果补齐 `recommendationClass / appliesWhen / ruleId / ruleVersion`
- [ ] `DbConfigRuleAnalysisExecutor` 改为规则集合编排
- [ ] 为关键阈值建立版本化常量或配置，并在 history 中可追踪

验收：
- [ ] 建议输出不再只是“重评估某参数”的泛化文案
- [ ] 每条建议都能追到 evidence refs
- [ ] 缺失上下文时能明确降级
- [ ] history detail 能看到命中的规则版本

## 阶段 4：grounding 与 agent 增强

- [ ] `DbConfigGroundingExecutor` 支持多 evidence refs 校验
- [ ] `RecommendationSchemaValidator` 扩展到二期 recommendation 结构
- [ ] `PromptInputBuilder` 扩展为 `configuration / runtime / host / observability / missing-context` 分块输入
- [ ] `DbConfigDiagnosisAgentExecutor` 输出二期 recommendation 结构
- [ ] 保持 DeepSeek / Qwen 的 `ChatResponseFormat.Json + 手动反序列化 + 严格抛错`
- [ ] 缺少宿主资源上下文时，禁止 agent 产出“具体推荐值”式结论
- [ ] 把 `ruleId / ruleVersion` 透传到最终结果和 history

验收：
- [ ] 新 recommendation 结构全部过 schema 校验
- [ ] grounding 不再误杀真实 query 场景
- [ ] agent 输出中包含 `confidence / impact / evidence refs / appliesWhen`

## 阶段 5：前端与 history 展示增强

- [ ] Workflow 结果面板展示 `findingType / confidence / requiresMoreContext / impactSummary`
- [ ] Workflow 结果面板展示 `recommendationClass / appliesWhen / ruleId / ruleVersion`
- [ ] History detail 展示扩展 evidence
- [ ] Replay / Review 面板能展示 richer recommendation 结构
- [ ] 前端展示缺失上下文项与缺失原因
- [ ] 前端展示宿主资源上下文摘要与来源作用域
- [ ] 前端展示 evidence 的 `capturedAt / isCached`

验收：
- [ ] 前端能看出建议是基于真实值还是保守降级
- [ ] History detail 能看出采集值、规则结果、review 决策
- [ ] 前端能区分 `container / host / managed-service / unknown`

## 阶段 6：验证与收尾

- [ ] 新增 MySQL 真实 query 场景集成测试
- [ ] 新增 PostgreSQL 真实 query 场景集成测试
- [x] 新增 richer evidence 结构单测
- [ ] 人工验证 MySQL / PostgreSQL 各至少一条完整闭环
- [ ] 核查控制面库里新增 evidence / 规则字段是否完整
- [ ] 人工验证至少一条“缺少宿主上下文的保守降级”场景
- [ ] 人工验证至少一条托管数据库或不可见宿主场景

验收：
- [ ] 自动化测试通过
- [ ] 至少两条真实闭环会话跑通
- [ ] 建议结果方向正确、证据明确、降级合理
- [ ] 建议结果明确区分 `tuning / observability / capacity-planning / hygiene`
