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
    void FixedUpdate()
    {
        // If the object goes below the ground, stop it.
        if (state.pos.y < -3.5f)
        {
            state.pos.y = -3.5f;
        }
        // Update gameobject using the state data
        transform.position = state.pos;
    }

    public static void AddMinConstraints(Vector2 node1, Vector2 node2, float minDist, float compensate1, float compensate2)
    {
        Vector2 delta = node2 - node1;
        float deltaLength = delta.magnitude;
        if (deltaLength > 0 && deltaLength < minDist)
        {
            float diff = (deltaLength - minDist) / deltaLength;
            node1 += delta * compensate1 * diff;
            node2 -= delta * compensate1 * diff;
        }
    }

    public static void AddMaxConstraints(Vector2 node1, Vector2 node2, float maxDist, float compensate1, float compensate2)
    {
        Vector2 delta = node2 - node1;
        float deltaLength = delta.magnitude;
        if (deltaLength > maxDist)
        {
            float diff = (deltaLength - maxDist) / deltaLength;
            node1 += delta * compensate1 * diff;
            node2 -= delta * compensate1 * diff;
        }
    }




}
