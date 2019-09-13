using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using SimpleJSON;
using System.IO;

public class StateMachineEditor : BaseEditor
{
    [MenuItem("Window/AI-Behaviors/StateMachineEditor")]
    protected static void ShowEditor()
    {
        BaseEditor editor = GetWindow<StateMachineEditor>();
    }

    protected override void GenerateGenericMenu(bool t_hasClickedOnNode)
    {
        GenericMenu menu = new GenericMenu();
        if (!t_hasClickedOnNode)
        {

            menu.AddItem(new GUIContent("Add State"), false, ContextCallback, "stateNode");
        }
        else
        {
            menu.AddItem(new GUIContent("Make Transition"), false, ContextCallback, "makeTransition");

            menu.AddSeparator("");

            menu.AddItem(new GUIContent("Delete State"), false, ContextCallback, "deleteState");
        }
        menu.ShowAsContext();
    }

    protected override void ContextCallback(object t_obj)
    {
        bool hasClickedOnNode = false;

        int selectIndex = -1;

        string clb = t_obj.ToString();

        switch (clb)
        {
            default:
            case "stateNode":
                BaseNode newNode = (BaseNode) CreateInstance("BaseNode");
                newNode.nodeRect = new Rect(m_mousePos.x, m_mousePos.y, 200, 100);
                
                m_nodes.Add(newNode);

                // Creating a default entry point
                if(m_nodes.Count == 1)
                {
                    m_entryPoint = newNode;
                }

                break;
            case "makeTransition":

                for (int i = 0; i < m_nodes.Count; ++i)
                {
                    if (m_nodes[i].nodeRect.Contains(m_mousePos))
                    {
                        selectIndex = i;
                        hasClickedOnNode = true;
                        break;
                    }
                }

                if(hasClickedOnNode)
                {
                    m_selectedNode = m_nodes[selectIndex];
                    canMakeTransition = true;
                }

                break;
            case "deleteState":

                for (int i = 0; i < m_nodes.Count; ++i)
                {
                    if (m_nodes[i].nodeRect.Contains(m_mousePos))
                    {
                        selectIndex = i;
                        hasClickedOnNode = true;
                        break;
                    }
                }

                if (hasClickedOnNode)
                {
                    BaseNode selectedNode = m_nodes[selectIndex];
                    m_nodes.RemoveAt(selectIndex);

                    // Assign a new entrypoint if the actual one is deleted
                    if(selectedNode == m_entryPoint)
                    {
                        if(m_nodes.Count > 0)
                        {
                            m_entryPoint = m_nodes[0];
                        }
                    }

                    foreach (BaseNode node in m_nodes)
                    {
                        node.NodeDeleted(selectedNode);
                    }
                }


                break;
        }
    }

    protected override void ExportJSON()
    {
        JSONObject allNodes = new JSONObject();

        JSONObject entryNode = new JSONObject();
        JSONArray allNodesWithoutEntry = new JSONArray();

        foreach (BaseNode node in m_nodes)
        {
            JSONArray inputs = new JSONArray();

            foreach (NodeTransition nt in node.inputTransitions)
            {
                JSONArray inputTransition = new JSONArray();

                inputTransition.Add(nt.fromNode.nodeName);
                inputTransition.Add(nt.toNode.nodeName);

                inputs.Add(inputTransition);
            }

            JSONArray outputs = new JSONArray();

            foreach (NodeTransition nt in node.outputTransitions)
            {
                JSONArray outputTransition = new JSONArray();

                outputTransition.Add(nt.fromNode.nodeName);
                outputTransition.Add(nt.toNode.nodeName);

                outputs.Add(outputTransition);
            }

            JSONObject jsonObject = new JSONObject();
            jsonObject.Add("nodeName", node.nodeName);
            jsonObject.Add("inputs", inputs);
            jsonObject.Add("outputs", outputs);

            if (node == m_entryPoint)
            {
                entryNode = jsonObject;
            }
            else
            {
                allNodesWithoutEntry.Add(jsonObject);
            }
        }

        allNodes.Add("entryNode", entryNode);
        allNodes.Add("nodes", allNodesWithoutEntry);

        string jsonPath = Application.persistentDataPath + "/StateMachine.json";
        File.WriteAllText(jsonPath, allNodes.ToString());
    }
}