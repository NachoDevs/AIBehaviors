using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;
using System;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;

public class Brain : MonoBehaviour
{
    // Public variables
    public BrainNode sm_initialState;
    public BrainNode sm_currentState;
    public BrainNode m_nextState;

    // Private variables
    private Creature m_creature;

    private Dictionary<string, BrainNode> sm_states;
    private Dictionary<string, BrainAction> sm_actions;

    private Dictionary<string, BrainNode> bt_nodes;

    // Start is called before the first frame update
    private void Start()
    {
        m_creature = GetComponent<Creature>();

        sm_states = new Dictionary<string, BrainNode>();
        bt_nodes = new Dictionary<string, BrainNode>();

        LoadBrain();
        SetUpStateMachine();
    }

    // Update is called once per frame
    private void Update()
    {
        BrainTransition triggeredTransition = null;

        foreach(KeyValuePair<string, BrainTransition> transition in sm_currentState.transitions)
        {
            if(transition.Value.isTriggered)
            {
                triggeredTransition = transition.Value;
                transition.Value.isTriggered = false;
                break;
            }
        }

        if(triggeredTransition != null)
        {
            BrainNode targetState = triggeredTransition.targetState;

            triggeredTransition.ExecuteTransition();

            sm_currentState = targetState;
        }
        else
        {
            sm_currentState.Execute();
            if(m_nextState != null)
            {
                sm_currentState = m_nextState;
                m_nextState = null;
            }
        }
    }

    private void LoadBrain()
    {
        // Read JSON...
        string jsonPath = Application.persistentDataPath + "/StateMachine.json";
        string jsonString = File.ReadAllText(jsonPath);

        JSONObject SMJson = (JSONObject) JSON.Parse(jsonString);

        // Entry point
        BrainState sm_entryState = new BrainState
        {
            nodeName = SMJson["entryNode"]["nodeName"]
        };

        sm_states.Add(sm_entryState.nodeName, sm_entryState);

        sm_initialState = sm_entryState;

        LoadBeaviorTree(sm_entryState);

        // Rest of the states

        foreach (JSONObject state in SMJson["nodes"])
        {
            BrainState newState = new BrainState
            {
                nodeName = state["nodeName"]
            };

            sm_states.Add(newState.nodeName, newState);

            LoadBeaviorTree(newState);
        }

        // Entry point transitions
        foreach (JSONArray jsonTransition in SMJson["entryNode"]["outputs"])
        {
            BrainTransition transition = new BrainTransition
            {
                targetState = sm_states[jsonTransition[1]]
            };

            sm_entryState.transitions.Add(jsonTransition[1], transition);
        }

        // Rest of the transitions
        foreach (JSONObject state in SMJson["nodes"])
        {
            foreach (JSONArray jsonTransition in state["outputs"])
            {
                BrainTransition transition = new BrainTransition
                {
                    targetState = sm_states[jsonTransition[1]]
                };

                sm_states[jsonTransition[0]].transitions.Add(jsonTransition[1], transition);
            }
        }
    }

    private void LoadBeaviorTree(BrainState t_state)
    {
        // Read JSON...
        string jsonPath = Application.persistentDataPath + "/" + t_state.nodeName +".json";
        string jsonString = File.ReadAllText(jsonPath);

        JSONObject SMJson = (JSONObject)JSON.Parse(jsonString);

        BehaviorTree bt = new BehaviorTree
        {
            btName = t_state.nodeName
        };
        bt.bt_initialNode = new BrainNode();
        SelectNodeType(out bt.bt_initialNode, SMJson["entryNode"]["nodeName"], SMJson["entryNode"]["nodeType"]);

        if (!bt_nodes.ContainsKey(bt.bt_initialNode.nodeName))
        {
            bt_nodes.Add(bt.bt_initialNode.nodeName, bt.bt_initialNode);
        }

        // Rest of the states
        foreach (JSONObject btNode in SMJson["nodes"])
        {
            BrainNode newNode = new BrainNode();
            SelectNodeType(out newNode, btNode["nodeName"], btNode["nodeType"]);

            if(!bt_nodes.ContainsKey(newNode.nodeName))
            {
                bt_nodes.Add(newNode.nodeName, newNode);
            }
        }

        // Entry point transitions
        foreach (JSONArray jsonTransition in SMJson["entryNode"]["outputs"])
        {
            BrainTransition transition = new BrainTransition
            {
                targetState = bt_nodes[jsonTransition[1]]
            };

            bt.bt_initialNode.transitions.Add(jsonTransition[1], transition);
        }

        // Rest of the transitions
        foreach (JSONObject state in SMJson["nodes"])
        {
            foreach (JSONArray jsonTransition in state["outputs"])
            {
                BrainTransition transition = new BrainTransition
                {
                    targetState = bt_nodes[jsonTransition[1]]
                };

                bt_nodes[jsonTransition[0]].transitions.Add(jsonTransition[1], transition);
            }
        }

        t_state.behavior = bt;
    }

    private void SelectNodeType(out BrainNode t_node, string t_nodeName, string t_nodeType)
    {
        switch (t_nodeType)
        {
            case "Task":
                t_node = new BrainTask();
                MethodInfo method = typeof(Brain).GetMethod(t_nodeName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                ((BrainTask)t_node).taskAction = (BrainAction)Delegate.CreateDelegate(typeof(BrainAction), this, method);
                break;
            case "Sequence":
                t_node = new BrainSequence();
                break;
            case "Selector":
                t_node = new BrainSelector();
                method = typeof(Brain).GetMethod(t_nodeName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                ((BrainSelector)t_node).condition = (BrainSelectorCondition)Delegate.CreateDelegate(typeof(BrainSelectorCondition), this, method);
                break;
            case "State":
                t_node = new BrainState();
                break;
            default:
                t_node = new BrainNode();
                break;
        }

        t_node.nodeName = t_nodeName;
    }

    public void SetUpStateMachine()
    {
        sm_currentState = sm_initialState;
    }

    private int IdleS()
    {
        return 1;
    }

    private int DrinkingSS()
    {
        return 1;
    }

    private void Drinking()
    {
        print("I am Drinking!!");
    }

    private void Eating()
    {
        print("I am Eating!!");
    }

    private void Idling()
    {
        print("I am Idling!!");
    }

    private void GoDrink()
    {
        m_nextState = sm_states["Drink"];
        print("I am transitioning to Drink!!");
    }

    private void GoEat()
    {
        m_nextState = sm_states["Eat"];
        print("I am transitioning to Eat!!");
    }

    private void GoIdle()
    {
        m_nextState = sm_states["Idle"];
        print("I am transitioning to Idle!!");
    }
}
