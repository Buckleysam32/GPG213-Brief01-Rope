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
            }
            //Else it is a dynamic node, set its fixed bool and other variables.
            else
            {
                newNode.isFixed = false;                
                newNode.prevNode = allNodes[i - 1].GetComponent<VerletNodeScript>();
                newNode.desiredDist = managerDist;
                newNode.mass = 1f;
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
    void FixedUpdate()
    {
        foreach (VerletNodeScript node in allNodes)
        {
            
        }
        //Public void updateallnodes(list allnodes)
        //List<Nodes> foreach constraint();
        //List<Nodes> foreach state.integrate();
        //List<Nodes> foreach state.Addforce gravity();
    }
}
