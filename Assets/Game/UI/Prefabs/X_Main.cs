using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X_Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Destr�i este objeto ap�s 3 segundos
         
            WaveManager.instance.Images.Remove(this.gameObject);
            Destroy(gameObject, 2f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
