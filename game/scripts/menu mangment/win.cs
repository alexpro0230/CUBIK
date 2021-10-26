using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class win : MonoBehaviour
{
    public GameObject winMenu;
    void Start()
    {
        winMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _win()
    {
        Time.timeScale = 0;
        winMenu.SetActive(true);
    }
}
