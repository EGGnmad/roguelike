using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string newScene)
    {
        LoadingManager.LoadScene(newScene);
    }
}
