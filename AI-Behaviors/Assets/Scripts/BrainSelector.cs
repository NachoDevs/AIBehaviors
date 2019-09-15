using System.Linq;

public delegate int BrainSelectorCondition();

public class BrainSelector : BrainNode
{
    public BrainSelectorCondition condition;

    public override void Execute()
    {
        // We execute the transition in the position recieved by the condition method
        transitions.ElementAt(condition()).Value.ExecuteTransition();
    }
}
