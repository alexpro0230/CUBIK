using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class bulletCountScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public shootBullet currentShootBulletScript;
    public weapon_manager weapon_Manager;

    void Start() 
    {
        text = GetComponent<TextMeshProUGUI>();    
    }
    void Update() 
    {
        GameObject wmGo = weapon_Manager.gameObject;

        int CurrentChildIndex = weapon_Manager.selectedWeapon;
        GameObject currentShtBlltGo = wmGo.transform.GetChild(CurrentChildIndex).gameObject;
        currentShtBlltGo.TryGetComponent<shootBullet>(out currentShootBulletScript);
        grenadeScript grenadeScript;
        currentShtBlltGo.TryGetComponent<grenadeScript>(out grenadeScript); 
        try
        {
            text.text = currentShootBulletScript.bullets.ToString();
            //text.text = grenadeScript.grenadeAmount.ToString();
        }
        catch
        {
            
        }
    }
}
