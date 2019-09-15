public class BrainTransition
{
    // Public variables
    public bool isTriggered;

    public BrainNode targetState;

    public BrainAction transitionAction;

    public void ExecuteTransition()
    {
        transitionAction?.Invoke();

        targetState.Execute();
    }

}
