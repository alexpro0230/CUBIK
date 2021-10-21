using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class repteatButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject.Find("player").GetComponent<movement>().buttonHover = true;
        GameObject weapon = GameObject.Find("player").transform.Find("weapon manager").gameObject;
        shootBullet shootBullet;
        weapon.transform.GetChild(weapon.GetComponent<weapon_manager>().selectedWeapon).TryGetComponent<shootBullet>(out shootBullet);
        shootBullet.canShoot = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject.Find("player").GetComponent<movement>().buttonHover = false;
        GameObject weapon = GameObject.Find("player").transform.Find("weapon manager").gameObject;
        shootBullet shootBullet;
        weapon.transform.GetChild(weapon.GetComponent<weapon_manager>().selectedWeapon).TryGetComponent<shootBullet>(out shootBullet);
        shootBullet.canShoot = true;
    }
}
