using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInstantiate : MonoBehaviour
{
    public delegate void LevelingReadyHandler();
    //public event LevelingReadyHandler LevelingReady;
    [SerializeField] private GameObject CanvasPowerUps;
    private GameObject CanvasPowerUpsX;
    private Animator AnimatorButtos;

    // Start is called before the first frame update
    void Start()
    {
        Leveling.instance.OnLvlChanged += LevelingReadyMet;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LevelingReadyMet() {

        CanvasPowerUps.SetActive(true);
        
    }
}
