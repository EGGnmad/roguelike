using System;
using System.Collections.Generic;
using Game.Character;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace AI
{
    public enum AiState
    {
        Patrol,
        Chase,
        Attack
    }
    
    [RequireComponent(typeof(Character))]
    public class AiController : MonoBehaviour, IController, IStateMachine<AiState>
    {
        #region Fields:Control

        [HideInInspector] public IControllable character;
        [HideInInspector] public ContextMap cm;

        #endregion

        #region Fields:Detect

        [Header("AI")]
        [Unit(Units.Meter)] public float detectRadius = 3f;

        #endregion

        #region Fields:FSM

        [Header("FSM")]
        [SerializeField] private AiState _currentStateKey;
        private Dictionary<AiState, IState> _stateMap;

        #endregion

        #region Methods:Control

        public void ControlStart(IControllable c)
        {
            character = c;
            c.ControlStarted(this);
        }

        public void StopControl()
        {
            cm.Clear();
            character?.Move(cm.GetDestination());
            character = null;
        }

        #endregion

        #region Methods:Unity

        private void Awake()
        {
            cm = new ContextMap(8);
            _stateMap = new Dictionary<AiState, IState>();
        }

        private void Start()
        {
            character = GetComponent<IControllable>();
            ControlStart(character);
            
            // Init FSM
            InitStateMap();
            ChangeState(GetCurrentStateKey());
        }

        private void Update()
        {
            if (character == null) return;
            
            GetCurrentState().StateUpdate();
        }

        #endregion

        #region Methods:FSM
        
        public AiState GetCurrentStateKey() => _currentStateKey;
        
        private IState GetCurrentState()
        {
            IState state;
            if (!_stateMap.TryGetValue(GetCurrentStateKey(), out state))
            {
                throw new NullReferenceException($"There's no state named {_currentStateKey.ToString()}");
            }

            return state;
        }

        public void ChangeState(AiState newStateKey)
        {
            GetCurrentState().StateExit();

            _currentStateKey = newStateKey;
            
            GetCurrentState().StateEnter();
        }

        public void InitStateMap()
        {
            var states = GetComponents<StateBehaviorBase<AiState>>();
            foreach (var state in states)
            {
                _stateMap.Add(state.GetStateKey(), state);
            }
        }

        #endregion

        private void OnDrawGizmos()
        {
            if (character == null) return;
            
            var c = character as Character;

            Handles.Label(c.transform.position - (c.transform.localScale/2), GetCurrentStateKey().ToString());
        }
    }
}