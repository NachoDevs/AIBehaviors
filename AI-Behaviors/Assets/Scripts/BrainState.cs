using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainState : BrainNode
{
    // Public variables
    public BehaviorTree behavior;

    public BrainState()
    {
        behavior = new BehaviorTree();
    }

    public override void Execute()
    {
        behavior.StartBehavior();
    }
}
