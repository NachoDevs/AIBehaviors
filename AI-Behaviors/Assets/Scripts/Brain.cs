using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;

public class Brain : MonoBehaviour
{
    // Public variables
    public SM_State initialState;

    public SM_State currentState;


    // Private variables
    private Creature m_creature;

    private List<SM_Action> m_actionsToPerform;

    private Dictionary<string, SM_State> m_smStates;
    private Dictionary<string, SM_Action> m_smActions;

    // Start is called before the first frame update
    private void Start()
    {
        m_creature = GetComponent<Creature>();

        m_actionsToPerform = new List<SM_Action>();

        m_smStates = new Dictionary<string, SM_State>();

        LoadStateMachine();
        SetUpStateMachine();
    }

    // Update is called once per frame
    private void Update()
    {
        SM_Transition triggeredTransition = null;

        foreach(KeyValuePair<string, SM_Transition> transition in currentState.transitions)
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
            SM_State targetState = triggeredTransition.targetState;

            m_actionsToPerform.Clear();
            m_actionsToPerform.Add(currentState.stateAction);

            triggeredTransition.transitionAction();

            currentState = targetState;

        }

        foreach(SM_Action action in m_actionsToPerform)
        {
            action?.Invoke();
        }
    }

    private void LoadStateMachine()
    {
        // Read JSON...
        string jsonPath = Application.persistentDataPath + "/StateMachine.json";
        string jsonString = File.ReadAllText(jsonPath);

        JSONObject SMJson = (JSONObject) JSON.Parse(jsonString);

        // Entry point
        SM_State entryState = new SM_State();

        m_smStates.Add(SMJson["entryNode"]["nodeName"], entryState);

        initialState = entryState;


        // Rest of the states

        foreach (JSONObject state in SMJson["nodes"])
        {
            SM_State newState = new SM_State();

            m_smStates.Add(state["nodeName"], newState);
        }

        // Entry point transitions
        foreach (JSONArray jsonTransition in SMJson["entryNode"]["outputs"])
        {
            SM_Transition transition = new SM_Transition
            {
                targetState = m_smStates[jsonTransition[1]]
            };

            entryState.transitions.Add(jsonTransition[1], transition);
        }

        // Rest of the transitions
        foreach (JSONObject state in SMJson["nodes"])
        {
            foreach (JSONArray jsonTransition in state["outputs"])
            {
                SM_Transition transition = new SM_Transition
                {
                    targetState = m_smStates[jsonTransition[1]]
                };

                m_smStates[jsonTransition[0]].transitions.Add(jsonTransition[1], transition);
            }
        }
    }

    private void SetUpStateMachine()
    {
        currentState = initialState;
        m_actionsToPerform.Add(currentState.stateAction);

    }

    private void JumpAction()
    {
        //print("I am jumping!!");
    }

    private void JumpTransitionAction()
    {
        //print("I am transitioning to jumping!!");
    }

    private void EatTransitionAction()
    {
        //print("I am transitioning to eating!!");
    }

    private void EatAction()
    {
        //print("I am eating!!");
    }
}
