using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Freehand.Core.FiniteStateMachine
{
    public class ChildState1 : StateBehaviour
    {
        protected internal override void Awake()
        {
            Debug.Log("Awake: " + name);
        }

        protected internal override void Start()
        {
            Debug.Log("Start: " + name);
        }

        protected internal override void Enter()
        {
            Debug.Log("Enter: " + name);
        }

        protected internal override void Exit()
        {
            Debug.Log("Exit: " + name);
        }

        protected internal override void Update()
        {
            Debug.Log("Update" + name);

            if (Input.GetKeyDown(KeyCode.F2))
            {
                parent.stateSystem.Play<ChildState2>();
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                parent.parent.stateSystem.Play<State1>();
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                parent.parent.stateSystem.Play<State2>();
            }
        }
    }
}
