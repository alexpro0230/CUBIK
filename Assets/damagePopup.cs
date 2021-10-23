using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class damagePopup : MonoBehaviour
{

    private float countDown;

    public static damagePopup Create(Vector2 pos, string damage, int time)
    {
        GameObject instance = Instantiate(GameObjHodler._i.damagePopUp, pos, Quaternion.identity);
        instance.GetComponent<TextMeshPro>().text = damage;
        instance.GetComponent<damagePopup>().countDown = time;
        return instance.GetComponent<damagePopup>();
    }

    private void Update()
    {
        countDown -= Time.deltaTime;
        GetComponent<TextMeshPro>().color *= new Color(1, 1, 1, countDown);
        if (countDown <= 0) Destroy(gameObject);
    }
}
