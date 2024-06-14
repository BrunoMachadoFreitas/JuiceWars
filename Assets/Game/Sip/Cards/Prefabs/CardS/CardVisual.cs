using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardVisual : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SmoothFollow();
    }

    private void SmoothFollow()
    {
        transform.position = Vector2.Lerp(transform.position, transform.parent.position + new Vector3(1,1), 5f * Time.deltaTime);
    }
}
