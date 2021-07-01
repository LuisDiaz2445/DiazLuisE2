using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public static int Points;

    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
        Points = 0;
    }

    private void Update()
    {
        text.text = "Points " + Points;
    }
}
