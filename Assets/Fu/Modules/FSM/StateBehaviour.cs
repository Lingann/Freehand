using UnityEngine;
using System.Collections;
namespace Lingann.Lab.FiniteStateMachine
{
    public delegate void BehaviourAction();

    public abstract class StateBehaviour
    {
        #region Properties
        public StateBehaviour parent { get { return _parent; } set { _parent = value; } }

        public string name { get { return _name; }  set { _name = value ; } }

        public int times { get { return _times; } }

        public bool isParentSate
        {
            get { return _isParentState; }
        }

        public bool isRunning
        {
            get {
                if (parent.stateSystem.currentState == this)
                    return true;
                else
                    return false;
            }
        }

        private MonoBehaviour _holder;

        private bool _isParentState = false;

        public FiniteStateMachineSystem stateSystem
        {
            get
            {
                if (_stateSystem == null)
                {
                    _stateSystem = new FiniteStateMachineSystem(this);
                    _isParentState = true;
                }
                return _stateSystem;
            }
        }

        #endregion

        private StateBehaviour _parent;
        private string _name;
        private int _times;
        private FiniteStateMachineSystem _stateSystem;

        public BehaviourAction OnAwake;
        public BehaviourAction OnStart;
        public BehaviourAction OnEnter;
        public BehaviourAction OnExit;
        public BehaviourAction OnUpdate;
        public BehaviourAction OnLateUpdate;
        public BehaviourAction OnFixedUpdate;

        public StateBehaviour()
        {
            OnAwake+= Awake;
            OnStart += Start;
            OnEnter += Enter;
            OnEnter += AddTimes;
            OnUpdate += Update;
            OnFixedUpdate += FixedUpdate;
            OnLateUpdate += LateUpdate;
            OnExit += Exit;
        }

        /// <summary>
        /// 当状态被添加到状态机时回调
        /// </summary>
        protected internal virtual void Awake() { }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void Start() { }

        /// <summary>
        /// 进入状态回调
        /// </summary>
        protected internal virtual void Enter() { }

        /// <summary>
        /// 退出状态时回调
        /// </summary>
        protected internal virtual void Exit() { }

        /// <summary>
        /// 每帧执行
        /// </summary>
        protected internal virtual void Update() { }

        /// <summary>
        ///  每帧执行
        /// </summary>
        protected internal virtual void FixedUpdate() { }

        /// <summary>
        ///  每帧执行
        /// </summary>
        protected internal virtual void LateUpdate() { }

        private void AddTimes()
        {
            _times ++;
        }

    }
}
