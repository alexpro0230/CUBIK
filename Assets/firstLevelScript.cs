using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstLevelScript : MonoBehaviour
{
    [System.Serializable]
    public class listItem
        {
            public List<GameObject> list = new List<GameObject>();
        }
        public List<listItem> attacks = new List<listItem>();


    public bool[] finishedAttack;

    private bool cavasOnPrevFrame;

    private void Start()
    {
        foreach(listItem list in attacks)
        {
            foreach(GameObject obj in list.list)
            {
                obj.SetActive(false);
                Debug.Log("deactivated " + obj);
            }
        }
    }

    private void Update()
    {
        if(finishedAttack.Length != attacks.Count)
        {
            Debug.LogError("finished attack length is not equal to attacks length");
        }
        
        try
        {
            if (GameObject.Find("Canvas").activeInHierarchy && cavasOnPrevFrame)
            {
                foreach (GameObject obj in attacks[0].list)
                {
                    Debug.Log(obj);
                    obj.SetActive(true);
                }
            }
        }
        catch{}
        
        int count = 0;
        
        foreach(listItem list in attacks)
        {
            foreach(GameObject obj in list.list)
            {
                try
                {
                    if (obj != null)
                    {
                        finishedAttack[count] = false;
                        break;
                    }
                    else
                        finishedAttack[count] = true;
                }
                catch { }
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
                continue;
            }
            else
            {
                if (i == finishedAttack.Length - 1)
                    return;

                foreach(GameObject obj in attacks[i + 1].list)
                {
                    obj.SetActive(true);
                    return;
                }
            }
        }        
    }
}
