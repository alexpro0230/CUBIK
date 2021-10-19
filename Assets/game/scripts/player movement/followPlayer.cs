using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector2 offset;

    private void Start()
    {
        player = GameObject.Find("player");
    }

    void Update()
    {
        Vector2 vec = new Vector2(player.transform.position.x, player.transform.position.y);
        transform.position = vec + offset;
    }
}
