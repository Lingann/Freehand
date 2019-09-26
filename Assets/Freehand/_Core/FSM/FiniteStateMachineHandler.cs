using UnityEngine;
using System.Collections;
namespace Freehand.Core.FiniteStateMachine
{
    public abstract class FiniteStateMachineHandler : MonoBehaviour
    {
        public RootState rootState
        {
            get
            {
                if (_rootState == null)
                {
                    _rootState = new RootState();
                    //_rootState.stateSystem.Play<DefaultState>();
                }

                return _rootState;
            }
        }

        private RootState _rootState;

        protected virtual void Awake()
        {
        }

        protected virtual void OnEnable()
        { }

        protected virtual void Start()
        { }

        protected virtual void OnDisable()
        { }

        protected virtual void OnDestroy()
        { }

        protected virtual void Update()
        {
            if (rootState.OnUpdate != null)
            {
                rootState.OnUpdate();
            }
         }

        protected virtual void FixedUpdate()
        {
            if (rootState.OnFixedUpdate != null)
            {
                rootState.OnFixedUpdate();
            }
        }

        protected virtual void LateUpdate()
        {
            if (rootState.OnLateUpdate != null)
            {
                rootState.OnLateUpdate();
            }
        }

        /// <summary>
        /// Returns true if the GameObject and the Component are active.
        /// </summary>
        public virtual bool IsActive()
        {
            return isActiveAndEnabled;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        { }

        protected virtual void Reset()
        { }
#endif
        /// <summary>
        /// This callback is called if an associated RectTransform has its dimensions changed. The call is also made to all child rect transforms, even if the child transform itself doesn't change - as it could have, depending on its anchoring.
        /// </summary>
        protected virtual void OnRectTransformDimensionsChange()
        { }

        protected virtual void OnBeforeTransformParentChanged()
        { }

        protected virtual void OnTransformParentChanged()
        { }

        protected virtual void OnDidApplyAnimationProperties()
        { }

        protected virtual void OnCanvasGroupChanged()
        { }

        /// <summary>
        /// Called when the state of the parent Canvas is changed.
        /// </summary>
        protected virtual void OnCanvasHierarchyChanged()
        { }

        /// <summary>
        /// Returns true if the native representation of the behaviour has been destroyed.
        /// </summary>
        /// <remarks>
        /// When a parent canvas is either enabled, disabled or a nested canvas's OverrideSorting is changed this function is called. You can for example use this to modify objects below a canvas that may depend on a parent canvas - for example, if a canvas is disabled you may want to halt some processing of a UI element.
        /// </remarks>
        public bool IsDestroyed()
        {
            // Workaround for Unity native side of the object
            // having been destroyed but accessing via interface
            // won't call the overloaded ==
            return this == null;
        }

        public T GetState<T>() where T : StateBehaviour
        {
           return rootState.stateSystem.GetState<T>();
        }

        public void AddState<T>() where T : StateBehaviour, new()
        {
            rootState.stateSystem.AddState<T>();
        }

        public void AddState<T>(string name) where T : StateBehaviour, new()
        {
            rootState.stateSystem.AddState<T>(name);
        }

        public void Play<T>() where T : StateBehaviour
        {
            rootState.stateSystem.Play<T>();
        }
    }
}