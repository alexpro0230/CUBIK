using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class bulletScript : MonoBehaviour
{
    [Header("gameObjects")]
    public GameObject explosion;
    public GameObject explsionSFX;
    public GameObject playerGameObject;
    
    [Header("layerMask")]
    public LayerMask player;

    [Header("shake")]
    public float amplitudeGain;
    public float frequencyGain;
    
    [Header("components")]
    public cinemaChinecameraShake cinemachiShake;

    [Header("damage")]
    public float damage;
    public float damageRandomization;
    public AnimationCurve damageOverDistance;

    private void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("player");
        cinemachiShake = GameObject.FindWithTag("cinemachineShake").GetComponent<cinemaChinecameraShake>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.transform.tag != "player" && collision.gameObject.transform.tag != "bullet")
        {
            GameObject expsfx = Instantiate(explsionSFX, transform.position, Quaternion.identity);
            GameObject bulletEffectExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(expsfx, 1f);
            Destroy(gameObject);
            shakeCamera();
            Destroy(bulletEffectExplosion, 2);
            
            if(collision.gameObject.tag == "enemy")
            {
                float distanceBetweenPlayerAndEnemy = Vector3.Distance(GameObject.Find("player").transform.position, collision.gameObject.transform.position);
                float baseDamageAtPointInTime = damageOverDistance.Evaluate(distanceBetweenPlayerAndEnemy);
                damage = Random.Range(baseDamageAtPointInTime - damageRandomization, baseDamageAtPointInTime + damageRandomization);
                enemyScript EnemyScript = null; 
                collision.gameObject.TryGetComponent<enemyScript>(out EnemyScript);
                if (EnemyScript != null)
                {
                    EnemyScript.health -= damage;
                    damagePopup.Create(collision.transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f)), damage.ToString("0.0"), 3);
                }
                else
                {
                    explosiveEnemyScript enemyScript = collision.gameObject.GetComponent<explosiveEnemyScript>();
                    enemyScript.health -= damage;
                    damagePopup.Create(collision.transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f)), damage.ToString("0.0"), 3);

                }
            }
            else if(collision.gameObject.tag == "grenade")
            {
                collision.gameObject.GetComponent<grenadeInstanceScript>().explode();
            }
        }
    }

    void shakeCamera()
    {
        cinemachiShake.ShakeCamera(amplitudeGain, frequencyGain);
    }
}