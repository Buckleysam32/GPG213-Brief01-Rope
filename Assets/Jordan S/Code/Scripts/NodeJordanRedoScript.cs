using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeJordanRedoScript : MonoBehaviour
{
    public Vector2 nodePos, nodePrevPos;
    public bool isLocked;
    public float mass;
    public ConstraintJordanScript constraintA, constraintB;
    public LineRenderer nodeLinerenderer;

    private void Update()
    {
        nodeLinerenderer.SetPosition(0, transform.position);
        if (constraintA)
        {
            nodeLinerenderer.SetPosition(1, constraintA.nodeA.transform.position);
        }
        else
        {
            nodeLinerenderer.SetPosition(1, transform.position);
        }
        nodeLinerenderer.SetPosition(2, transform.position);
        if (constraintB)
        {
            nodeLinerenderer.SetPosition(3, constraintB.nodeB.transform.position);
        }
        else
        {
            nodeLinerenderer.SetPosition(3, transform.position);
        }
    }

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