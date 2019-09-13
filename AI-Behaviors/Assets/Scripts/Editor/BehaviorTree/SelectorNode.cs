using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectorNode : BaseNode
{
    // Private variables
    private Sprite m_sprite;

    public SelectorNode() : base()
    {
        string spritePath = "Sprites/spr_selectorNode";

        m_sprite = Resources.Load<Sprite>(spritePath);
    }

    public override void DrawNode()
    {
        nodeName = EditorGUILayout.TextField("Title", nodeName);

        GUI.DrawTexture(new Rect(10, 10, 60, 60), m_sprite.texture, ScaleMode.ScaleToFit, true, 10.0F);
    }
}