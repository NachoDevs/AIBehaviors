using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseNode : ScriptableObject
{
    public Rect nodeRect; // Location and size of this node

    public string nodeName = "NewState";

    public List<NodeTransition> m_inputTransitions;

    private List<NodeTransition> m_transitions;

    public BaseNode()
    {
        m_inputTransitions = new List<NodeTransition>();
        m_transitions = new List<NodeTransition>();
    }

    public void DrawNode()
    {
        nodeName = EditorGUILayout.TextField("Title", nodeName);

        GUILayout.Label("Inputs: " + m_inputTransitions.Count);
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
            Rect outputRect = new Rect(transition.toNode.nodeRect);

            if (m_inputTransitions.Count > 0)
            {
                NodeTransition inputTransition = (NodeTransition)CreateInstance("NodeTransition");
                inputTransition.fromNode = transition.toNode;
                inputTransition.toNode = transition.fromNode;

                if (m_inputTransitions.Contains(inputTransition))
                //if (transition.toNode == m_inputTransition.fromNode)
                {
                    outputRect.x -= 20;
                }
            }

            StateMachineEditor.DrawNodeTransitionLine(inputRect, outputRect);
        }

    }

    public void SetInput(BaseNode t_input, Vector2 t_mousePos)
    {
        // We are at the recieving end of the transition here

        if(m_inputTransitions.Count < 0)
        {
            return;
        }

        // We add our input transition
        NodeTransition inputTransition = (NodeTransition) CreateInstance("NodeTransition");
        inputTransition.fromNode = t_input;
        inputTransition.toNode = this;

        // If the transition already exists we don't add it
        if(t_input.ContainsTransition(inputTransition))
        {
            return;
        }

        m_inputTransitions.Add(inputTransition);

        // We add to our input the same transition
        t_input.m_transitions.Add(inputTransition);

    }

    public void NodeDeleted(BaseNode t_node)
    {
        //if(m_inputTransitions.Count > 0 )
        //{
        //    foreach (NodeTransition transition in t_node.m_transitions)
        //    {
        //        if(transition.toNode == this)
        //        {
        //            t_node.m_transitions.Remove(transition);
        //            break;
        //        }
        //    }
        //}

        //foreach (NodeTransition transition in m_transitions)
        //{
        //    transition.toNode.m_inputTransition = null;
        //}
        m_inputTransitions.Clear();
        m_transitions.Clear();
    }

    //public virtual BaseInputNode ClickedOnInput(Vector2 t_pos)
    //{
        //return null;
    //}

    public bool ContainsTransition(NodeTransition t_newTransition)
    {
        foreach(NodeTransition transition in m_transitions)
        {
            //if(transition.fromNode != t_newTransition.fromNode)
            //{
            //    return false;
            //}

            if (transition.toNode == t_newTransition.toNode)
            {
                return true;
            }

        }
        return false;
    }
}
