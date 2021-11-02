using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosiveEnemyScript : MonoBehaviour
{
    public float health;
    public int damage;

    //explosion damage radius
    public float hitRadius;

    //Dashing system
    private bool dashing;
    private Vector3 dashDirection;
    
    
    private void Start()
    {
        dashing = false;
    }

    private void Update()
    {
        if (health <= 0)
            Destroy(transform.parent.gameObject);

        if(dashing)
        {
            //dont remove, just comment if next doesnt work
            //transform.position = Vector3.Lerp(transform.position, GameObject.Find("player").transform.position, Time.deltaTime);
            
            //use this, if works ofc
            transform.position += dashDirection * Time.deltaTime * 5;    
        }

        if (Vector3.Distance(transform.parent.position, GameObject.Find("player").transform.position) <= 6 && !dashing)
        {
            dashing = true;
            dashDirection = GameObject.Find("player").transform.position - transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, hitRadius);

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
                _script.health -= damage;
            }
        }

        Destroy(transform.parent.gameObject);
    }
}
