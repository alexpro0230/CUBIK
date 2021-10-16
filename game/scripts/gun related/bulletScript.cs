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
            //CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1);
            Destroy(bulletEffectExplosion, 2);
            
            if(collision.gameObject.tag == "enemy")
            {
                enemyScript enemyScript = collision.gameObject.GetComponent<enemyScript>();
                enemyScript.health -= damage;
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