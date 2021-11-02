using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstLevelScript : MonoBehaviour
{
    public List<List<GameObject>> attacks = new List<List<GameObject>>();

    public bool[] finishedAttack;

    private void Start()
    {
        foreach(List<GameObject> list in attacks)
        {
            foreach(GameObject obj in list)
            {
                obj.SetActive(false);
                Debug.Log("deactivated " + obj);
            }
        }
    }

    private void Update()
    {
        if(finishedAttack.Length != attacks.Length)
        {
            Debug.LogError("finished attack length is not equal to attacks length");
        }
        try
        {
            if (GameObject.Find("Canvas").activeInHierarchy && cavasOnPrevFrame)
            {
                foreach (GameObject obj in attacks[0])
                    obj.SetActive(true);
            }
        }
        catch
        {}
        int count = 0;
        foreach(List<GameObject> list in attacks)
        {
            foreach(GameObject obj in list)
            {
                if (go.transform.Find("enemy gfx") != null)
                {
                    finishedAttack[count] = false;
                    break;
                }
                else
                    finishedAttack[count] = true;        
            }
            count++;
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
        for(int i = 0; i < finishedAttack.Length; i++)
        {
            if(finishedAttack[i])
            {
            }
            else
            {
                foreach(GameObject obj in attacks[i])
                {
                    obj.SetActive(true);
                }
            }
        }        
    }
}
