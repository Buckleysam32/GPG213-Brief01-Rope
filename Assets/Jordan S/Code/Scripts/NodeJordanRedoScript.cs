using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeJordanRedoScript : MonoBehaviour
{
    public Vector2 nodePos, nodePrevPos;
    public bool isLocked;
    public float mass;
    private void OnMouseDrag()
    {
        Vector3 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempPos.z = 0f;
        nodePos = tempPos;
    }

    private void OnMouseUp()
    {
        transform.position = nodePos;
    }
}