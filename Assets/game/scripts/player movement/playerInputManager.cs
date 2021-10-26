using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInputManager : MonoBehaviour
{

    movement movement;

    void Start()
    {
        movement = GetComponent<movement>();
    }

    void Update()
    {
        
    }

    void takeInput()
    {
        if (!movement.lockMovement)
            takeJumpInput();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            movement.slowMo();
        }
    }

    void takeJumpInput()
    {
        if (!movement.hasJetpack)
        {
            if (movement.grounded)
            {
                movement.hangCounter = movement.hangTime;
            }
            else
            {
                movement.hangCounter -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                movement.JumpBufferCount = movement.JumpBufferLength;
            }
            else
            {
                movement.JumpBufferCount -= Time.deltaTime;
            }

            if (movement.JumpBufferCount >= 0 && movement.hangCounter > 0)
            {
                jump();
                movement.JumpBufferCount = 0;
            }

            if (Input.GetKeyUp(KeyCode.Space) && movement.rb.velocity.y > 0)
            {
                movement.rb.velocity = new Vector2(movement.rb.velocity.x, movement.rb.velocity.y * .30f);
            }
        }
        else
        {
            movement.healthBar.gameObject.transform.parent.Find("jetpack counter bar").gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space) && movement.jetPackTimeLeft > 0)
            {
                movement.jetPacking = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                movement.jetPacking = false;
            }
        }
    }

    void jump()
    {
        movement.rb.velocity = new Vector2(movement.rb.velocity.x, movement.jumpForce);
    }
}
