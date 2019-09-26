using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void LensAction(Camera camera);

public class LensViewTrigger : MonoBehaviour {

    private Camera _camera;

    private Renderer _renderer;

    private bool _isRenderer;

    private bool _isVisible;

    private bool _isPrevVisiable;

    private void Start()
    {
        _renderer = transform.GetComponentInChildren<Renderer>();

        if (_camera == null)

            _camera = Camera.main;
    }
   
    private bool IsInView()
    {

        Vector3 pointOnScreen = _camera.WorldToScreenPoint(_renderer.bounds.center);

        // 判断是否在镜头前
        if (pointOnScreen.z < 0)
        {
            Debug.Log("Behind: " + transform.name);
            return false;
        }

        // 判断是否在视角内
        if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) || (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
        {
            Debug.Log("OutOfBounds: " + transform.name);
            return false;
        }

        // 判断是否有遮挡物
        RaycastHit hit;

        Vector3 heading = transform.position - _camera.transform.position;

        Vector3 direction = heading.normalized;// / heading.magnitude;

        if (Physics.Linecast(_camera.transform.position, _renderer.bounds.center, out hit))
        {
            if (hit.transform.name != transform .name)
            {
#if UNITY_EDITOR
 //               Debug.DrawLine(_camera.transform.position, _renderer.bounds.center, Color.red);

 //               Debug.Log(transform.name + " occluded by " + hit.transform.name);

 //               Debug.Log(transform .name + " occluded by " + hit.transform.name);
#endif
                return false;
            }
        }
        return true;
    }

    void OnBecameVisible()
    {
        Debug.Log("物体可见");
    }

    private void OnBecameInvisible()
    {
        Debug.Log("物体不可见");
    }

    void Update()
    {
        if (_renderer.isVisible)
        {
            if (IsInView() && !_isVisible)
            {
                Debug.Log("进入视角");

                ILensViewTrigger trigger = GetComponent<ILensViewTrigger>();

                if (trigger != null)
                {
                    trigger.OnVisiableEnter(_camera);
                }

                _isVisible = true;
            }
            else if(!IsInView() && _isVisible)
            {
                Debug.Log("退出视角");

                ILensViewTrigger trigger = GetComponent<ILensViewTrigger>();

                if (trigger != null)
                {
                    trigger.OnVisiableOut(_camera);
                }

                _isVisible = false;
            }
        }
        else
        {
            if (!IsInView() && _isVisible)
            {
                Debug.Log("退出视角2");

                ILensViewTrigger trigger = GetComponent<ILensViewTrigger>();

                if (trigger != null)
                {
                    trigger.OnVisiableOut(_camera);
                }

                _isVisible = false;
            }
        }
       
    }
}
