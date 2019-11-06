using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Freehand.Core.FiniteStateMachine
{
    public class ChildState2 : StateBehaviour
    {
        protected override void Awake()
        {
            Debug.Log("Awake: " + name);
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

            if (Input.GetKeyDown(KeyCode.F1))
            {
                parent.stateSystem.Play<ChildState1>();
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
