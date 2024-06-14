using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemOnUiManager : MonoBehaviour
{
    public static PlayerItemOnUiManager instance;
    [SerializeField] private List<Image> PLayerItemsOnUiList;
    [SerializeField] private Image DefultImage;
    [SerializeField] private Material DefultMaterial;


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
      
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ManageItemsOfPLayer()
    {

        for (int j = 0; j < Player_Main.instance.WeaponsSpots.Count; j++)
        {


           
                if (Player_Main.instance.WeaponsSpots[j].transform.childCount == 1)
                {
                    if (Player_Main.instance.WeaponsSpots[j].transform.GetChild(0) != null)
                    {


                        PLayerItemsOnUiList[j].transform.GetChild(0).GetComponent<Image>().sprite = Player_Main.instance.WeaponsSpots[j].transform.GetChild(0).GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite;

                        //if (Player_Main.instance.WeaponsSpots[j].GetComponent<WeaponControll>().GetComponentInChildren<MiniGun_Main>())
                        //{


                        //    //if (Player_Main.instance.WeaponsSpots[j].GetComponent<WeaponControll>().GetComponentInChildren<MiniGun_Main>().lvlUpgrade == 1)
                        //    //{
                        //    //    //PLayerItemsOnUiList[j].GetComponent<Image>().color = Color.blue;
                        //    //}

                        //}

                    }
                }
            

            if (!Player_Main.instance.WeaponsSpots[j].activeSelf)
            {
                PLayerItemsOnUiList[j].transform.GetChild(0).GetComponent<Image>().sprite = DefultImage.sprite;
                PLayerItemsOnUiList[j].transform.GetChild(0).GetComponent<Image>().material = DefultMaterial;
            }

        }
    }
}
