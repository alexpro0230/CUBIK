using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraStopOnLevelFinish : MonoBehaviour
{

    public enum coordinate
    {
        x,
        u,
        z
    }

    public coordinate stopAtAxis; //if we want to stop at a specifin accis of the next variable
    public Vector3 stopPosition; //The coordinates after the which we would stop moving the camera fowrard
    private GameObject endLevelCamera; //the camera that would be transistioned into

    void Start()
    {
        endLevelCamera = GameObject.Find("camera things").transform.Find("CM vcam1").gameObject;    
    }

    void Update()
    {
        if(stopAtAxis == coordinate.x)
        {
            if(transform.position.x >= stopPosition.x)
            {
                endLevelCamera.GetComponent<CinemachineVirtualCamera>().Follow = null;
                
            }
            else
            {
                endLevelCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform;
            }
        }
    }
}
