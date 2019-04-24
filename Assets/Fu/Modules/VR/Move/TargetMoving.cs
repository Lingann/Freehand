using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMoving : MonoBehaviour {

    public Transform freeArea;

    public Transform head;

    public void AdjustPosition(Vector3 targetPos)
    {
        Quaternion desiredRotation = Quaternion.LookRotation(head.forward, head.up);

        Quaternion rotationCorrection = desiredRotation * Quaternion.Inverse(head.localRotation);

        freeArea.rotation = rotationCorrection;

        Vector3 positionCorrection = targetPos - head.position;

        freeArea.position = freeArea.position + positionCorrection;
    }

    public void AdjustRotation(float yEnlerAngles)
    {
        transform.rotation = Quaternion.Euler(0, yEnlerAngles - head.localEulerAngles.y, 0);
    }

    public void AdjustTransfrom(Transform target)
    {
        AdjustPosition(target.position);

        AdjustRotation(target.eulerAngles.y);

        AdjustPosition(target.position);

        AdjustRotation(target.eulerAngles.y);
    }

    void ReSetPosition()
    {
        AdjustPosition(Vector3.zero);
    }
}
