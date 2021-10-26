using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenadeScript : MonoBehaviour
{
    //game objects 
    public GameObject dot;
    private GameObject[] dots;
    public GameObject grenadePrefab;
    public GameObject shootPoint;

    //Variables
    public float coolDownAmount;
    public int grenadeAmount;
    private float coolDown;
    public float grenadeExplosionTime;
    public float throwForce;
    public int numberOfDots;
    public float distanceBetweenDots;
    public int childIndexRelativelyToParent;
    public bool showTrajectory;
    public bool canThrow;
    private void Start() 
    {
        weapon_manager.switchedToGreanade += granadeSwitched;
        canThrow = true;
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dot, shootPoint.transform.position, Quaternion.identity);
            dots[i].transform.position = findPosition(i * distanceBetweenDots);
        }
    }

    private void Update() 
    {
        if(grenadeAmount <= 0) gameObject.SetActive(false);

        coolDown -= Time.deltaTime;

        if(grenadeAmount > 0 && canThrow)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && coolDown <= 0)
            {
                showTrajectory = true;
                //StartCoroutine(startExplosionCooldown());
            }

            if(Input.GetKeyUp(KeyCode.Mouse0) && coolDown <= 0)
            {         
                throwGrenade();
                grenadeAmount--;
                coolDown = coolDownAmount;
            }
        }
        else if(grenadeAmount == 0)
        {
            showTrajectory = false;
            transform.parent.GetComponent<weapon_manager>().selectedWeapon++;
            Destroy(gameObject);
            foreach(GameObject dot in dots)
            {
                Destroy(dot);
            }
        }

        if(showTrajectory)
        {
            for(int i = 0; i < numberOfDots; i++)
            {
                dots[i].SetActive(true);
                dots[i].transform.position = findPosition(i * distanceBetweenDots);
                dots[i].GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            if(coolDown > 0)
            {
                for(int i = 0; i < numberOfDots; i++)
                {
                    dots[i].GetComponent<SpriteRenderer>().color = Color.red;
                    dots[i].SetActive(true);
                    dots[i].transform.position = findPosition(i * distanceBetweenDots);
                }
            }
        }

        if(showTrajectory == false)
        {
            if(coolDown > 0)
            {
                for(int i = 0; i < numberOfDots; i++)
                {
                    dots[i].GetComponent<SpriteRenderer>().color = Color.red;
                    dots[i].SetActive(true);
                    dots[i].transform.position = findPosition(i * distanceBetweenDots);
                }
            }else
            {
                for(int i = 0; i < numberOfDots; i++)
                {
                    dots[i].SetActive(false);
                }
            }
        }
    }

    private void throwGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, shootPoint.transform.position, gameObject.transform.rotation);
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        grenadeInstanceScript gis = grenade.GetComponent<grenadeInstanceScript>();

        rb.velocity = grenade.transform.right * throwForce;
        gis.timeLeftForExplsion = grenadeExplosionTime;
        gis.startExplsionTimer();
    }
    
    /*
    private IEnumerator startExplosionCooldown()
    {
        if(grenadeExplosionTime <= 0) StopCoroutine(startExplosionCooldown());
        yield return new WaitForEndOfFrame();
        grenadeExplosionTime -= Time.deltaTime;
    }
    */

    private Vector2 findPosition(float t)
    {
        rotateGun rotateGun = transform.parent.GetComponent<rotateGun>();

        Vector2 position = (Vector2)transform.position + (rotateGun.aimDir * throwForce * t) + 0.5f * Physics2D.gravity * (t*t);
        return position;
    }

    private void granadeSwitched(object sender, EventArgs args)
    {
        showTrajectory = false;
    }
}
