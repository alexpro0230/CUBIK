using System;
using UnityEngine;

public class weapon_manager : MonoBehaviour
{

    public int selectedWeapon;
    public bool canSwitch;
    public bool[] hasWeapons;
    public scriptableObjects[] items = new scriptableObjects[8];
    public static event EventHandler switchedToGreanade;

    void Start()
    {
#if UNITY_EDITOR
        PlayerPrefsX.SetBoolArray("player weapons", hasWeapons);
#endif
        canSwitch = true;
        saveWeapons();
        selectWeapon();
        canSwitch = true;
        string message = "Playerprefs saved weapons: \n";

        for (int i = 0; i < hasWeapons.Length; i++)
        {
            message += $"   Player prefs array 'saved weapons' at { i }: { PlayerPrefsX.GetBoolArray("player weapons")[i] } var type: bool\n";
        }

        Debug.Log(message);

        for(int i = 0; i < hasWeapons.Length; i++)
        {
            if(hasWeapons[i])
            {
                Instantiate(items[i].prefab, gameObject.transform);
            }
        }

        saveWeapons();
        selectWeapon();
    }

    
    void Update()
    {
        if (selectedWeapon > transform.childCount - 1)
            selectedWeapon = 0;
        if(selectedWeapon < 0)
            selectedWeapon = transform.childCount - 1;
            
        saveWeapons();

        int prev = selectedWeapon;

        if (canSwitch)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (selectedWeapon >= transform.childCount - 1)
                {
                    selectedWeapon = 0;
                }
                else
                {
                    selectedWeapon++;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (selectedWeapon <= 0)
                {
                    selectedWeapon = transform.childCount - 1;
                }
                else
                {
                    selectedWeapon--;
                }
            }

            if (prev != selectedWeapon)
            {
                selectWeapon();
            }
        }
    }

    void saveWeapons()
    {
        PlayerPrefsX.SetBoolArray("player weapons", hasWeapons);
    }

    void selectWeapon() 
    {
        int i = 0;
        foreach(Transform obj in transform)
        {
            if(i == selectedWeapon)
            {
                if (obj.transform.tag == "grenade" && obj.gameObject.GetComponent<grenadeScript>().grenadeAmount <= 0)
                    obj.gameObject.SetActive(false);
                obj.gameObject.SetActive(true);
            }
            else
            {
                if(obj.transform.tag == "grenade")
                {
                    if(switchedToGreanade != null) switchedToGreanade(this, EventArgs.Empty);
                }

                obj.gameObject.SetActive(false);
            }

            i++;
        } 
    }
}
