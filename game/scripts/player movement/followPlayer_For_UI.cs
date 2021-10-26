using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer_For_UI : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(player.transform.position);
    }
}
