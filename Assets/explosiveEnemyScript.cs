using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosiveEnemyScript : MonoBehaviour
{
    public int life;
    public int damage;

    public float hitRadius;

    private bool dashing;

    private void Start()
    {
        dashing = false;
    }

    private void Update()
    {
        if(dashing)
        {
            transform.position = Vector3.Lerp(transform.position, GameObject.Find("player").transform.position, Time.deltaTime);
        }

        if (Vector3.Distance(transform.parent.position, GameObject.Find("player").transform.position) < 3 && !dashing)
        {
            dashing = true;
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
                enemyScript script = null;
                col.TryGetComponent<enemyScript>(out script);
                script.health -= damage;
            }
        }
    }
}
