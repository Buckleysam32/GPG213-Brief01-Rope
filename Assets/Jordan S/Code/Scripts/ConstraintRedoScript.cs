using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintRedoScript : MonoBehaviour
{
    public NodeRedoScript nodeA, nodeB;
    public float maxJointDist, minJointDist;

    /// <summary>
    /// A function that is used to constrain nodes at a fixed distance to each other.
    /// </summary>
    public void FixedDistanceUpdate()
    {
        //Figure out the centre point of the two nodes
        Vector2 constraintCentre = (nodeA.nodePos + nodeB.nodePos) / 2f;
        //Get the direction of the two nodes positions.
        Vector2 constraintDirection = (nodeA.nodePos - nodeB.nodePos).normalized;
        if (!nodeA.isLocked)
        {
            //If node a isn't locked, go to the centre point, add the direction, move to the end of where
            //the node should be, and update the nodes position. 
            nodeA.nodePos = constraintCentre + constraintDirection * ((maxJointDist + minJointDist) / 2f) / 2f;
            nodeA.transform.position = nodeA.nodePos;
        }
        if (!nodeB.isLocked)
        {
            //If node a isn't locked, go to the centre point, add the direction, move to the end of where
            //the node should be, and update the nodes position. 
            nodeB.nodePos = constraintCentre - constraintDirection * ((maxJointDist + minJointDist) / 2f) / 2f;
            nodeB.transform.position = nodeB.nodePos;
        }
    }
    /// <summary>
    /// A function that is used to constrain nodes at a variable distance. So long as the nodes are between a minimum
    /// and maximum distance it doesn't affect them. If they are over the maximum, it brings them closer, if it's under
    /// then it pushes the nodes apart.
    /// </summary>
    public void VariableDistanceUpdate()
    {
        //If nodeA is locked, we can only move node b
        if (nodeA.isLocked)
        {
            MaxDistanceUpdate(0f, 1f);
        }
        //Same for node b if node a isn't locked
        else if(nodeB.isLocked)
        {
            MaxDistanceUpdate(1f, 0f);
        }
        //if neither is locked, move both equally.
        else
        {
            MaxDistanceUpdate(0.5f, 0.5f); 
        }
    }
    /// <summary>
    /// A function to push two nodes apart if they are too close.
    /// </summary>
    /// <param name="compensate1"></param>
    /// <param name="compensate2"></param>
    public void MinDistanceUpdate(float compensate1, float compensate2)
    {
        //calculate the difference in position
        Vector2 delta = nodeB.nodePos - nodeA.nodePos;
        //get the magnitude so we can compare it to another float.
        float deltaLength = delta.magnitude;
        //If that magnitude is smaller than the minimum distance, move the nodes apart.
        if (deltaLength > 0 && deltaLength < minJointDist)
        {
            //Get the difference.
            float diff = (deltaLength - minJointDist) / deltaLength;
            //Move each node apart based on how much to affect each node by their relevant compensation rate.
            nodeA.nodePos += delta * (compensate1 * diff);
            nodeB.nodePos -= delta * (compensate2 * diff);
        }
    }
    /// <summary>
    /// A function to bring two nodes closer together if they are too far apart.
    /// </summary>
    /// <param name="compensate1"></param>
    /// <param name="compensate2"></param>
    public void MaxDistanceUpdate(float compensate1, float compensate2)
    {        
        //Calculate the difference in position
        Vector2 delta = nodeB.nodePos - nodeA.nodePos ;        
        //Get the magnitude so we can compare it to another float.
        float deltaLength = delta.magnitude;        
        //If that magnitude is larger than the minimum distance, move the nodes apart.
        if (deltaLength > maxJointDist)
        {            
            //Get the difference.
            float diff = (deltaLength - maxJointDist) / deltaLength;            
            //Move each node apart based on how much to affect each node by their relevant compensation rate.
            nodeA.nodePos += delta * (compensate1 * diff);
            nodeB.nodePos -= delta * (compensate2 * diff);
        }
    }
}
