using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeGame_Main : MonoBehaviour
{
    [SerializeField] private GameObject hideCanvasChoose;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNextRound()
    {
        PowerUpsManager.instance.ResumeGame();
    }
}
