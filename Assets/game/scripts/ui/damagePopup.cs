using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class damagePopup : MonoBehaviour
{

    private float countDown;
    public float ySpeed;

    public static damagePopup Create(Vector2 pos, string damage, int time)
    {
        GameObject instance = Instantiate(GameObjHodler._i.damagePopUp, pos, Quaternion.identity);
        instance.GetComponent<TextMeshPro>().text = damage;
        instance.GetComponent<damagePopup>().countDown = time;
        return instance.GetComponent<damagePopup>();
    }

    TextMeshPro textMesh;
    Color textColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textColor = textMesh.color;
    }

    private void Update()
    {
        countDown -= Time.deltaTime;
        transform.position += new Vector3(0, ySpeed) * Time.deltaTime;
        float disappearSpeed = 3f;
        textColor.a -= disappearSpeed * Time.deltaTime;

        textMesh.color = textColor;

        if (countDown <= 0) Destroy(gameObject);
    }
}
