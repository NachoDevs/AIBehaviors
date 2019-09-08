using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StateMachineEditor : EditorWindow
{
    private List<BaseNode> m_nodes = new List<BaseNode>();

    private Vector2 m_mousePos;

    private BaseNode m_selectedNode;

    private bool canMakeTransition = false;

    [MenuItem("Window/AI-Behaviors/StateMachineEditor")]
    private static void ShowEditor()
    {
        StateMachineEditor editor = GetWindow<StateMachineEditor>();
    }

    private void OnGUI()
    {
        bool hasClickedOnNode = false;

        int selectIndex = -1;

        Event e = Event.current;

        m_mousePos = e.mousePosition;

        for (int i = 0; i < m_nodes.Count; ++i)
        {
            if(m_nodes[i].nodeRect.Contains(m_mousePos))
            {
                selectIndex = i;
                hasClickedOnNode = true;
                break;
            }
        }

        if(e.button == 1 && !canMakeTransition)
        {
            if(e.type == EventType.MouseDown)
            {

                GenericMenu menu = new GenericMenu();
                if (!hasClickedOnNode)
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
                e.Use();

            }
        }
        else if(e.button == 0 && e.type == EventType.MouseDown && canMakeTransition)
        {
            if(hasClickedOnNode && !m_nodes[selectIndex].Equals(m_selectedNode))
            {
                m_nodes[selectIndex].SetInput(m_selectedNode, m_mousePos);
            }

            canMakeTransition = false;
            m_selectedNode = null;

            e.Use();
        }
        else if(e.button == 0 && e.type == EventType.MouseDown && !canMakeTransition)
        {

            if(hasClickedOnNode)
            {
                BaseNode nodeToChange = m_nodes[selectIndex];

                if(nodeToChange != null)
                {
                    m_selectedNode = nodeToChange;
                    canMakeTransition = true;
                }
            }
        }

        if(canMakeTransition && m_selectedNode != null)
        {
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);

            DrawNodeCurve(m_selectedNode.nodeRect, mouseRect);

            Repaint();
        }

        foreach (BaseNode node in m_nodes)
        {
            node.DrawTransitions();
        }

        BeginWindows();

        for (int i = 0; i < m_nodes.Count; ++i)
        {
            m_nodes[i].nodeRect = GUI.Window(i, m_nodes[i].nodeRect, DrawNodeWindow, m_nodes[i].nodeName);
        }

        EndWindows();
    }

    private void DrawNodeWindow(int t_id)
    {
        m_nodes[t_id].DrawNode();
        GUI.DragWindow();
    }

    private void ContextCallback(object t_obj)
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

                    foreach(BaseNode node in m_nodes)
                    {
                        node.NodeDeleted(selectedNode);
                    }
                }

                 break;
        }
    }

    public static void DrawNodeCurve(Rect t_start, Rect t_end)
    {
        Vector3 startPos = new Vector3(t_start.x + t_start.width / 2, t_start.y + t_start.height / 2, 0);
        Vector3 endPos = new Vector3(t_end.x + t_end.width / 2, t_end.y + t_end.height / 2, 0);
        Handles.DrawLine(startPos, endPos);
    }

}
