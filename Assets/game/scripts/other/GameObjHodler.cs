using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjHodler : MonoBehaviour
{
    private static GameObjHodler i;
    public static GameObjHodler _i
    {
        get
        {
            if (i == null)
            {
                i = (Instantiate(Resources.Load("GameObjHodler")) as GameObject).GetComponent<GameObjHodler>();
            }
            
            return i;
        }
    }

    public GameObject landParticeEffect;
    public GameObject jetpackVFX;
    public GameObject damagePopUp;

    [Header("\nRandom Room Generation System")]
    public GameObject[] upRooms;
    public GameObject[] downRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
}
 