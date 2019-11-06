using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Freehand.Core.FiniteStateMachine
{
    public class State1 : StateBehaviour
    {
        protected override void Awake()
        {
            stateSystem.AddState<ChildState1>();
            stateSystem.AddState<ChildState2>();
        }

        protected override void Start()
        {
            Debug.Log("Start: " + name);
        }

        protected override void Enter()
        {
            Debug.Log("Enter: " + name);
        }

        protected override void Exit()
        {
            Debug.Log("Exit: " + name);
        }

        protected override void Update()
        {
            Debug.Log("Update" + name);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                stateSystem.Play<ChildState1>();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                stateSystem.Play<ChildState2>();
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                parent.stateSystem.Play<State2>();
            }
        }
    }
}
