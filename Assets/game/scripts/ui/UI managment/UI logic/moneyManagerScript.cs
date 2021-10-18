using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class moneyManagerScript : MonoBehaviour
{
    public int money;
    public TextMeshProUGUI moneyCounter;

    private void Start() {
        
        PlayerPrefs.SetInt("money", money);
        
        money = PlayerPrefs.GetInt("money");
    }
    private void Update() {
        moneyCounter.text = money.ToString();
        PlayerPrefs.SetInt("money", money);
    }
}
