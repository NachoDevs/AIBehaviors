using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public delegate void SM_Actionn();

public class SM_State
{
    // Public variables
    public List<SM_Transition> transitions;

    public SM_Actionn stateAction;

    public SM_State()
    {
        transitions = new List<SM_Transition>();
    }
}
