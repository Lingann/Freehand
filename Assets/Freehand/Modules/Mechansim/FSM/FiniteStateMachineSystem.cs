using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Freehand.Core.FiniteStateMachine
{
    public class FiniteStateMachineSystem
    {
        public BehaviourAction OnAwake;
        public BehaviourAction OnUpdate;
        public BehaviourAction OnFixedUpdate;
        public BehaviourAction OnLateUpdate;

        Dictionary<string, StateBehaviour> _states;

        private StateBehaviour m_root;

        private StateBehaviour _currentState;

        private StateBehaviour _defaultState;

        public StateBehaviour currentState
        {
            get
            {
                return _currentState;
            }
        }

        public FiniteStateMachineSystem(StateBehaviour state = null)
        {
            if(state == null)
                m_root = new RootState();
            else
                m_root = state;

            _states = new Dictionary<string, StateBehaviour>();

            m_root.OnUpdate += Update;

            m_root.OnLateUpdate += LateUpdate;

            m_root.OnFixedUpdate += FixedUpdate;

            _defaultState = AddState<DefaultState>();

            _currentState = _defaultState;
        }

        public T GetState<T>() where T : StateBehaviour
        {
            System.Type type = typeof(T);

            foreach (var temp in _states)
            {
                if (temp.Value.GetType() == type)
                {
                    return temp.Value as T;
                }
            }
            return null;
        }

        public T AddState<T>() where T : StateBehaviour, new()
        {
            string name = typeof(T).ToString();

            if (_states.ContainsKey(name))
            {
                throw new System.Exception("The Finite State Machine already conatins the state " + name);
            }

            T state = new T();

            state.name = name;

            state.parent = m_root;

            AddState(state);

            return state;
        }

        public T AddState<T>(string name) where T : StateBehaviour, new()
        {
            if (_states.ContainsKey(name))
            {
                throw new System.Exception("The Finite State Machine already conatins the state " + name);
            }

            T state = new T();

            state.name = name;

            state.parent = m_root;

            AddState(state);

            return state;
        }

        public void Play<T>() where T : StateBehaviour
        {
            StateBehaviour state = GetState<T>();

            if (state == null)
            {
                throw new System.Exception("Failed to find State of " + typeof(T).ToString() + "empty");
            }

            SetState(state);
        }

        public void ResetState()
        {
            _currentState = _defaultState;
        }

        private void AddState(StateBehaviour state)
        {
            _states.Add(state.name, state);

            if(state.OnAwake!=null)
                state.OnAwake();
        }

        private void SetState(StateBehaviour state)
        {
            if (_currentState == state) return;

            if(_currentState != null)
                _currentState.OnExit();

            if (_currentState.isParentSate)
                _currentState.stateSystem.ResetState();

                _currentState = state;

            if (_currentState.times == 0)
                _currentState.OnStart();

            _currentState.OnEnter();
        }

        private void Update()
        {
            if (OnUpdate != null)
            {
                OnUpdate();
            }

            _currentState.OnUpdate();
        }

        private void LateUpdate()
        {
            if (OnLateUpdate != null)
            {
                OnLateUpdate();
            }

            _currentState.OnLateUpdate();
        }

        private void FixedUpdate()
        {
            if (OnFixedUpdate != null)
            {
                OnFixedUpdate();
            }

            _currentState.OnFixedUpdate();

        }


    }
}
