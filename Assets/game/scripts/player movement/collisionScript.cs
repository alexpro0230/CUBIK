using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionScript : MonoBehaviour
{
    movement movement;

    private void Start()
    {
        movement = GetComponent<movement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        findTriggerType(collision);
    }

    void findTriggerType(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            LavaCollision();
        }

        switch (collision.gameObject.tag)
        {
            case "win trigger":
                movement.gameMenuScript.win();
                break;

            case "speed p-up":
                movement.speedPowerUp(collision);
                break;

            case "deleteOnEnter":
                string num = collision.gameObject.name.Replace("trigger instruction ", "");
                if (GameObject.Find("Instruction") == null)
                    break;   
                
                if (GameObject.Find("Instruction").GetComponent<floatingInstructionSystem>().currentInstruction < int.Parse(num))
                    GameObject.Find("Instruction").GetComponent<floatingInstructionSystem>().SetInsturcion(int.Parse(num));
                break;

            case "TriggerEnemyAttack":
                triggerEnemyAttackSingle triggerEnemyAttackSingle = collision.gameObject.GetComponent<triggerEnemyAttackSingle>();
                triggerEnemyAttackSingle.startEnemyAttack();
                break;

            case "jetpack":
                movement.hasJetpack = true;
                movement.slowMoTimeLeft = movement.slowMoTime;
                Destroy(collision.gameObject);
                break;

            case "jump power up":
                movement.jumpPowerUp(collision.gameObject);
                break;

            default:
                Debug.Log("collision with object without known tag dettected, collision variable: " + collision);
                break;
        }
    }

    private void LavaCollision()
    {
        movement.health -= 40;
        movement.rb.AddForce(Vector2.up * 200000);
        GameObject instantiatedObject = Instantiate(movement.lavaCollEffect, new Vector3(transform.position.x, transform.position.y - 0.5f), Quaternion.identity);
        StartCoroutine(movement.deleteAfterTime(1f, instantiatedObject));
    }
}
