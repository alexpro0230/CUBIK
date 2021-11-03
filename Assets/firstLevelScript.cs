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


    public int currentAttack;

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

        #region spawn first time
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
        catch { }

        #endregion
        
        //dont do the following calculation if canvas is not on
        if(GameObject.Find("Canvas") == null)
        {
            currentAttack = 0;
            goto skip;
        }

        int count = currentAttack;

        foreach(GameObject obj in attacks[currentAttack].list)
        {
            if(obj == null)
            {
                currentAttack = count + 1;
            }
            else
            {
                currentAttack = count;
                break;
            }
        }

    //for goto statement
    skip:
        
        currentAttack = Mathf.Clamp(currentAttack, 0, attacks.Count - 1);
        
        if(GameObject.Find("Canvas") != null)
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
        foreach(GameObject obj in attacks[currentAttack].list)
        {
            if(obj != null)
                obj.SetActive(true);
        }
    }
}
