using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintJordanScript
{
    public NodeJordanRedoScript nodeA, nodeB;
    public float maxJointDist, minJointDist;
    

    public void FixedDistanceUpdate(float compensate1, float compensate2)
    {
        Vector2 constraintCentre = (nodeA.nodePos + nodeB.nodePos) / 2f;
        Vector2 constraintDirection = (nodeA.nodePos - nodeB.nodePos).normalized;
        if (!nodeA.isLocked)
        {
            nodeA.nodePos = constraintCentre + constraintDirection * ((maxJointDist + minJointDist) / 2f) / 2f;
            nodeA.transform.position = nodeA.nodePos;
        }
        if (!nodeB.isLocked)
        {
            nodeB.nodePos = constraintCentre - constraintDirection * ((maxJointDist + minJointDist) / 2f) / 2f;
            nodeB.transform.position = nodeB.nodePos;
        }
    }

    public void VariableDistanceUpdate()
    {
        if (nodeA.isLocked)
        {
            MaxDistanceUpdate(0f, 1f);
        }
        else if(nodeB.isLocked)
        {
            MaxDistanceUpdate(1f, 0f);
        }
        else
        {
            MaxDistanceUpdate(0.5f, 0.5f); 
        }
    }

    public void MinDistanceUpdate(float compensate1, float compensate2)
    {
        Vector2 delta = nodeB.nodePos - nodeA.nodePos ;
        float deltaLength = delta.magnitude;
        if (deltaLength > 0 && deltaLength < minJointDist)
        {
            float diff = (deltaLength - minJointDist) / deltaLength;
            nodeA.nodePos += delta * (compensate1 * diff);
            nodeB.nodePos -= delta * (compensate2 * diff);
        }
    }
    public void MaxDistanceUpdate(float compensate1, float compensate2)
    {
        Vector2 delta = nodeB.nodePos - nodeA.nodePos ;
        float deltaLength = delta.magnitude;
        if (deltaLength > maxJointDist)
        {
            float diff = (deltaLength - maxJointDist) / deltaLength;
            nodeA.nodePos += delta * (compensate1 * diff);
            nodeB.nodePos -= delta * (compensate2 * diff);
        }
    }
}
