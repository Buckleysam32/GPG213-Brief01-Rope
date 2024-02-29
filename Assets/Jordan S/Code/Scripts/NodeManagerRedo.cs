using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NodeManagerRedo : MonoBehaviour
{
    public float managerDist = 1.5f;
    public int verticalRopeSegments, horizontalRopeSegments;
    public GameObject nodePrefab;
    public List<NodeJordanRedoScript> allNodes;
    public List<ConstraintJordanScript> allConstraints;
    public bool useFixedDistance, singleRope;
    public LineRenderer ManagerLineRenderer;
    void Start()
    {
        SetupNodesInLine(singleRope);
    }

    private void FixedUpdate()
    {
        SimulateNodes();
    }

    public void SetupNodesInLine(bool singleRope)
    {
        allNodes = new List<NodeJordanRedoScript>();
        Vector3 newSpawnPos = transform.position;
        if (singleRope)
        {
            //Generate a rope starting at fixed point, with distance d, 
            for (int i = 0; i < verticalRopeSegments; i++)
            {
                newSpawnPos.y -= managerDist;
                Debug.Log(newSpawnPos);
                GameObject newNodeGO = Instantiate(nodePrefab, newSpawnPos, Quaternion.identity);
                NodeJordanRedoScript newNode = newNodeGO.GetComponent<NodeJordanRedoScript>();
            
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
            //Generate a sheet starting at fixed point, with grid distance d
            for (int i = 0; i < horizontalRopeSegments; i++)
            {                    
                newSpawnPos.x += managerDist;
                newSpawnPos.y = originalY;
                for (int j = 0; j < verticalRopeSegments; j++)
                {
                    newSpawnPos.y -= managerDist;
                    GameObject newNodeGO = Instantiate(nodePrefab, newSpawnPos, Quaternion.identity);
                    NodeJordanRedoScript newNode = newNodeGO.GetComponent<NodeJordanRedoScript>();

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
            ConstraintJordanScript newConstraint = gameObject.AddComponent<ConstraintJordanScript>();
            if (i % verticalRopeSegments == 0)
            {
                //do nothing so the last node in a collumn doesn't connect to the first of the next
            }
            else if (i != 1 && verticalRopeSegments % i == 0)
            {
                newConstraint.nodeA = allNodes[i - verticalRopeSegments];
                newConstraint.nodeB = allNodes[i]; 
                //tell it its min and max distances
                newConstraint.minJointDist = managerDist - (managerDist / 2f);
                newConstraint.maxJointDist = managerDist + (managerDist / 2f);
                //add it to the constraints list
                allConstraints.Add(newConstraint);
            }
            else
            {
                //tell it its two nodes
                newConstraint.nodeA = allNodes[i - 1];
                newConstraint.nodeB = allNodes[i];
                //tell it its min and max distances
                newConstraint.minJointDist = managerDist - (managerDist / 2f);
                newConstraint.maxJointDist = managerDist + (managerDist / 2f);
                //add it to the constraints list
                allConstraints.Add(newConstraint);
            }
        }
    }
    public void SimulateNodes()
    {
        foreach (NodeJordanRedoScript node in allNodes)
        {
            if (!node.isLocked)
            {
                Vector2 updatePosition = node.nodePos;
                node.nodePos += node.nodePos - node.nodePrevPos;
                node.nodePos += Vector2.down * (9.81f * node.mass * Time.deltaTime * Time.deltaTime);
                node.nodePrevPos = updatePosition;
            }
        }
        for (int i = 0; i < 25; i++)
        {
            foreach (ConstraintJordanScript constraint in allConstraints)
            {
                if (useFixedDistance)
                {
                    constraint.FixedDistanceUpdate(0.5f, 0.5f);
                }
                else
                {
                    constraint.VariableDistanceUpdate();
                }
            }
        }
        foreach (NodeJordanRedoScript node in allNodes)
        {
            node.transform.position = node.nodePos;
        }

    }
}
