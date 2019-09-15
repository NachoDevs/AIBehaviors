using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree
{
    // Public variables
    public string btName;

    public BrainNode bt_initialNode;
    //public BrainNode bt_currentState;

    //public Dictionary<string, BrainTask> bt_states;
    //public Dictionary<string, BrainAction> bt_actions;

    //public BehaviorTree()
    //{
    //    bt_states = new Dictionary<string, BrainTask>();
    //    bt_actions = new Dictionary<string, BrainAction>();
    //}

    public void StartBehavior()
    {
        bt_initialNode.Execute();
    }
}
