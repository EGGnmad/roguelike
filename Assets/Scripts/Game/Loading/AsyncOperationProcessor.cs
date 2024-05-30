using UnityEngine;

public class AsyncOperationProcessor : IProcessor
{
    private AsyncOperation _operation;
    
    public AsyncOperationProcessor(AsyncOperation operation)
    {
        _operation = operation;
    }
    
    public string GetProcessName() => "비동기 작업";

    public float GetProgress()
    {
        return _operation.progress;
    }
}