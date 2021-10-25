using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapplingGunScipt : MonoBehaviour
{
    movement movement;
    LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        movement = GetComponent<movement>();
    }

    private void Update()
    {
        grapplingGunCalculation();
    }

    void grapplingGunCalculation()
    {
        //if there was input for grappling gun acitvation and there's a wal close then attach to it
        if (Input.GetKeyDown(KeyCode.Mouse1) && GameObject.Find("grappling hook pointer").GetComponent<SpriteRenderer>().enabled)
        {
            movement.isGrappling = true;

            movement.joint = GetComponent<DistanceJoint2D>();
            movement.joint.connectedAnchor = GameObject.Find("grappling hook pointer").transform.position;

            lr.positionCount = 2;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, GetComponent<DistanceJoint2D>().connectedAnchor);
            lr.enabled = true;

            GetComponent<DistanceJoint2D>().enabled = true;
        }

        //stop grappling
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            movement.isGrappling = false;

            lr.enabled = false;
            movement.joint.enabled = false;
        }

        if (movement.isGrappling)
        {
            //if maximum angle is reached break rope
            if (GetComponent<DistanceJoint2D>().connectedAnchor.y - transform.position.y <= 0.5f)
            {
                Debug.Log("hahahahah");
                movement.rb.AddForce(Vector2.down * 500, ForceMode2D.Force);

                lr.enabled = false;
                movement.joint.enabled = false;
            }

            lr.positionCount = 2;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, GetComponent<DistanceJoint2D>().connectedAnchor);
        }
    }
}
