using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class GameManager : MonoBehaviour
{
    [Inject] private MapGenerator _mapGenerator;
    
    void Start()
    {
        UniTask.Create(_mapGenerator.Generate);
        ShowLoadingScene();
    }

    public void ShowLoadingScene()
    {
        LoadingManager.StartLoadingScene(_mapGenerator);
    }
}
