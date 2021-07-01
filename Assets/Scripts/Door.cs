using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
public class Door : MonoBehaviour
{
    TextMeshPro text;
    SpriteRenderer spriteRenderer;

    string[] abc = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    List<Color> DcolorList = new List<Color>()
    {
        Color.blue,
        Color.red,
        Color.green
    };
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Ejemplo para mostrar como se escribe texto
        SetDoorText(abc[Random.Range(0, abc.Length)]);
        SetDoorColor(DcolorList[Random.Range(0, DcolorList.Count)]);
    }

    void SetDoorColor(Color DoorColor)
    {
        spriteRenderer.color = DoorColor;
    }
    void SetDoorText(string textToDisplay)
    {
     
        text.SetText(textToDisplay);
    }

    
}
