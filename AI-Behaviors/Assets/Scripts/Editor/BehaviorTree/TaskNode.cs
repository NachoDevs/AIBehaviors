using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TaskNode : BaseNode
{
    public override void DrawNode()
    {
        nodeName = EditorGUILayout.TextField("Title", nodeName);
    }
}
