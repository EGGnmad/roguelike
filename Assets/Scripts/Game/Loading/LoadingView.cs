using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private Slider _slider;

    private IProcessor _processor;
    
    // 부드러운 로딩 (보간)
    private float _t = 0;
    private float _end = 0; 

    #region Methods:Loading

    public void StartLoading(IProcessor processor)
    {
        _processor = processor;
        _infoText.text = string.Format(_infoText.text, processor.GetProcessName());
    }

    public void StopLoading()
    {
        _processor = null;
        SceneManager.UnloadSceneAsync("LoadingScene");
    }

    #endregion
    
    #region Methods:Unity

    private void Update()
    {
        if (_processor == null) return;
        
        if (Mathf.Approximately(_slider.value, _end))
        {
            _end = _processor.GetProgress();
            _t = 0;
        }
        else
        {
            _t += Time.deltaTime;
            _slider.value = Mathf.Lerp(_slider.value, _end, _t);
        }
        
        if (Mathf.Approximately(_slider.value, 1f))
        {
            StopLoading();
        }
    }

    #endregion
}
