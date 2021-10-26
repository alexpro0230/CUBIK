using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotScript : MonoBehaviour
{
    private void Update() {
        GameObject player = GameObject.Find("player");
        GameObject weaponManager = player.transform.Find("weapon manager").gameObject;
        int selectedWeapon = weaponManager.GetComponent<weapon_manager>().selectedWeapon;
        
        if(weaponManager.transform.GetChild(selectedWeapon).gameObject.tag != "grenade") gameObject.SetActive(false);

        if(weaponManager.transform.GetChild(selectedWeapon).gameObject.tag == "grenade")
        {
            if(weaponManager.transform.GetChild(selectedWeapon).GetComponent<grenadeScript>().grenadeAmount == 0) 
            {
                gameObject.SetActive(false);
            }
        }
    }
}
