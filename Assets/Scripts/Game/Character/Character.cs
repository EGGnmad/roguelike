using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Character
{
    [SelectionBase, RequireComponent(typeof(Rigidbody2D))]
    public class Character : MonoBehaviour, IControllable, IDamagable
    {
        #region Fields:Serialized

        [TabGroup("Stats"), HideLabel] public CharacterStats stats;

        public virtual Vector2 LookDirection => _force.normalized;

        #endregion
        
        #region Fields:Control

        protected IController _controller;
        protected Rigidbody2D _rigid;
        protected SpriteRenderer _sr;
        protected Vector2 _force;
    
        #endregion

        #region Methods:Control

        public virtual void ControlStarted(IController controller)
        {
            _controller = controller;
        }
        
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
            else
            {
                SetTint();
            }
        }

        public virtual void Died()
        {
            _controller.StopControl();
            Destroy(gameObject);
        }

        public async void SetTint()
        {
            _sr.color = Color.red;
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            
            if (!_sr) return;
            _sr.color = Color.white;
        }
        
        #endregion

        #region Methods:Unity

        protected virtual void Start()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            
            stats.SetHpToMax();
        }

        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, LookDirection);
        }
    }
}