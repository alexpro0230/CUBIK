using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;
public class triggerEnemyAttackSingle : MonoBehaviour
{
    public AIDestinationSetter destinationSetter;
    public GameObject enemy;
    public string enemyName;
    private void Start() 
    {
        if (string.IsNullOrEmpty(enemyName))
        {
            enemy = GameObject.Find("enemy");
            destinationSetter = enemy.GetComponent<AIDestinationSetter>();
            destinationSetter.target = null;
        }
        else
        {
            enemy = GameObject.Find(enemyName);
            
            if (enemy != null)
            {
                destinationSetter = enemy.GetComponent<AIDestinationSetter>();
                destinationSetter.target = null;
            }
            else
            {
                Debug.Log("enemy is null");
            }
        }
    }

    public void startEnemyAttack()
    {
        GameObject player = GameObject.Find("player");
        destinationSetter.target = player.transform;
    }
}
