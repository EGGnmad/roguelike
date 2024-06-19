using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class GameManager : MonoBehaviour
{
    [Inject] private MapGenerator _mapGenerator;
    
    async void Start()
    {
        LoadingManager.StartLoadingScene(_mapGenerator); // 맵 생성 로딩 화면 보여주기
        await UniTask.Create(_mapGenerator.Generate);
    }
}
