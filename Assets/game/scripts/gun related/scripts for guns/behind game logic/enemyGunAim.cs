using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGunAim : MonoBehaviour
{
    [Header("gameobjects")]
    private GameObject player;
    public GameObject enemy;
    public GameObject bulletPref;

    [Header("variables")]
    public float aimRadius;
    private float recoveringTime;
    public float recoverTime;
    public bool auto;
    public float bulletSpeed;

    [Header("transforms")]
    public Transform shootPoint;

    private void Start()
    {
        player = GameObject.Find("player");    
    }

    private void Update()
    { 
        Vector2 distanceToPlayer = player.transform.position - enemy.transform.position;
        if(distanceToPlayer.magnitude <= aimRadius)
        {
            Vector3 normalizedDstToPlayer = distanceToPlayer.normalized;
            float angle = Mathf.Atan2(normalizedDstToPlayer.y, normalizedDstToPlayer.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);
            shooting();
        }
    }

    private void shooting()
    {
        recoveringTime -= Time.deltaTime;
        if(!auto)
        {
            if(recoveringTime <= 0)
            {
                GameObject isntantiatedBullet = Instantiate(bulletPref, shootPoint.position, transform.rotation);
                Rigidbody2D rb = isntantiatedBullet.GetComponent<Rigidbody2D>();

                isntantiatedBullet.GetComponent<enemyBullet>().damage = transform.parent.GetComponent<enemyScript>().damage;

                rb.AddForce(shootPoint.transform.right * bulletSpeed, ForceMode2D.Impulse);

                recoveringTime = recoverTime;
            }
        }
    }
}
