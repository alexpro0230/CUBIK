using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstLevelScript : MonoBehaviour
{
    public List<GameObject> firstAttack = new List<GameObject>();
    public List<GameObject> SecondtAttack = new List<GameObject>();
    public List<GameObject> ThirdAttack = new List<GameObject>();

    [SerializeField]
    private bool finished1Attack;
    [SerializeField]
    private bool finished2Attack;
    [SerializeField]
    private bool finished3Attack;

    private bool cavasOnPrevFrame; //on the beggining of the game we need to wait until splash screen turns off, this is a variable to check it

    private void Start()
    {
        foreach (GameObject obj in firstAttack)
            obj.SetActive(false);

        foreach (GameObject obj in SecondtAttack)
            obj.SetActive(false);
        
        foreach (GameObject obj in ThirdAttack)
            obj.SetActive(false);
    }

    private void Update()
    {
        try
        {
            if (GameObject.Find("Canvas").activeInHierarchy && cavasOnPrevFrame)
            {
                foreach (GameObject obj in firstAttack)
                    obj.SetActive(true);
            }
        }
        catch
        {

        }

        foreach(GameObject go in firstAttack)
        {
            if (go.transform.Find("enemy gfx") != null)
            {
                finished1Attack = false;
                break;
            }
            else
                finished1Attack = true;
        }
        
        foreach(GameObject go in SecondtAttack)
        {
            if(go.transform.Find("enemy gfx") != null)
            {
                finished2Attack = false;
                break;
            }
            else
                finished2Attack = true;
        }
        
        foreach(GameObject go in ThirdAttack)
        {
            if(go.transform.Find("enemy gfx") != null)
            {
                finished3Attack = false;
                break;
            }
            else
                finished3Attack = true;
        }


        spawnIfNeeded();
        try
        {
            if (GameObject.Find("Canvas").activeInHierarchy)
                cavasOnPrevFrame = false;
        }
        catch
        {
            cavasOnPrevFrame = true;
        }
    }

    private void spawnIfNeeded()
    {
        //just finshed the first attack(start the second one)
        if (finished1Attack && !finished2Attack && !finished3Attack)
        {
            foreach (GameObject obj in SecondtAttack)
                obj.SetActive(true);
        }

        //finshed 2nd attack(start 3rd)
        if (finished1Attack && finished2Attack && !finished3Attack)
        {
            foreach (GameObject obj in ThirdAttack)
                obj.SetActive(true);
        }

        //killed all enemies, open door
        if (finished1Attack && finished2Attack && finished3Attack)
        {
            GameObject.Find("Door").GetComponent<doorScript>().openDoor = true;
        }
    }
}
