public delegate void BrainAction();

public class BrainTask : BrainNode
{
    // Public variables
    public BrainAction taskAction;

    public override void Execute()
    {
        taskAction?.Invoke();
    }
}
