using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectorNode : BaseNode
{
    // Private variables
    private Sprite m_sprite;

    private void OnEnable()
    {
        string spritePath = "Sprites/spr_selectorNode";

        m_sprite = Resources.Load<Sprite>(spritePath);
    }

    public override void DrawNode()
    {
        nodeName = EditorGUILayout.TextField("Title", nodeName);

        GUI.DrawTexture(new Rect(0, 10, 200, 100), m_sprite.texture, ScaleMode.StretchToFill, true, 10.0F);
    }
}