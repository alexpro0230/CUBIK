using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class storeItemScript : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private scriptableObjects item;
    public TextMeshProUGUI price;
    private Button button;
    public moneyManagerScript moneyManager;
    public AudioSource mouseOverClick;
    public AudioClip mouseOverSFX;

    
    private void Start() {
        item.childCanvasOrder = transform.parent.GetSiblingIndex();

        Debug.Log("child index of button: " + item.childCanvasOrder);
        price = gameObject.transform.Find("Text (TMP)").transform.GetComponent<TextMeshProUGUI>();
        price.text = item.price.ToString();
        Image icon = GetComponent<Image>();
        icon.sprite = item.Image;
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { Buy(); });
        moneyManager = GameObject.FindGameObjectWithTag("money manager").GetComponent<moneyManagerScript>();
        mouseOverClick = GameObject.FindWithTag("OnMouseOverSound").GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData ped)
    {
        mouseOverClick.PlayOneShot(mouseOverSFX);
    }

    public void Buy()
    {
        Transform wmGo = GameObject.Find("player").transform.Find("weapon manager").transform;
        weapon_manager wm = wmGo.GetComponent<weapon_manager>();
        if(moneyManager.money >= item.price && wm.hasWeapons[item.id - 1] == false)
        {
            moneyManager.money -= item.price;
            realizeBuyingProcess();
        }
        else
        {
            if(wm.hasWeapons[item.id - 1] == false)Debug.Log("Not enough Money. Current money: "+ moneyManager.money);
            if(wm.hasWeapons[item.id - 1])Debug.Log("You already own this item");
        }
    }

    public void realizeBuyingProcess()
    {
        Transform target = GameObject.Find("player").transform.Find("weapon manager").transform;
        GameObject instantiation = Instantiate(item.prefab, GameObject.Find("player").transform.position, Quaternion.Euler(new Vector3(0, 0, GameObject.Find("player").transform.Find("weapon manager").transform.GetComponent<rotateGun>().angle)), target);
        instantiation.SetActive(false);
        weapon_manager wm = target.GetComponent<weapon_manager>();
        wm.hasWeapons[item.id - 1] = true;
    }
}
