using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TaskNode : BaseNode
{
    private void OnEnable()
    {
        nodeName = "NewTask";
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
