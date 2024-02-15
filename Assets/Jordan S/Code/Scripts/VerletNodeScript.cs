using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class VerletNodeScript : MonoBehaviour
{
    public VerletState state = new VerletState();
    public VerletNodeScript prevNode;
    public float desiredDist;
    public float compensate1, compensate2;
    public float mass;
    public bool isFixed;
    

    private void OnMouseDrag()
    {
        Vector3 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempPos.z = 0f;
        state.pos = tempPos;
        
    }

    private void OnMouseUp()
    {
        transform.position = state.pos;
    }

    public void FixedConstraints(Vector2 node1, Vector2 node2, float minDist, float compensate1, float compensate2)
    {
        Vector2 delta = node1 - node2;
        float deltaLength = delta.magnitude;
        if (deltaLength > 0)
        {
            float diff = (desiredDist - deltaLength) / deltaLength;
            node1 += delta * compensate1 * diff;
            node2 -= delta * compensate1 * diff;
        }
    }
    public void AddMinConstraints(Vector2 node1, Vector2 node2, float minDist, float compensate1, float compensate2)
    {
        Vector2 delta = node2 - node1;
        float deltaLength = delta.magnitude;
        if (deltaLength > 0 && deltaLength < minDist)
        {
            float diff = (deltaLength - minDist) / deltaLength;
            node1 += delta * (compensate1 * diff);
            node2 -= delta * (compensate2 * diff);
        }
    }
    public void AddMaxConstraints(Vector2 node1, Vector2 node2, float maxDist, float compensate1, float compensate2)
    {
        Vector2 delta = node2 - node1;
        float deltaLength = delta.magnitude;
        if (deltaLength > maxDist)
        {
            float diff = (deltaLength - maxDist) / deltaLength;
            node1 += delta * (compensate1 * diff);
            node2 -= delta * (compensate2 * diff);
        }
    }
}
