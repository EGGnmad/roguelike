using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Character
{
    [SelectionBase, RequireComponent(typeof(Rigidbody2D))]
    public class Character : MonoBehaviour, IControllable, IDamagable
    {
        #region Fields:Serialized

        [TabGroup("Stats"), HideLabel] public CharacterStats stats;

        #endregion
        
        #region Fields:Control

        private Rigidbody2D _rigid;
        private SpriteRenderer _sr;
        private Vector2 _force;
    
        #endregion

        #region Methods:Control

        public virtual void Move(Vector2 dir)
        {
            _force = dir * stats.speed;
        }

        public void FixedUpdate()
        {
            _rigid.MovePosition(_rigid.position + _force * Time.fixedDeltaTime);

            // 방향 바꾸기
            if (!_sr) return;
            if (_force.x > 0.5f) _sr.flipX = false;
            if (_force.x < -0.5f) _sr.flipX = true;
        }

        #endregion

        #region Methods:Damage

        public virtual void GetDamage(int damage)
        {
            stats.Hp = stats.Hp - damage;
            if (stats.Hp <= 0)
            {
                Died();
            }
        }

        protected virtual void Died()
        {
            
        }

        #endregion

        #region Methods:Unity

        protected virtual void Start()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
        }

        #endregion
    }
}