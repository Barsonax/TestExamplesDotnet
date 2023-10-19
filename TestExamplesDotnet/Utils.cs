namespace TestExamplesDotnet;

public static class Utils
{
    public static void RunWithoutSynchronizationContext(Action action)
    {
        // Capture the current synchronization context so we can restore it later.
        // We don't have to be afraid of other threads here as this is a ThreadStatic.
        var synchronizationContext = SynchronizationContext.Current;
        try
        {
            SynchronizationContext.SetSynchronizationContext(null);
            action();
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(synchronizationContext);
        }
    }
}
