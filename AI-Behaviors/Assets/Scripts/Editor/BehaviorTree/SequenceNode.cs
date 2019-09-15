using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SequenceNode : TaskNode
{
    // Private variables
    private Sprite m_sprite;

    private void OnEnable()
    {
        nodeName = "NewSequence";
        nodeType = NodeType.Sequence;

        string spritePath = "Sprites/spr_sequenceNode";

        m_sprite = Resources.Load<Sprite>(spritePath);
    }

    public override void DrawNode()
    {
        nodeName = EditorGUILayout.TextField("Title", nodeName);

        GUI.DrawTexture(new Rect(0, 10, 200, 100), m_sprite.texture, ScaleMode.StretchToFill, true, 10.0F);
    }

    public override bool CanMakeTransition()
    {
        return true;
    }
}