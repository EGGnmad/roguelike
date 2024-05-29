using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class GameManager : MonoBehaviour
{
    [Inject] private MapGenerator _mapGenerator;
    
    void Start()
    {
        UniTask.Create(_mapGenerator.Generate);
        ShowLoadingScene();
    }

    public async void ShowLoadingScene()
    {
        // Init Loading Scene
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        await UniTask.DelayFrame(1);
        
        GameObject.FindWithTag("Manager").GetComponent<LoadingManager>()?.StartLoading(_mapGenerator);
    }
}
