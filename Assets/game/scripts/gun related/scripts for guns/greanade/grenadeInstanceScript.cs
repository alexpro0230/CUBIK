using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenadeInstanceScript : MonoBehaviour
{
    public GameObject explosionSFX;
    public GameObject explosionGFX;
    public float timeLeftForExplsion;
    public bool usedOnce;
    private bool startTimer;
    public float radius;
    public int damage;

    private void Update() 
    {
        if(timeLeftForExplsion <= 0 && usedOnce == false)
        {
            explode();
        }
        
        if(startTimer)
        {
            timeLeftForExplsion -= Time.deltaTime;
        }
    }

    public void explode()
    {
        usedOnce = true;
        GameObject VFX = Instantiate(explosionGFX, transform.position, Quaternion.identity);
        GameObject SFX = Instantiate(explosionSFX, transform.position, Quaternion.identity);
        takeDamage();
        StartCoroutine(destroySFXandVFX(SFX, VFX));
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        Destroy(gameObject, 4);
        startTimer = false;

    }

    void takeDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        movement movement;
        enemyScript[] enemyScript = new enemyScript[colliders.Length];
        
        for(int i = 0; i < colliders.Length; i++)
        {   
            colliders[i].TryGetComponent<enemyScript>(out enemyScript[i]);
            colliders[i].TryGetComponent<movement>(out movement);
            if(movement != null)
            {
                movement.Health -= damage;
            }
        }

        for(int i = 0; i < enemyScript.Length; i++)
        {
            if(enemyScript[i] != null)
            {
                enemyScript[i].Health -= damage;
            }
        }


    }

    IEnumerator destroySFXandVFX(GameObject SFX, GameObject VFX)
    {
        yield return new WaitForSeconds(3);
        Destroy(SFX);
        Destroy(VFX);
    }

    public void startExplsionTimer()
    {
        startTimer = true;
    }
}
