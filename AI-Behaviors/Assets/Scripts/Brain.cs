using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    // Public variables

    public List<SM_State> states;

    public SM_State initialState;

    public SM_State currentState;


    // Private variables

    private Creature m_creature;

    List<SM_Actionn> actions;

    // Start is called before the first frame update
    private void Start()
    {
        m_creature = GetComponent<Creature>();

        actions = new List<SM_Actionn>();

        LoadStateMachine();
        SetUpStateMachine();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            currentState.transitions[0].isTriggered = true;
        }


        SM_Transition triggeredTransition = null;

        foreach(SM_Transition transition in currentState.transitions)
        {
            if(transition.isTriggered)
            {
                triggeredTransition = transition;
                transition.isTriggered = false;
                break;
            }
        }

        if(triggeredTransition != null)
        {
            SM_State targetState = triggeredTransition.targetState;

            actions.Clear();
            actions.Add(currentState.stateAction);

            triggeredTransition.transitionAction();

            currentState = targetState;

        }

        foreach(SM_Actionn action in actions)
        {
            action();
        }
    }

    private void LoadStateMachine()
    {
        // Read JSON...

        SM_State jumpState = new SM_State
        {
            stateAction = JumpAction
        };

        SM_State eatState = new SM_State
        {
            stateAction = EatAction
        };

        SM_Transition jumpEat = new SM_Transition
        {
            targetState = eatState,
            transitionAction = EatTransitionAction
        };
        jumpState.transitions.Add(jumpEat);
        SM_Transition eatJump = new SM_Transition
        {
            targetState = jumpState,
            transitionAction = JumpTransitionAction
        };
        eatState.transitions.Add(eatJump);

        initialState = jumpState;
    }

    private void SetUpStateMachine()
    {
        currentState = initialState;
        actions.Add(currentState.stateAction);

    }

    private void JumpAction()
    {
        print("I am jumping!!");
    }

    private void JumpTransitionAction()
    {
        print("I am transitioning to jumping!!");
    }

    private void EatTransitionAction()
    {
        print("I am transitioning to eating!!");
    }

    private void EatAction()
    {
        print("I am eating!!");
    }
}
