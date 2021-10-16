using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class storeGoScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter( PointerEventData ped ) 
    {
        GameObject.FindWithTag("weapon manager").GetComponent<weapon_manager>().canSwitch = false;
        try
        {
            GameObject.FindWithTag("weapon manager").transform.GetChild(GameObject.FindWithTag("weapon manager").GetComponent<weapon_manager>().selectedWeapon).GetComponent<shootBullet>().canShoot = false;   
        }
        catch
        {
        }
    }

    public void OnPointerExit( PointerEventData ped )
    {
        GameObject.FindWithTag("weapon manager").GetComponent<weapon_manager>().canSwitch = true;
        try
        {
            GameObject.FindWithTag("weapon manager").transform.GetChild(GameObject.FindWithTag("weapon manager").GetComponent<weapon_manager>().selectedWeapon).GetComponent<shootBullet>().canShoot = true;   
        }
        catch
        {
        }
    }
}
