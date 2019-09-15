using System.Collections.Generic;

public class BrainSequence : BrainNode
{
    public override void Execute()
    {
        foreach (KeyValuePair<string, BrainTransition> transition in transitions)
        {
            transition.Value.ExecuteTransition();
        }
    }
}
