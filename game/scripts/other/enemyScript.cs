using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public float health;
    public int damage;

    void Start()
    {
        
    }    
    
    void Update()
    {
        if(health <= 0) Destroy(gameObject);
    }

}
