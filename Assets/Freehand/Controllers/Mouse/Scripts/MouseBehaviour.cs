using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviour : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("按下鼠标左键， Fire1");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("按下鼠标右键， Fire2");
        }

        if (Input.GetButtonDown("Fire3"))
        {
            Debug.Log("按下鼠标中键， Fire3");
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("按下鼠标左键,GetMouseButtonDown(0)");
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("按下鼠标右键键,GetMouseButtonDown(1)");
        }

        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("按下鼠标中键,GetMouseButtonDown(2)");
        }
    }


    private void OnGUI()
    {
        // 游戏窗口的左下角位于（0，0）。的屏幕的窗口的右上方是(Screen.width,Screen.height)
        GUILayout.TextField("鼠标屏幕空间位置：  " + Input.mousePosition.ToString());
        GUILayout.TextField("鼠标世界空间位置：  "+ GetWorldMousePosition().ToString());
    }


    /// <summary>
    /// 转变position从屏幕空间到世界空间
    /// </summary>
    /// <returns>返回世界空间鼠标位置坐标</returns>
    private Vector3 GetWorldMousePosition()
    {
        Camera camera = Camera.main;
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();
        
        // 从Event获取鼠标位置（请注意，Event的y位置是倒置的）
        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = camera.pixelHeight - currentEvent.mousePosition.y;

        var point = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.nearClipPlane));
        
        return point;
    }
}
