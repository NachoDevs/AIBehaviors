using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseNode : ScriptableObject
{
    public Rect nodeRect; // Location and size of this node

    public string nodeName = "NewState";

    public List<NodeTransition> inputTransitions;

    public List<NodeTransition> outputTransitions;

    public BaseNode()
    {
        inputTransitions = new List<NodeTransition>();
        outputTransitions = new List<NodeTransition>();
    }

    public void DrawNode()
    {
        nodeName = EditorGUILayout.TextField("Title", nodeName);

        GUILayout.Label("Inputs: " + inputTransitions.Count);
        GUILayout.Label("Outputs: " + outputTransitions.Count);
    }

    public void DrawTransitions()
    {
        if(outputTransitions.Count <= 0)
        {
            return;
        }

        Rect inputRect = nodeRect;
        foreach (NodeTransition transition in outputTransitions)
        {
            Rect outputRect = new Rect(transition.toNode.nodeRect);

            if (inputTransitions.Count > 0)
            {
                NodeTransition inputTransition = (NodeTransition)CreateInstance("NodeTransition");
                inputTransition.fromNode = transition.toNode;
                inputTransition.toNode = transition.fromNode;

                if (inputTransitions.Contains(inputTransition))
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

        if(inputTransitions.Count < 0)
        {
            return;
        }

        // We add our input transition
        NodeTransition inputTransition = (NodeTransition) CreateInstance("NodeTransition");
        inputTransition.fromNode = t_input;
        inputTransition.toNode = this;

        // If the transition already exists we don't add it
        if(t_input.ContainsTransition(inputTransition) != null)
        {
            return;
        }

        inputTransitions.Add(inputTransition);

        // We add to our input the same transition
        t_input.outputTransitions.Add(inputTransition);

    }

    public void NodeDeleted(BaseNode t_node)
    {
        foreach(NodeTransition transition in t_node.inputTransitions)
        {
            NodeTransition nTrans = transition.fromNode.ContainsTransition(transition);
            transition.fromNode.outputTransitions.Remove(nTrans);
        }

        t_node.inputTransitions.Clear();
        t_node.outputTransitions.Clear();
    }

    public NodeTransition ContainsTransition(NodeTransition t_newTransition)
    {
        foreach(NodeTransition transition in outputTransitions)
        {
            if (transition.toNode == t_newTransition.toNode)
            {
                return transition;
            }
        }
        return null;
    }
}
