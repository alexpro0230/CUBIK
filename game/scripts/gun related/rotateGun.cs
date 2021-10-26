using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class rotateGun : MonoBehaviour
{
    public Vector2 aimDir;
    public float angle;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePos = UtilsClass.GetMouseWorldPosition();

        aimDir = (mousePos - (Vector2)transform.position).normalized;

        angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
