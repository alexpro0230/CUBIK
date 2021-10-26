using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootBullet : MonoBehaviour
{
    [Header("gameObjects")]
    public GameObject shootPoint;
    public GameObject bulletPrefab;
    
    [Header("Variables")]
    public float bullletSpeed;
    public bool canShoot;
    public float countDown;
    public float recoverTime;
    public bool isAuto;
    private bool isfierying;
    public float bullets;

    [Header("flame thorower settings")]
    public bool isFlameThrower;
    public GameObject flame;
    private GameObject _flame;

    [Header("components")]
    public movement movement;

    void Start()
    {
        GameObject playerGo = GameObject.Find("player").gameObject;
        movement = playerGo.GetComponent<movement>();
        bullets = movement.bullets;
        canShoot = true;
    }

    void Update()
    {
        if(isfierying && countDown <= 0 && bullets > 0 && canShoot)
        {
            bullets--;
            if (!isFlameThrower)
            {
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.transform.position, gameObject.transform.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(shootPoint.transform.right * bullletSpeed, ForceMode2D.Impulse);
                countDown = recoverTime;
            }
        }

        countDown = Mathf.Clamp(countDown, 0, recoverTime);
        countDown -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Mouse0) && countDown <= 0 && canShoot && bullets > 0)
        {
            if (!isAuto)
            {
                bullets--;
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.transform.position, gameObject.transform.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(shootPoint.transform.right * bullletSpeed, ForceMode2D.Impulse);
                countDown = recoverTime;
            }
            else if(!isFlameThrower)
            {
                isfierying = true;
            }
            else if(isFlameThrower)
            {
                GameObject flame_ = Instantiate(flame, shootPoint.transform.position, gameObject.transform.rotation, gameObject.transform);
                flame_.transform.localScale = new Vector3(1, 1, 1);
                _flame = flame_;
                ParticleSystem partSys = flame_.GetComponent<ParticleSystem>();

                partSys.Play();
            }
        }

        if(bullets <= 0 && isFlameThrower)
        {
            Destroy(_flame);
        }

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            isfierying = false;
            Destroy(_flame);
        }
    }
}
