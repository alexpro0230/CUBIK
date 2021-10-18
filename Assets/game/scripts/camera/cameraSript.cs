using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSript : MonoBehaviour
{
    public GameObject player;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = player.transform.position + new Vector3(0,0,-10);

        Vector3 smoothedPos = Vector3.Lerp(transform.position, difference, speed * Time.deltaTime);

        transform.position = smoothedPos;
    }
}
