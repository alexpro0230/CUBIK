using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;
public class triggerEnemyAttackSingle : MonoBehaviour
{
    private AIDestinationSetter destinationSetter;
    private List<GameObject> enemy = new List<GameObject>();
    public List<string> enemyName = new List<string>();

    private void Start() 
    {

        foreach(string name in enemyName)
        {
            enemy.Add(GameObject.Find(name));
        }
        
        foreach(GameObject en in enemy)
        {
            if (en != null)
            {
                destinationSetter = en.GetComponent<AIDestinationSetter>();
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
