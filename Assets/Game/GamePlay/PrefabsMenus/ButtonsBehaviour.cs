using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColor(Color colorButton)
    {
        // Change the color of the GameObject to red when the mouse is over GameObject
        colorButton.a = 255;
    }
}
