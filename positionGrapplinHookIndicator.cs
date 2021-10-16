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
                found = true;
                if (!coll.gameObject.GetComponent<Collider2D>().bounds.Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    //GetComponent<LineRenderer>().positionCount = 2;
                    //GetComponent<LineRenderer>().SetPosition(0, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    //GetComponent<LineRenderer>().SetPosition(1, Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), coll.transform.position).point);
                    gameObject.transform.position =
                        Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), coll.transform.position).point;
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }

        if (!found) GetComponent<SpriteRenderer>().enabled = false;
        
    }
}
