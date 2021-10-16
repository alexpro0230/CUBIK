using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionGrapplinHookIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool found = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 3);
        
        foreach(Collider2D coll in colliders)
        {
            if(coll.gameObject.tag == "grappable")
            {
                LineRenderer lr = GetComponent<LineRenderer>();
                found = true;
                GetComponent<SpriteRenderer>().enabled = true;
                //lr.positionCount = 2;
                //lr.SetPosition(0, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                //lr.SetPosition(1, Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), coll.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).point);
                gameObject.transform.position =
                    Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), (coll.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition))).point;
                if (coll.bounds.Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                {
                    Debug.Log("lol");
                    found = false;
                }
            }
        }

        if (!found) GetComponent<SpriteRenderer>().enabled = false;
        if (!found) GetComponent<LineRenderer>().positionCount = 0;
        
    }
}


//