using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeJordanRedoScript : MonoBehaviour
{
    public Vector2 nodePos, nodePrevPos;
    public bool isLocked;
    public float mass;
    public ConstraintJordanScript constraintA, constraintB;
    public LineRenderer nodeLinerenderer;

    //Every update make sure the line renderer is pointing at the correct positions for each of our constraints
    private void Update()
    {
        //set the first position of the linerenderer to our position.
        nodeLinerenderer.SetPosition(0, transform.position);
        //If this node has a vertical constraint
        if (constraintA)
        {
            //place the first position at the other node on that constraint, drawing a line
            nodeLinerenderer.SetPosition(1, constraintA.nodeA.transform.position);
        }
        else
        {
            //else keep it on us
            nodeLinerenderer.SetPosition(1, transform.position);
        }
        //Bring the second position back, so when we draw our next line it starts from this node, instead of the 
        //other node's position.
        nodeLinerenderer.SetPosition(2, transform.position);
        if (constraintB)
        {
            //If we have a horizontal contstraint, update our next position
            nodeLinerenderer.SetPosition(3, constraintB.nodeB.transform.position);
        }
        else
        {
            //else keep it on us
            nodeLinerenderer.SetPosition(3, transform.position);
        }
    }

    //When the mouse clicks and drags a node, move it to the mouse
    private void OnMouseDrag()
    {
        Vector3 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempPos.z = 0f;
        nodePos = tempPos;
    }
    //When the mouse stops dragging, update position.
    private void OnMouseUp()
    {
        transform.position = nodePos;
    }
}