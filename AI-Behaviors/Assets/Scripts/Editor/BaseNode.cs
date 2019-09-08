using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseNode : ScriptableObject
{
    public Rect nodeRect; // Location and size of this node

    public string nodeName = "NewState";

    public NodeTransition m_inputTransition;

    private List<NodeTransition> m_transitions;

    public BaseNode()
    {
        m_transitions = new List<NodeTransition>();
    }

    public void DrawNode()
    {
        nodeName = EditorGUILayout.TextField("Title", nodeName);

        GUILayout.Label("Inputs: " + ((m_inputTransition == null) ? 0 : 1));
        GUILayout.Label("Outputs: " + m_transitions.Count);
    }

    public void DrawTransitions()
    {
        if(m_transitions.Count <= 0)
        {
            return;
        }

        Rect inputRect = nodeRect;
        foreach (NodeTransition transition in m_transitions)
        {
            Rect outputRect = transition.toNode.nodeRect;

            StateMachineEditor.DrawNodeCurve(inputRect, outputRect);
        }

    }

    public void SetInput(BaseNode t_input, Vector2 t_mousePos)
    {
        // We are at the recieving end of the transition here

        if(m_inputTransition != null)
        {
            return;
        }

        // We add our input transition
        m_inputTransition = (NodeTransition) CreateInstance("NodeTransition");
        m_inputTransition.fromNode = t_input;
        m_inputTransition.toNode = this;

        // We add to our input the same transition
        t_input.m_transitions.Add(m_inputTransition);

    }

    public void NodeDeleted(BaseNode t_node)
    {
        if(m_inputTransition != null)
        {
            foreach (NodeTransition transition in t_node.m_transitions)
            {
                if(transition.toNode == this)
                {
                    t_node.m_transitions.Remove(transition);
                    break;
                }
            }
        }

        foreach (NodeTransition transition in m_transitions)
        {
            transition.toNode.m_inputTransition = null;
        }
        m_transitions.Clear();
    }

    //public virtual BaseInputNode ClickedOnInput(Vector2 t_pos)
    //{
        //return null;
    //}
}
