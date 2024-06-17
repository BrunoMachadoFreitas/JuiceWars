using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerUpsManager : MonoBehaviour
{

    public static PowerUpsManager instance;
    public Dictionary<string, bool> powerUps = new Dictionary<string, bool>();

    public int valueRerool = 3;


    [SerializeField] private GameObject hideCanvasChoose;
    public List<GameObject> PowerUpObject; // Lista que vai guardar o objeto pai que tem 1 botao 1 imagem 1 text para descri��o 1 text para pre�o
    public List<Sprite> ImagesForButtons; // Lista que vai guardar o objeto pai que tem 1 botao 1 imagem 1 text para descri��o 1 text para pre�o
    [SerializeField] List<string> Prices; // Lista que vai guardar o objeto pai que tem 1 botao 1 imagem 1 text para descri��o 1 text para pre�o
    [SerializeField] List<string> Descriptions; // Lista que vai guardar o objeto pai que tem 1 botao 1 imagem 1 text para descri��o 1 text para pre�o

    [SerializeField] List<string> WhatToBuy = new List<string>();
    public List<string> ListToSave = new List<string>();


    [SerializeField] private TextMeshProUGUI txtRerool;
    private bool CanVerify = false;


    [SerializeField] private GameObject Card;
    private GameObject CardX;

    List<GameObject> Cards;
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
        powerUps.Add("JuiceLife", DataManagment.instance.objectData.FoundJuiceLife);

        powerUps.Add("Pistol", DataManagment.instance.objectData.FoundPistol);
        powerUps.Add("MiniGun", DataManagment.instance.objectData.FoundMinigun);
        powerUps.Add("JuiceReg", DataManagment.instance.objectData.FoundReg);
        powerUps.Add("JuiceClub", DataManagment.instance.objectData.FoundJuiceClub);
        powerUps.Add("Boomerang", DataManagment.instance.objectData.FoundBoomerang);
        powerUps.Add("CardWine", DataManagment.instance.objectData.FoundCardWine);
        powerUps.Add("TequillaCard", DataManagment.instance.objectData.FoundTequillaCard);
        powerUps.Add("JuiceHole", DataManagment.instance.objectData.FoundJuiceHoleCard);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        txtRerool.text = valueRerool.ToString();
        if (CanVerify)
        {

        
            for(int i = 0; i < PowerUpObject.Count; i++)
            {
                if (Player_Main.instance.Money <  0 /*Convert.ToInt32(powerUps.ElementAt(i).Value*/)
                {
                    PowerUpObject[i].GetComponentInChildren<Image>().transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                    PowerUpObject[i].GetComponentInChildren<Button>().interactable = false;
                }
                else
                {
                    PowerUpObject[i].GetComponentInChildren<Image>().transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                    PowerUpObject[i].GetComponentInChildren<Button>().interactable = true;
                }

            }

        }
    }


    public void SaveItem1()
    {
        // Obtém o item no índice 0 do dicionário whatToBuy e adiciona à ListToSave
        ListToSave.Add(WhatToBuy[0]);
    }

    public void SaveItem2()
    {
        // Obtém o item no índice 1 do dicionário whatToBuy e adiciona à ListToSave
        ListToSave.Add(WhatToBuy[1]);
    }

    public void SaveItem3()
    {
        // Obtém o item no índice 2 do dicionário whatToBuy e adiciona à ListToSave
        ListToSave.Add(WhatToBuy[2]);
    }
    public void SaveItem4()
    {
        // Obtém o item no índice 3 do dicionário whatToBuy e adiciona à ListToSave
        ListToSave.Add(WhatToBuy[3]);
    }
    public void Rerool()
    {
        resetEveryThing();
        if (Player_Main.instance.Money >= 0)
        {
            CanVerify = false;
        
            for (int i = 0; i < PowerUpObject.Count ; i++)
            {
                RandomizePowerUpsForButtons(i);
            }

         
            //Player_Main.instance.Money -= valueRerool;
            valueRerool += 3;
        }

    }
    public void RandomizePowerUpsForButtons(int i)
    {
        try
        {

        
            int randomPowerUp = UnityEngine.Random.Range(0, powerUps.Count);
            PowerUpObject[i].GetComponentInChildren<Button>().image.sprite = ImagesForButtons[randomPowerUp];
            PowerUpObject[i].GetComponentInChildren<Button>().gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Descriptions[randomPowerUp];
            PowerUpObject[i].GetComponentInChildren<Image>().transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "0";
       
            WhatToBuy.Add(powerUps.ElementAt(randomPowerUp).Key);
            if (powerUps.ElementAt(randomPowerUp).Value == false)
            {
                    PowerUpObject[i].transform.GetChild(2).gameObject.SetActive(true);
            }
            CanVerify = true;
        }
        catch(Exception ex)
        {
            UnityEngine.Debug.Log(ex.Message);
        }


    }


    public void resetEveryThing()
    {

        WhatToBuy.Clear();
        for(int i = 0; i < PowerUpObject.Count; i++)
        {
            PowerUpObject[i].GetComponentInChildren<Button>().image.sprite = null;
            PowerUpObject[i].GetComponentInChildren<Button>().gameObject.GetComponentInChildren<TextMeshProUGUI>().text = null;
            PowerUpObject[i].GetComponentInChildren<Image>().transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = null;

        }
    }
    private void IncreaseLife(){

        if(Player_Main.instance.Money >= 0) {


            Player_Stats.instance.Life += 10;
            //Player_Main.instance.Money -= 10;
            
        }
        
    }
    private void RegLife()
    {

        if (Player_Main.instance.Money >= 0)
        {

            if(Player_Stats.instance.CurrentLife <= Player_Stats.instance.Life)
            Player_Stats.instance.CurrentLife += 10;
            //Player_Main.instance.Money -= 10;

        }

    }

   

    public void BuyBtn1()
    {
        switchPowerups(0);
        PowerUpObject[0].transform.GetChild(2).gameObject.SetActive(false);
        SoundManager.instance.GameSounds[3].Play();
        ResumeGame();
       
    }


   
    public void BuyBtn2()
    {
        switchPowerups(1);
        SoundManager.instance.GameSounds[3].Play();
        PowerUpObject[1].transform.GetChild(2).gameObject.SetActive(false);
        ResumeGame();
    }

    public void BuyBtn3()
    {
        switchPowerups(2);
        SoundManager.instance.GameSounds[3].Play();
        PowerUpObject[2].transform.GetChild(2).gameObject.SetActive(false);
        ResumeGame();
    }

    public void BuyBtn4()
    {
        switchPowerups(3);
        SoundManager.instance.GameSounds[3].Play();
        PowerUpObject[3].transform.GetChild(2).gameObject.SetActive(false);
        ResumeGame();
    }

    public void ResumeGame()
    {

        Player_Main.instance.CanMov = true;
        RoundsManager.instance.newwaveManagerPrefab.SetActive(true);

        hideCanvasChoose.SetActive(false);
        RoundsManager.instance.CanvasMobile.gameObject.SetActive(false);



        PowerUpsManager.instance.resetEveryThing();
        GameStateController.instance.currentGameState = GameState.Playing;

        if(Player_Main.instance.TargetPlatform == TargetPlatform.Android || Player_Main.instance.TargetPlatform == TargetPlatform.IOS)
        RoundsManager.instance.CanvasMobile.gameObject.SetActive(true);
      
        Player_Main.instance.CanvasExp.gameObject.SetActive(true);

        
       
    }
    private void switchPowerups(int elementAt)
    {
        switch (WhatToBuy.ElementAt(elementAt))
        {
            case "Pistol":
                if (Player_Main.instance.Money >= 0)
                {


                    GameObject weaponSpotAux = null;

                    // Verifica se há um local de arma vazio
                    foreach (GameObject weaponSpot in Player_Main.instance.WeaponsSpots)
                    {
                        if (weaponSpot.transform.childCount == 0 && weaponSpot.GetComponent<WeaponControll>().HasChild == false)
                        {
                            weaponSpotAux = weaponSpot;
                            break;
                        }
                    }

                    // Se houver um local de arma vazio, ativa e configura a arma
                    if (weaponSpotAux != null)
                    {
                        weaponSpotAux.SetActive(true);
                        weaponSpotAux.GetComponent<WeaponControll>().WpType = WeaponType.Pistol;
                        //weaponSpotAux.GetComponent<WeaponControll>().SetWeapon();




                    }
                    else
                    {
                    }
                    DataManagment.instance.objectData.FoundPistol = true;
                    DataManagment.instance.SaveData();
                    powerUps["Pistol"] = DataManagment.instance.objectData.FoundPistol;
                }
                break;

            case "MiniGun":
                int miniGunCount = 0;
                MiniGun_Main firstMiniGun = null;
                WeaponControll secondMiniGunSpot = null;
                //int countMiniGuns = 0;
                if (Player_Main.instance.Money >= 0)
                {


                    GameObject weaponSpotAux = null;

                    for (int i = 0; i < Player_Main.instance.WeaponsSpots.Count; i++)
                    {
                        GameObject weaponSpot = Player_Main.instance.WeaponsSpots[i];

                        if (weaponSpot.transform.childCount == 1)
                        {
                            MiniGun_Main miniGun = weaponSpot.transform.GetChild(0).GetComponentInChildren<MiniGun_Main>();
                            if (miniGun != null && miniGun.lvlUpgrade < 1)
                            {
                                miniGunCount++;
                                if (firstMiniGun == null)
                                {
                                    firstMiniGun = miniGun;
                                }
                                else if (secondMiniGunSpot == null)
                                {
                                    secondMiniGunSpot = weaponSpot.GetComponent<WeaponControll>();
                                }
                            }
                        }
                        else if (weaponSpot.transform.childCount < 1)
                        {
                            if (weaponSpot.transform.childCount == 0 && weaponSpot.GetComponent<WeaponControll>().HasChild == false)
                            {
                                weaponSpotAux = weaponSpot;
                                break;
                            }
                        }
                    }

                    // Se houver um local de arma vazio, ativa e configura a arma
                    if (weaponSpotAux != null)
                    {

                        weaponSpotAux.SetActive(true);

                        weaponSpotAux.GetComponent<WeaponControll>().WpType = WeaponType.MiniGun;
                        //weaponSpotAux.GetComponent<WeaponControll>().SetWeapon();

                        
                    }
                    else
                    {
                    }

                    DataManagment.instance.objectData.FoundMinigun = true;
                    //DataManagment.instance.objectData.imagePistol = DataManagment.instance.SpriteToBase64(ImagesForButtons[2]);
                    DataManagment.instance.SaveData();
                    powerUps["MiniGun"] = DataManagment.instance.objectData.FoundMinigun;
                    // Se houver pelo menos dois spots com MiniGun_Main
                    //if (miniGunCount >= 1)
                    //{
                    //    // Faça upgrade no primeiro MiniGun_Main
                    //    firstMiniGun.lvlUpgrade++;
                    //    int indexSecondMiniGunSpot = -1;

                    //    // Destrua o segundo MiniGun_Main
                    //    if (secondMiniGunSpot != null)
                    //    {

                    //        MiniGun_Main secondMiniGun = secondMiniGunSpot.GetComponentInChildren<MiniGun_Main>();
                    //        if (secondMiniGun != null)
                    //        {
                    //            secondMiniGunSpot.WpType = WeaponType.None;
                    //            Destroy(secondMiniGunSpot.transform.GetChild(0).gameObject);
                    //            secondMiniGunSpot.gameObject.SetActive(false);
                    //            secondMiniGun = null;
                    //            firstMiniGun = null;
                    //            miniGunCount = 0;
                    //        }
                    //    }
                    //}
                    //else
                    //{


                    //}




                }
                break;
            case "JuiceLife": IncreaseLife(); DataManagment.instance.objectData.FoundJuiceLife = true;
                //DataManagment.instance.objectData.imagePistol = DataManagment.instance.SpriteToBase64(ImagesForButtons[0]);
                DataManagment.instance.SaveData();
                powerUps["JuiceLife"] = DataManagment.instance.objectData.FoundJuiceLife;

                break;
            case "JuiceReg": RegLife(); DataManagment.instance.objectData.FoundReg = true;
                //DataManagment.instance.objectData.imagePistol = DataManagment.instance.SpriteToBase64(ImagesForButtons[3]);
                DataManagment.instance.SaveData();
                powerUps["JuiceReg"] = DataManagment.instance.objectData.FoundReg;
                break;
            case "JuiceClub": 
                if (Player_Main.instance.Money >= 0)
                {


                    GameObject weaponSpotAux = null;

                    // Verifica se há um local de arma vazio
                    foreach (GameObject weaponSpot in Player_Main.instance.WeaponsSpots)
                    {
                        if (weaponSpot.transform.childCount == 0 && weaponSpot.GetComponent<WeaponControll>().HasChild == false)
                        {
                            weaponSpotAux = weaponSpot;
                            break;
                        }
                    }

                    // Se houver um local de arma vazio, ativa e configura a arma
                    if (weaponSpotAux != null)
                    {
                        weaponSpotAux.SetActive(true);

                        weaponSpotAux.GetComponent<WeaponControll>().WpType = WeaponType.Club;
                        //weaponSpotAux.GetComponent<WeaponControll>().SetWeapon();

                       
                    }
                    else
                    {
                    }

                    DataManagment.instance.objectData.FoundJuiceClub = true;
                    //DataManagment.instance.objectData.imagePistol = DataManagment.instance.SpriteToBase64(ImagesForButtons[4]);
                    DataManagment.instance.SaveData();
                    powerUps["JuiceClub"] = DataManagment.instance.objectData.FoundJuiceClub;
                }
                break;

               
            case "Boomerang":
                int BoomerangCount = 0;
                Boomerang_Main firstBoomerang = null;
                WeaponControll secondBoomerangSpot = null;
                //int countBoomerangs = 0;
                if (Player_Main.instance.Money >= 0)
                {


                    GameObject weaponSpotAux = null;

                    for (int i = 0; i < Player_Main.instance.WeaponsSpots.Count; i++)
                    {
                        GameObject weaponSpot = Player_Main.instance.WeaponsSpots[i];

                        if (weaponSpot.transform.childCount == 1)
                        {
                            Boomerang_Main Boomerang = weaponSpot.transform.GetChild(0).GetComponentInChildren<Boomerang_Main>();
                            if (Boomerang != null && Boomerang.lvlUpgrade < 1)
                            {
                                BoomerangCount++;
                                if (firstBoomerang == null)
                                {
                                    firstBoomerang = Boomerang;
                                }
                                else if (secondBoomerangSpot == null)
                                {
                                    secondBoomerangSpot = weaponSpot.GetComponent<WeaponControll>();
                                }
                            }
                        }
                        else if (weaponSpot.transform.childCount < 1)
                        {
                            if (weaponSpot.transform.childCount == 0)
                            {
                                weaponSpotAux = weaponSpot;
                                break;
                            }
                        }
                    }
                    // Se houver um local de arma vazio, ativa e configura a arma
                    if (weaponSpotAux != null)
                    {
                        weaponSpotAux.SetActive(true);

                        weaponSpotAux.GetComponent<WeaponControll>().WpType = WeaponType.Boomerang;

                        
                    }
                    else
                    {
                    }
                    DataManagment.instance.objectData.FoundBoomerang = true;
                    //DataManagment.instance.objectData.imagePistol = DataManagment.instance.SpriteToBase64(ImagesForButtons[5]);
                    DataManagment.instance.SaveData();
                    powerUps["Boomerang"] = DataManagment.instance.objectData.FoundBoomerang;


                    //// Se houver pelo menos dois spots com MiniGun_Main
                    //if (BoomerangCount >= 1)
                    //{
                    //    // Faça upgrade no primeiro MiniGun_Main
                    //    firstBoomerang.lvlUpgrade++;
                    //    int indexSecondBoomerangSpot = -1;

                    //    // Destrua o segundo MiniGun_Main
                    //    if (secondBoomerangSpot != null)
                    //    {

                    //        Boomerang_Main secondBoomerang = secondBoomerangSpot.GetComponentInChildren<Boomerang_Main>();
                    //        if (secondBoomerang != null)
                    //        {
                    //            secondBoomerangSpot.WpType = WeaponType.None;
                    //            Destroy(secondBoomerangSpot.transform.GetChild(0).gameObject);
                    //            secondBoomerangSpot.gameObject.SetActive(false);
                    //            secondBoomerang = null;
                    //            firstBoomerang = null;
                    //            BoomerangCount = 0;
                    //        }
                    //    }
                    //}
                    //else
                    //{


                    //}




                }
                break;

            case "CardWine":
             
                ManageCardEffects("RedWineCard");
                DataManagment.instance.objectData.FoundCardWine = true;
                //DataManagment.instance.objectData.imagePistol = DataManagment.instance.SpriteToBase64(ImagesForButtons[6]);
                DataManagment.instance.SaveData();

                powerUps["RedWineCard"] = DataManagment.instance.objectData.FoundCardWine;
                break;
            case "TequillaCard":

                ManageCardEffects("TequillaCard");
                DataManagment.instance.objectData.FoundCardWine = true;
                //DataManagment.instance.objectData.imagePistol = DataManagment.instance.SpriteToBase64(ImagesForButtons[7]);
                DataManagment.instance.SaveData();
                powerUps["TequillaCard"] = DataManagment.instance.objectData.FoundTequillaCard;
                break;

            case "JuiceHole":

                ManageCardEffects("JuiceHole");
                DataManagment.instance.objectData.FoundJuiceHoleCard = true;
                //DataManagment.instance.objectData.imagePistol = DataManagment.instance.SpriteToBase64(ImagesForButtons[7]);
                DataManagment.instance.SaveData();
                powerUps["JuiceHole"] = DataManagment.instance.objectData.FoundJuiceHoleCard;
                break;
        }
        PlayerItemOnUiManager.instance.ManageItemsOfPLayer();


    }

    private void ManageCardEffects(string CardPower)
    {
            if (CardPower == "RedWineCard" )
            {
                AtributteCardPowerImagesAndStats(6);
            }
            if (CardPower == "TequillaCard")
            {
                AtributteCardPowerImagesAndStats(7);
            }

        if (CardPower == "JuiceHole")
        {
            AtributteCardPowerImagesAndStats(8);
        }
    }


    private void AtributteCardPowerImagesAndStats(int IndexCardForImageOnListImages)
    {
        foreach (CardSpot o in CardSpotsManager.instance.cardSpots)
        {
            if (o.GetComponent<CardSpot>().HasCard == false && !o.GetComponent<CardSpot>().IsCardSpotStucked)
            {
                if (IndexCardForImageOnListImages == 6 && !Player_Stats.instance.CardsActive.Contains(CardType.RedWine))
                {
                    o.GetComponent<CardSpot>().CardType = CardType.RedWine;
                    o.InstantiateCard();

                }
                else if (IndexCardForImageOnListImages == 7 && !Player_Stats.instance.CardsActive.Contains(CardType.Tequilla))
                {
                    o.GetComponent<CardSpot>().CardType = CardType.Tequilla;
                    o.InstantiateCard();
                }

                else if (IndexCardForImageOnListImages == 8 && !Player_Stats.instance.CardsActive.Contains(CardType.JuiceHole))
                {
                    o.GetComponent<CardSpot>().CardType = CardType.JuiceHole;
                    o.InstantiateCard();
                }
                break; // Para a iteração assim que encontrar um CardSpot sem card e atribuir o tipo de card.
            }

        }
    }

   

    List<string> ListCards = new List<string>();
}
