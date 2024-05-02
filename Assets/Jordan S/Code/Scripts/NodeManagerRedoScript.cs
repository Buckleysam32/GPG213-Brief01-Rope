using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NodeManagerRedoScript : MonoBehaviour
{
    public float managerDist = 1.5f;
    public int verticalRopeSegments, horizontalRopeSegments;
    public GameObject nodePrefab;
    public List<NodeRedoScript> allNodes;
    public List<ConstraintRedoScript> allConstraints;
    public bool useFixedDistance, singleRope;
    
    //Setup the nodes, then setup the constraints for those nodes
    void Start()
    {
        SetupNodes(singleRope);
    }
    private void FixedUpdate()
    {
        SimulateNodes();
    }
    public void SetupNodes(bool singleRope)
    {
        allNodes = new List<NodeRedoScript>();
        Vector3 newSpawnPos = transform.position;
        if (singleRope)
        {
            //Generate a rope starting at this gameobjects position, going down by managerdist for every node 
            for (int i = 0; i < verticalRopeSegments; i++)
            {
                newSpawnPos.y -= managerDist;
                GameObject newNodeGO = Instantiate(nodePrefab, newSpawnPos, Quaternion.identity);
                NodeRedoScript newNode = newNodeGO.GetComponent<NodeRedoScript>();
            
                //If this is the first node, we need it to be a fixed point that doesn't move based on other nodes
                if (i == 0)
                {
                    newNode.nodePos = newNode.transform.position;
                    newNode.nodePrevPos = newNode.nodePos;
                    newNode.isLocked = true;
                }
                //Else it is a dynamic node, set its fixed bool and other variables.
                else
                {
                    newNode.nodePos = newNode.transform.position;
                    newNode.nodePrevPos = newNode.nodePos;
                    newNode.mass = 1;
                    newNode.isLocked = false;
                }
                allNodes.Add(newNode);
            }
        }
        else
        {
            float originalY = newSpawnPos.y;
            //Similar to a normal rope, but also go across using a second loop.
            for (int i = 0; i < horizontalRopeSegments; i++)
            {                    
                newSpawnPos.x += (managerDist * 1.2f);
                newSpawnPos.y = originalY;
                for (int j = 0; j < verticalRopeSegments; j++)
                {
                    newSpawnPos.y -= managerDist;
                    GameObject newNodeGO = Instantiate(nodePrefab, newSpawnPos, Quaternion.identity);
                    NodeRedoScript newNode = newNodeGO.GetComponent<NodeRedoScript>();

                    //If this is the first node, we need it to be a fixed point that doesn't move based on other nodes
                    if (i % 3 == 0 && j == 0)
                    {
                        newNode.nodePos = newNode.transform.position;
                        newNode.nodePrevPos = newNode.nodePos;
                        newNode.isLocked = true;
                    }
                    //Else it is a dynamic node, set its fixed bool and other variables.
                    else
                    {
                        newNode.nodePos = newNode.transform.position;
                        newNode.nodePrevPos = newNode.nodePos;
                        newNode.mass = 1;
                        newNode.isLocked = false;
                    }
                    allNodes.Add(newNode);
                }
            }
        }
        //Now that the nodes are set up, lets get the constraints
        for (int i = 1; i < allNodes.Count; i++)
        {
            //for each of the nodes we spawn, create a new constraint
            ConstraintRedoScript newConstraintA = gameObject.AddComponent<ConstraintRedoScript>();            
            if (i % verticalRopeSegments == 0)
            {
                //do nothing so the first node in a column doesn't connect to the last of the previous column
            }
            else
            {
                //tell it its two nodes
                newConstraintA.nodeA = allNodes[i - 1];
                newConstraintA.nodeB = allNodes[i];
                //tell it its min and max distances
                newConstraintA.minJointDist = managerDist - (managerDist / 2f);
                newConstraintA.maxJointDist = managerDist + (managerDist / 2f);
                //add it to the constraints list
                allConstraints.Add(newConstraintA);
                allNodes[i].constraintA = newConstraintA;
            }
        }
        //Setup a second constraint that targets the node vertical rope segments back in the list, this connects to the
        //node horizontally to the left. Only do this if we are making a 2D cloth not a single rope.
        if (!singleRope)
        {
            for (int i = 0; i < allNodes.Count; i++)
            {
                //create a new constraint
                ConstraintRedoScript newConstraintB = gameObject.AddComponent<ConstraintRedoScript>();
                //If this node isnt the first node, but is still on the top row, setup its horizontal constraint
                if (i != 0 && i % verticalRopeSegments == 0)
                {
                    newConstraintB.nodeB = allNodes[i - verticalRopeSegments];
                    newConstraintB.nodeA = allNodes[i];
                    //tell it its min and max distances
                    newConstraintB.minJointDist = managerDist - (managerDist / 2f);
                    newConstraintB.maxJointDist = managerDist + (managerDist / 2f);
                    //add it to the constraints list
                    allConstraints.Add(newConstraintB);                
                    allNodes[i].constraintB = newConstraintB;
                }
                //else if it is a node that isn't in the first column, go back the number of vertical nodes (to get to the
                //same position but in the last column) and make that our second constraint
                else if (allNodes.ElementAtOrDefault(i - verticalRopeSegments))
                {
                    newConstraintB.nodeB = allNodes[i - verticalRopeSegments];
                    newConstraintB.nodeA = allNodes[i];
                    //tell it its min and max distances
                    newConstraintB.minJointDist = managerDist - (managerDist / 2f);
                    newConstraintB.maxJointDist = managerDist + (managerDist / 2f);
                    //add it to the constraints list
                    allConstraints.Add(newConstraintB);                
                    allNodes[i].constraintB = newConstraintB;
                }
            }
        }
    }
    /// <summary>
    /// During physics update, integrate the nodes movement, then update their positions using each constraints
    /// update function.
    /// </summary>
    public void SimulateNodes()
    {
        //For each node we have already setup in Start
        foreach (NodeRedoScript node in allNodes)
        {
            if (!node.isLocked)
            {
                //Track its current position as the basis of where we're starting
                Vector2 updatePosition = node.nodePos;
                //Add the difference between our last position to our current position to track our movement
                node.nodePos += node.nodePos - node.nodePrevPos;
                //Add gravity to the resulting movement
                node.nodePos += Vector2.down * (9.81f * node.mass * Time.deltaTime * Time.deltaTime);
                //Update the nodes prevpos value with the original position before we added the movement to it.
                node.nodePrevPos = updatePosition;
            }
        }
        //Now that we've updated the node values, we can call our constraints functions to bring them back in line
        for (int i = 0; i < 10; i++)
        {
            //For each constraint
            foreach (ConstraintRedoScript constraint in allConstraints)
            {
                //call that specific constraints distance update function depending on the whether we're using
                //fixed updates or variable updates. 
                if (useFixedDistance)
                {
                    constraint.FixedDistanceUpdate();
                }
                else
                {
                    constraint.VariableDistanceUpdate();
                }
            }
        }
        //Now that the constraints have updated all the nodes positions based on where they can be, update the 
        //nodes transform position to that value.
        foreach (NodeRedoScript node in allNodes)
        {
            node.transform.position = node.nodePos;
        }
    }
}
