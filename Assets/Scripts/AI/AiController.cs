using System;
using AI;
using Game.Character;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class AiController : MonoBehaviour, IController
{
    #region Fields:Control
    
    private IControllable _character;
    private ContextMap _cm;
    
    #endregion

    #region Fields:Detect

    public float detectRadius = 3f;

    #endregion

    #region Methods:Control

    public void ControlStart(IControllable character)
    {
        _character = character;
    }

    #endregion

    private void Awake()
    {
        _cm = new ContextMap(16);
    }

    private void Start()
    {
        _character = GetComponent<IControllable>();
    }

    private void Update()
    {
        // 캐릭터
        var colliders = Physics2D.OverlapCircleAll(transform.position, detectRadius, LayerMask.GetMask("Character"));
        foreach (var obj in colliders)
        {
            if(obj.gameObject == gameObject) continue;
            
            if (obj.CompareTag("Player"))
            {
                _cm.Add(obj.transform.position - transform.position, ContextMap.Mode.Interest);
            }
            else if (obj.CompareTag("Enemy"))
            {
                if(Vector2.Distance(transform.position, obj.transform.position) > 1f) continue;
                _cm.Add(obj.transform.position - transform.position, ContextMap.Mode.Danger);
            }
        }

        _character.Move(_cm.GetDestination());
        Debug.DrawRay(transform.position, _cm.GetDestination(), Color.red);
        _cm.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
