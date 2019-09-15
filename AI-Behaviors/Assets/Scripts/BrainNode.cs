using System.Collections.Generic;


public class BrainNode
{
    // Public variables
    public string nodeName;

    public Dictionary<string, BrainTransition> transitions;

    public BrainNode()
    {
        transitions = new Dictionary<string, BrainTransition>();
    }

    public virtual void Execute()
    {

    }
}