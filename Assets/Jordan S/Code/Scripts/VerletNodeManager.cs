using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class VerletNodeManager : MonoBehaviour
{
    public float managerDist = 1.5f;
    public int ropeSegments;
    public GameObject nodePrefab;

    public List<VerletNodeScript> allNodes;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        for (int i = 1; i < allNodes.Count; i++)
        {
            VerletNodeScript nodeToUpdate = allNodes[i];
            if (!nodeToUpdate.isFixed)
            {
                nodeToUpdate.state.addForce(new Vector2(0f, -9.81f));
                nodeToUpdate.state.integrate();
                if (nodeToUpdate.prevNode && !nodeToUpdate.prevNode.isFixed)
                {
                    for (int k = 0; k < 20; k++)
                    {
                        nodeToUpdate.FixedConstraints(nodeToUpdate.state.pos, nodeToUpdate.prevNode.state.pos, nodeToUpdate.desiredDist, nodeToUpdate.compensate1, nodeToUpdate.compensate2);
                    }
                }

                if (nodeToUpdate.state.pos.y < -4f)
                {
                    nodeToUpdate.state.pos.y = -4f;
                }

                nodeToUpdate.transform.position = nodeToUpdate.state.pos;
            }
        }
    }
    void Start()
    {
        allNodes = new List<VerletNodeScript>();
        Vector3 newSpawnPos = transform.position;
        newSpawnPos.y += managerDist;
        //Generate a rope starting at fixed point, with distance d, 
        for (int i = 0; i < ropeSegments; i++)
        {
            newSpawnPos.y -= managerDist;
            Debug.Log(newSpawnPos);
            GameObject newNodeGO = Instantiate(nodePrefab, newSpawnPos, Quaternion.identity);
            VerletNodeScript newNode = newNodeGO.GetComponent<VerletNodeScript>();
            
            //If this is the first node, we need it to be a fixed point that doesn't move based on other nodes
            if (i == 0)
            {
                newNode.isFixed = true;
                newNode.prevNode = null;
                newNode.desiredDist = managerDist;
                newNode.mass = 1f;
                newNode.state.pos = newNode.transform.position;
            }
            //Else it is a dynamic node, set its fixed bool and other variables.
            else
            {
                newNode.isFixed = false;                
                newNode.prevNode = allNodes[i - 1].GetComponent<VerletNodeScript>();
                newNode.desiredDist = managerDist;
                newNode.mass = 1f;
                newNode.state.pos = newNode.transform.position;
                Debug.Log(newNode.state.pos);
                //Check the previous node, if it was our starting node, we know it can't move so set compensate for us to 0
                if (newNode.prevNode.isFixed)
                {
                    newNode.compensate2 = 0f;
                    newNode.compensate1 = 1f;
                }
                //Else make each compensate equal
                else
                {
                    newNode.compensate1 = 0.5f;
                    newNode.compensate2 = 0.5f;
                }
            }
            allNodes.Add(newNode);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            VerletNodeScript lastNode = allNodes[allNodes.Count - 1];
            lastNode.state.addForce(new Vector2(0f, 100f));
        }
    }

    /*void FixedUpdate()
    {
        for (int i = 1; i < allNodes.Count; i++)
        {
            VerletNodeScript node = allNodes[i];
            node.state.addForce(new Vector2(0f, -9.81f));
            node.state.integrate();

            for (int j = 0; j < 10; j++)
            {
                node.FixedConstraints(node.state.pos, node.prevNode.state.pos, node.desiredDist, node.compensate1,
                    node.compensate2);
                /*node.AddMinConstraints(node.state.pos, node.prevNode.state.pos, node.desiredDist, node.compensate1, node.compensate2);
                node.AddMaxConstraints(node.state.pos, node.prevNode.state.pos, node.desiredDist, node.compensate1, node.compensate2);
            #1#
            }
            if (node.state.pos.y < -3.5f)
            {
                node.state.pos.y = -3.5f;
            }

            //node.transform.position = node.state.pos;
        }
        /*for (int i = 1; i < allNodes.Count; i++)
        {
            VerletNodeScript node = allNodes[i];
            node.AddMinConstraints(node.state.pos, node.prevNode.state.pos, node.desiredDist, node.compensate1, node.compensate2);
            node.AddMaxConstraints(node.state.pos, node.prevNode.state.pos, node.desiredDist, node.compensate1, node.compensate2);
        }
        for (int i = 1; i < allNodes.Count; i++)
        {
            VerletNodeScript node = allNodes[i];
            node.state.integrate();
        }#1#
    }*/
}
