using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public delegate void SM_Action();

public class SM_State
{
    // Public variables
    public Dictionary<string, SM_Transition> transitions;

    public SM_Action stateAction;

    public SM_State()
    {
        transitions = new Dictionary<string, SM_Transition>();
    }
}
