using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase, RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IControllable
{
    #region Fields:Serialized
    
    [TabGroup("Stats"), HideLabel] public CharacterStats stats;

    #endregion

    #region Fields:private

    private Rigidbody2D _rigid;
    private SpriteRenderer _sr;
    private Vector2 _force;
    
    #endregion

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    public void Move(Vector2 dir)
    {
        _force = dir * stats.speed;
    }

    public void FixedUpdate()
    {
        _rigid.MovePosition(_rigid.position + _force * Time.fixedDeltaTime);

        // 방향 바꾸기
        if (!_sr) return;
        if (_force.x > 0) _sr.flipX = false;
        if (_force.x < 0) _sr.flipX = true;
    }
}
