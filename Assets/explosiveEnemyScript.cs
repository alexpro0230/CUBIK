using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosiveEnemyScript : MonoBehaviour
{
    public float health;
    public int damage;

    //explosion damage radius
    public float hitRadius;
    
    //the distance needed for the enemy to come flying towards the player
    public float attackRadius;

    //Dashing system
    private bool isDashing;
    private Vector3 dashDirection;
    public float dashForce;
    
    private void Start()
    {
        isDashing = false;
    }

    private void Update()
    {
        if (health <= 0)
        {
            Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, hitRadius);
            explode(coll);
        }

        if(isDashing)
        {
            transform.position += dashDirection * Time.deltaTime * dashForce;
        }

        if (Vector3.Distance(transform.parent.position, GameObject.Find("player").transform.position) <= attackRadius && !isDashing)
        {
            isDashing = true;
            dashDirection = GameObject.Find("player").transform.position - transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, hitRadius);
        explode(coll);
    }

    private void explode(Collider2D[] coll)
    {

        foreach (Collider2D col in coll)
        {
            if (col.gameObject.name == "player")
            {
                col.GetComponent<movement>().health -= damage;
            }
            else if (col.gameObject.tag == "enemy")
            {
                enemyScript _script = null;
                col.TryGetComponent<enemyScript>(out _script);
                if (_script != null) _script.health -= damage;
            }
        }

        Destroy(transform.parent.gameObject);
    }
}
