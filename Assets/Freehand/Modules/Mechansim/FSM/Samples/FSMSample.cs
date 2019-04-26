using UnityEngine;
using System.Collections;
namespace Freehand.Core.FiniteStateMachine
{
    public class FSMSample : FiniteStateMachineHandler
    {
        protected override void Awake()
        {
            AddState<State1>();
            AddState<State2>();
        }

        protected override void Start()
        {
            Play<State1>();
        }
    }

}
