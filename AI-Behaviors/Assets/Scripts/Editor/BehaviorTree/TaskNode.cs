using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum NodeType
{
    Task,
    Sequence,
    Selector
}

public class TaskNode : BaseNode
{
    // Public variables
    public NodeType nodeType;

    private void OnEnable()
    {
        nodeName = "NewTask";
        nodeType = NodeType.Task;
    }

    public override void DrawNode()
    {
        nodeName = EditorGUILayout.TextField("Title", nodeName);
    }

    
    public override bool CanMakeTransition()
    {
        return false;
    }

    protected override bool CanRecieveInput()
    {
        return inputTransitions.Count < 1;
    }
}
