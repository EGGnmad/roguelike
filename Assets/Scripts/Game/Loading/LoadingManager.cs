using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadingManager
{
    public static async void StartLoadingScene(IProcessor processor)
    {
        // 씬 로딩
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        await UniTask.DelayFrame(1);

        // 진행도 표시
        foreach (var manager in GameObject.FindGameObjectsWithTag("Manager"))
        {
            manager.GetComponent<LoadingView>()?.StartLoading(processor);
        }
    }

    public static void LoadScene(string newScene)
    {
        var operation = SceneManager.LoadSceneAsync(newScene);
        StartLoadingScene(new AsyncOperationProcessor(operation));
    }
}