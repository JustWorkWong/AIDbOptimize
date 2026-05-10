using AIDbOptimize.Infrastructure.Workflows.Runtime;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class WorkflowProgressCalculatorTests
{
    [Fact]
    public void GetProgressPercent_ReturnsStableProgress_ForSkillPolicyGateNode()
    {
        var progress = WorkflowProgressCalculator.GetProgressPercent(
            "SkillPolicyGate",
            "Running");

        Assert.Equal(55, progress);
    }

    [Fact]
    public void GetProgressPercent_ReturnsStableProgress_ForWorkflowRagContextAssemblerNode()
    {
        var progress = WorkflowProgressCalculator.GetProgressPercent(
            "WorkflowRagContextAssembler",
            "Running");

        Assert.Equal(62, progress);
    }
}
