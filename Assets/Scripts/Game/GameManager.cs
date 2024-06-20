using Cysharp.Threading.Tasks;
using MapGeneration;
using UnityEngine;
using UnityEngine.Events;
using VContainer;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    #region Fields:Serialzed

    public UnityEvent<GlobalMap> mapGenerationCompleted;

    #endregion
    
    [Inject] private MapGenerator _mapGenerator;
    [Inject] private GlobalMap _globalMap;
    
    async void Start()
    {
        // 맵 생성
        LoadingManager.StartLoadingScene(_mapGenerator); // 맵 생성 로딩 화면 보여주기
        await UniTask.Create(_mapGenerator.Generate);
        
        mapGenerationCompleted.Invoke(_globalMap);
    }
}
