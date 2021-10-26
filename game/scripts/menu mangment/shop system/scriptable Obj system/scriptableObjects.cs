using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newItem", menuName ="Item")]
public class scriptableObjects : ScriptableObject
{
    public int id;
    public int price;
    public GameObject prefab;
    public Sprite Image;
    public bool have;
    public int childCanvasOrder;
    
}
