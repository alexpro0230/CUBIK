using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class enemyBullet : MonoBehaviour
{
    [Header("gameObjects")]
    public GameObject explosion;
    public GameObject explsionSFX;
    public GameObject playerGameObject;
    
    [Header("layerMask")]
    public LayerMask enemy;
    public LayerMask player;

    [Header("shake")]
    public float amplitudeGain;
    public float frequencyGain;
    
    [Header("components")]
    public cinemaChinecameraShake cinemachiShake;
    public movement movementScript;

    [Header("other variables")]
    public int damage;
    
    void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("player");
        cinemachiShake = GameObject.FindWithTag("cinemachineShake").GetComponent<cinemaChinecameraShake>();
        movementScript = playerGameObject.GetComponent<movement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.transform.tag != "bullet")
        {
            GameObject expsfx = Instantiate(explsionSFX, transform.position, Quaternion.identity);
            GameObject bulletEffectExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(expsfx, 1f);
            Destroy(gameObject);
            shakeCamera();
            //CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1);
            Destroy(bulletEffectExplosion, 2);

            if(collision.gameObject.transform.tag == "player")
            {
                movementScript.health -= damage;
            }
        }
    }
  
    void shakeCamera()
    {
        cinemachiShake.ShakeCamera(amplitudeGain, frequencyGain);
    }
}