using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour, IControllable
{
    #region Fields:Serialized

    [SerializeField, TabGroup("Stats"), ProgressBar(0, 10), LabelText(SdfIconType.Speedometer)] 
    private float _speed = 1f;

    #endregion

    #region Fields:private

    private Transform _transform;
    
    #endregion

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    public void Move(Vector2 dir)
    {
        // Controller를 통해 움직이기
        _transform.Translate(dir * _speed * Time.deltaTime);
    }
}
