using UnityEngine;
using System.Collections;


public interface ILensViewTrigger
{
    void OnVisiableEnter(Camera camera);

    void OnVisiableOut(Camera camera);

    //void OnVisableStay(Camera camera);
}
