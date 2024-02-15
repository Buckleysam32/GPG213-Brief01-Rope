using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NodeManagerRedo : MonoBehaviour
{
    public float managerDist = 1.5f;
    public int ropeSegments;
    public GameObject nodePrefab;
    public List<NodeJordanRedoScript> allNodes;
    public List<ConstraintJordanScript> allConstraints;
    public bool useFixedDistance;
    public LineRenderer ManagerLineRenderer;
    void Start()
    {
        SetupNodesInLine();
    }

    private void FixedUpdate()
    {
        SimulateNodes();
    }

    public void SetupNodesInLine()
    {
        allNodes = new List<NodeJordanRedoScript>();
        Vector3 newSpawnPos = transform.position;
        newSpawnPos.y += managerDist;
        //Generate a rope starting at fixed point, with distance d, 
        for (int i = 0; i < ropeSegments; i++)
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
                newNode.mass = i;
                newNode.isLocked = false;
            }
            allNodes.Add(newNode);
        }

        for (int i = 1; i < allNodes.Count; i++)
        {
            ConstraintJordanScript newConstraint = gameObject.AddComponent<ConstraintJordanScript>();
            newConstraint.nodeA = allNodes[i - 1];
            newConstraint.nodeB = allNodes[i];
            newConstraint.minJointDist = managerDist - (managerDist / 2f);
            newConstraint.maxJointDist = managerDist + (managerDist/2f);
            allConstraints.Add(newConstraint);
        }
        for (int i = 0; i < allNodes.Count; i++)
        {
            ManagerLineRenderer.SetPosition(i, allNodes[i].transform.position);
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
                node.nodePos += Vector2.down * 9.81f * node.mass * Time.deltaTime * Time.deltaTime;
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

        for (int i = 0; i < ManagerLineRenderer.positionCount; i++)
        {
            ManagerLineRenderer.SetPosition(i, allNodes[i].transform.position);
        }
    }
}
