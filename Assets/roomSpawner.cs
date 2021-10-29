using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomSpawner : MonoBehaviour
{
    public enum rooms
    {
        top,
        down,
        left,
        right
    }

    public rooms exit;

    private void Awake()
    {
        Debug.Log("on awake");
    }

    private void Start()
    {
        Debug.Log("start");
    }

    private void OnEnable()
    {
        Debug.Log("on enable");
    }
}
