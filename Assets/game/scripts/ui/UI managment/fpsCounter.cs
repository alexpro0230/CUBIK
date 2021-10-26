using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fpsCounter : MonoBehaviour
{
    private float count;
    private void Start()
    {
        count = 0f;
    }
    void Update()
    {
        if (count / 10 == (int)(count / 10))
        {
            float fps = 1 / Time.unscaledDeltaTime;
            GetComponent<Text>().text = "" + fps;
        }
    }
}
