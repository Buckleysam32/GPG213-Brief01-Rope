using System.Collections;
using System.Collections.Generic;
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
    //public void Constraints
}
