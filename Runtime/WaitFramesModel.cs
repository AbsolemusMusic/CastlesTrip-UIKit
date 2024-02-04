using System;
using System.Collections;

public class WaitFramesModel
{
    private int frameCount;
    private IEnumerator waiter;

    public WaitFramesModel(int frameCount)
    {
        frameCount = Math.Clamp(frameCount, 1, 24);
        this.frameCount = frameCount;
    }

    public void Start(Action completion)
    {
        Stop();
        waiter = Waiter(completion);
        Coroutines.StartRoutine(waiter);
    }

    public void Stop()
    {
        if (waiter == null)
            return;

        Coroutines.StopRoutine(waiter);
        waiter = null;
    }

    private IEnumerator Waiter(Action completion)
    {
        for (int i = 0; i < frameCount; i++)
            yield return null;

        completion?.Invoke();
        Stop();
    }
}
