using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceController : MonoBehaviour
{
    [SerializeField] private CheckDevice deviceCheck;
    public static DeviceController instance;
    //Variavel para controlo da plaforma
    public TargetPlatform TargetPlatform;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (deviceCheck.isTouchInterface)
        {
            TargetPlatform = TargetPlatform.Android;

        }
        else
        {
            TargetPlatform = TargetPlatform.PC;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
