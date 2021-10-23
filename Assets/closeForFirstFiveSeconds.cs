using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeForFirstFiveSeconds : MonoBehaviour
{
    //The time you want to wit 
    public float timeToWait;
    //the time that has already been awaited
    private float timeWaited;

    public List<GameObject> objToClose = new List<GameObject>();

    void Start()
    {
        GameObject.Find("player").GetComponent<movement>().lockMovement = true;
        GameObject.Find("player").GetComponent<movement>().lockMusicPlay = true;
        GameObject.Find("player").GetComponent<movement>().lockShooting = true;
        
        timeWaited = timeToWait;

        foreach(GameObject obj in objToClose)
        {
            obj.SetActive(false);
        }
    }

    void Update()
    {
        timeWaited -= Time.deltaTime;
        if(timeWaited <= 0)
        {
            foreach(GameObject obj in objToClose)
            {
                obj.gameObject.SetActive(true);
            }

            GameObject.Find("player").GetComponent<movement>().lockMovement = false;
            GameObject.Find("player").GetComponent<movement>().lockMusicPlay = false;
            GameObject.Find("player").GetComponent<movement>().lockShooting = false;

            Destroy(gameObject);
        }
    }
}
