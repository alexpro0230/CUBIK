using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class floatingInstructionSystem : MonoBehaviour
{
    public List<string> instrunctionsChange = new List<string>();
    public int currentInstruction;
    public Vector3 offset;

    private void Start()
    {
        currentInstruction = -1;
        textMesh = GetComponent<TextMeshPro>();
        textColor = textMesh.color;
    }

    private void Update()
    {
        transform.position = GameObject.Find("player").transform.position + offset;
        Color col = GetComponent<TextMeshPro>().color;
        col.a += Time.deltaTime;
        textMesh.color = col;
    }

    private TextMeshPro textMesh;
    private Color textColor;

    public void SetInsturcion(int i)
    {
        currentInstruction = i;
        textMesh.text = instrunctionsChange[i];
        textMesh.color = new Color(textColor.r, textColor.g, textColor.b, 0);
    }
}
