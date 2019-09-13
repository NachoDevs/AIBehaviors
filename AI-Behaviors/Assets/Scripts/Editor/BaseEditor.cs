using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using SimpleJSON;
using System.IO;

public class BaseEditor : EditorWindow
{
    protected BaseNode m_entryPoint;
    protected BaseNode m_selectedNode;

    protected List<BaseNode> m_nodes = new List<BaseNode>();

    protected Vector2 m_mousePos;

    protected bool canMakeTransition = false;

    protected void OnGUI()
    {
        bool hasClickedOnNode = false;

        int selectIndex = -1;

        Event e = Event.current;

        m_mousePos = e.mousePosition;

        for (int i = 0; i < m_nodes.Count; ++i)
        {
            if (m_nodes[i].nodeRect.Contains(m_mousePos))
            {
                selectIndex = i;
                hasClickedOnNode = true;
                break;
            }
        }

        if (e.button == 1 && !canMakeTransition)
        {
            if (e.type == EventType.MouseDown)
            {
                GenerateGenericMenu(hasClickedOnNode);

                e.Use();

            }
        }
        else if (e.button == 0 && e.type == EventType.MouseDown && canMakeTransition)
        {
            if (hasClickedOnNode && !m_nodes[selectIndex].Equals(m_selectedNode))
            {
                m_nodes[selectIndex].SetInput(m_selectedNode, m_mousePos);
            }

            canMakeTransition = false;
            m_selectedNode = null;

            e.Use();
        }
        else if (e.button == 0 && e.type == EventType.MouseDown && !canMakeTransition)
        {

            if (hasClickedOnNode)
            {
                BaseNode nodeToChange = m_nodes[selectIndex];

                if (nodeToChange != null)
                {
                    m_selectedNode = nodeToChange;
                    canMakeTransition = true;
                }
            }
        }

        if (canMakeTransition && m_selectedNode != null)
        {
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);

            DrawNodeTransitionLine(m_selectedNode.nodeRect, mouseRect);

            Repaint();
        }

        foreach (BaseNode node in m_nodes)
        {
            node.DrawTransitions();
        }

        BeginWindows();

        for (int i = 0; i < m_nodes.Count; ++i)
        {
            // Definig background color for the node
            GUI.color = (m_nodes[i] == m_entryPoint) ? Color.green : new Color(193, 123, 193);

            m_nodes[i].nodeRect = GUI.Window(i, m_nodes[i].nodeRect, DrawNodeWindow, m_nodes[i].nodeName);
        }

        EndWindows();

        GUI.color = new Color(193, 123, 193);
        if (GUILayout.Button("Export StateMachine"))
        {
            ExportJSON();
        }
    }

    protected virtual void GenerateGenericMenu(bool t_hasClickedOnNode)
    {
    }

    protected void DrawNodeWindow(int t_id)
    {
        m_nodes[t_id].DrawNode();
        GUI.DragWindow();
    }

    protected virtual void ContextCallback(object t_obj)
    {
        bool hasClickedOnNode = false;

        int selectIndex = -1;

        string clb = t_obj.ToString();

        switch (clb)
        {
            default:
            case "stateNode":
                BaseNode newNode = (BaseNode)CreateInstance("BaseNode");
                newNode.nodeRect = new Rect(m_mousePos.x, m_mousePos.y, 200, 100);

                m_nodes.Add(newNode);

                // Creating a default entry point
                if (m_nodes.Count == 1)
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

                if (hasClickedOnNode)
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
                    if (selectedNode == m_entryPoint)
                    {
                        if (m_nodes.Count > 0)
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

    public static void DrawNodeTransitionLine(Rect t_start, Rect t_end)
    {
        Vector3 startPos = new Vector3(t_start.x + t_start.width / 2, t_start.y + t_start.height / 2, 0);
        Vector3 endPos = new Vector3(t_end.x + t_end.width / 2, t_end.y + t_end.height / 2, 0);
        Handles.DrawLine(startPos, endPos);
    }

    protected virtual void ExportJSON()
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