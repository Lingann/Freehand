using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovingObject : MonoBehaviour {

    [SerializeField] private Transform target;

   [SerializeField] private TargetMoving targetMoving;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            targetMoving.AdjustTransfrom(target);
        }
	}
}
