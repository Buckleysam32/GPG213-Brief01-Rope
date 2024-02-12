using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletNodeManager : MonoBehaviour
{
    public float managerDist;
    public int ropeSegments;
    public List<VerletNodeScript>
    // Start is called before the first frame update
    void Start()
    {
        //Generate a rope starting at fixed point, with distance d, 
        for (int i = 0; i < ropeSegments; i++)
        {
            //Node = Instantiate() + offset 
            //node.prevnode 
            //ifPrevnode.isFixed
            /*{
                compensate2 = 0, compensate1 = 1;
            }*/
            //state.pos = transform.position;
            //state.prevPos = state.pos;
            //state.force = Vector2.zero;
        }
    }
    void FixedUpdate()
    {
        //Public void updateallnodes(list allnodes)
        //List<Nodes> foreach state.integrate();
        //List<Nodes> foreach state.Addforce gravity();
        //List<Nodes> foreach constraint();
    }
}
