using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour, IDamage
{
    public int damage;

    public float health;

    public float Health { get ; set ; }

    void Start()
    {
        Health = health;
    }    
    
    void Update()
    {
        if(Health <= 0) Destroy(transform.parent.gameObject);
    }
}
