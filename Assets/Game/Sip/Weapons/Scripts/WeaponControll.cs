using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class WeaponControll : MonoBehaviour
{

    [SerializeField] public WeaponType WpType;


    [SerializeField] private GameObject PistolPrefab;
    [SerializeField] private GameObject DaggerPrefab;
    [SerializeField] private GameObject MiniGunPrefab;
    [SerializeField] private GameObject ShotgunPrefab;
    [SerializeField] private GameObject RocketPrefab;
    [SerializeField] private GameObject BoomerangPrefab;
    [SerializeField] private GameObject SwordPrefab;
    [SerializeField] private GameObject ClubPrefab;
    [SerializeField] private GameObject CactusPrefab;
    [SerializeField] private GameObject BallRotatePrefab;

    
    GameObject Dagger;
    GameObject Minigun;
    GameObject ShotGun;
    GameObject Rocket;
    GameObject Boomerang;
    GameObject Sword;
    GameObject Club;
    GameObject Cactus;
    GameObject BallRotate;



    public bool HasChild = false;

   
    void Awake()
    {
        

    }

    // Start is called before the first frame update
    void Start()
    {
        switch (WpType)
        {

            case WeaponType.Pistol:
                GameObject Pistol = Instantiate(PistolPrefab, this.transform.position, Quaternion.identity);
                Pistol.transform.SetParent(this.transform);
                Pistol.transform.localScale = new Vector3(1f, 1f, 0f);
                Player_Main.instance.Weapons.Add(PistolPrefab);
                HasChild = true;
                break;


            case WeaponType.Dagger:
                Dagger = Instantiate(DaggerPrefab, this.transform.position, Quaternion.identity);
                Dagger.transform.SetParent(this.transform);
                //Dagger.transform.localScale = new Vector3(15f, -15f, 0f);
                Player_Main.instance.Weapons.Add(DaggerPrefab);
                HasChild = true;
                break;

            case WeaponType.MiniGun:
                Minigun = Instantiate(MiniGunPrefab, this.transform.position, Quaternion.identity);
                Minigun.transform.SetParent(this.transform);
                Minigun.transform.localScale = new Vector3(7f, 15f, 0f);
                Player_Main.instance.Weapons.Add(MiniGunPrefab);
                HasChild = true;
                break;

            case WeaponType.Shotgun:
                ShotGun = Instantiate(ShotgunPrefab, this.transform.position, Quaternion.identity);
                ShotGun.transform.SetParent(this.transform);
                ShotGun.transform.localScale = new Vector3(1f, 1f, 0f);
                Player_Main.instance.Weapons.Add(ShotgunPrefab);
                HasChild = true;
                break;

            case WeaponType.Rocket:
                Rocket = Instantiate(RocketPrefab, this.transform.position, Quaternion.identity);
                Rocket.transform.SetParent(this.transform);
                Rocket.transform.localScale = Vector3.one;
                Player_Main.instance.Weapons.Add(RocketPrefab);
                HasChild = true;
                break;

            case WeaponType.Boomerang:
                Boomerang = Instantiate(BoomerangPrefab, this.transform.position, Quaternion.identity);
                Boomerang.transform.SetParent(this.transform);
                Boomerang.transform.localScale = new Vector3(12f, 15f, 0f);
                Player_Main.instance.Weapons.Add(BoomerangPrefab);
                HasChild = true;
                break;

            case WeaponType.Sword:
                Sword = Instantiate(SwordPrefab, this.transform.position, Quaternion.identity);
                Sword.transform.SetParent(this.transform);
                Player_Main.instance.Weapons.Add(Sword);
                HasChild = true;
                break;

            case WeaponType.Club:
                Club = Instantiate(ClubPrefab, this.transform.position, Quaternion.identity);
                Club.transform.SetParent(this.transform);
                Player_Main.instance.Weapons.Add(ClubPrefab);
                HasChild = true;
                break;

            case WeaponType.Cactos:
                Cactus = Instantiate(CactusPrefab, this.transform.position, Quaternion.identity);
                Cactus.transform.SetParent(this.transform);
                Player_Main.instance.Weapons.Add(CactusPrefab);
                HasChild = true;
                break;

            case WeaponType.BallRotate:
                BallRotate = Instantiate(BallRotatePrefab, this.transform.position, Quaternion.identity);
                
                Player_Main.instance.Weapons.Add(BallRotatePrefab);
                HasChild = true;
                break;
               
        }
        Debug.LogWarning("From Start");
        PlayerItemOnUiManager.instance.ManageItemsOfPLayer();
    }
    
    void FixedUpdate()
    {
        
    }

    public void SetWeapon()
    {
        switch (WpType)
        {
            case WeaponType.Pistol:
                
                GameObject Pistol = Instantiate(PistolPrefab, this.transform.position, Quaternion.identity);
                Pistol.transform.SetParent(this.transform);
                Pistol.transform.localScale = new Vector3(1f, 1f, 0f);
                Player_Main.instance.Weapons.Add(PistolPrefab);
                
                break;


            case WeaponType.Dagger:
                Dagger = Instantiate(DaggerPrefab, this.transform.position, Quaternion.identity);
                Dagger.transform.SetParent(this.transform);
                Dagger.transform.localScale = new Vector3(15f, -15f, 0f);
                Player_Main.instance.Weapons.Add(DaggerPrefab);
                break;

            case WeaponType.MiniGun:
                Minigun = Instantiate(MiniGunPrefab, this.transform.position, Quaternion.identity);
                Minigun.transform.SetParent(this.transform);
                Minigun.transform.localScale = new Vector3(7f, 15f, 0f);
                Player_Main.instance.Weapons.Add(MiniGunPrefab);
                break;

            case WeaponType.Shotgun:
                ShotGun = Instantiate(ShotgunPrefab, this.transform.position, Quaternion.identity);
                ShotGun.transform.SetParent(this.transform);
                ShotGun.transform.localScale = new Vector3(1f, 1f, 0f);
                Player_Main.instance.Weapons.Add(ShotgunPrefab);
                break;

            case WeaponType.Rocket:
                Rocket = Instantiate(RocketPrefab, this.transform.position, Quaternion.identity);
                Rocket.transform.SetParent(this.transform);
                Rocket.transform.localScale = Vector3.one;
                Player_Main.instance.Weapons.Add(RocketPrefab);
                break;

            case WeaponType.Boomerang:
                Boomerang = Instantiate(BoomerangPrefab, this.transform.position, Quaternion.identity);
                Boomerang.transform.SetParent(this.transform);
                Boomerang.transform.localScale = new Vector3(12f, 15f, 0f);
                Player_Main.instance.Weapons.Add(BoomerangPrefab);
                break;

            case WeaponType.Sword:
                Sword = Instantiate(SwordPrefab, this.transform.position, Quaternion.identity);
                Sword.transform.SetParent(this.transform);
                Player_Main.instance.Weapons.Add(Sword);
                break;

            case WeaponType.Club:
                Club = Instantiate(ClubPrefab, this.transform.position, Quaternion.identity);
                Club.transform.SetParent(this.transform);
                Player_Main.instance.Weapons.Add(ClubPrefab);
                break;

            case WeaponType.Cactos:
                Cactus = Instantiate(CactusPrefab, this.transform.position, Quaternion.identity);
                Cactus.transform.SetParent(this.transform);
                Player_Main.instance.Weapons.Add(CactusPrefab);
                break;
            case WeaponType.BallRotate:
                BallRotate = Instantiate(BallRotatePrefab, this.transform.position, Quaternion.identity);
                
                Player_Main.instance.Weapons.Add(BallRotatePrefab);
                break;

        }
        Debug.LogWarning("From SetWeapon");
        foreach (GameObject weaponSpot in Player_Main.instance.WeaponsSpots)
        {
            if (weaponSpot.transform.childCount > 1)
            {

                Destroy(weaponSpot.transform.GetChild(1).gameObject);
                break;
            }
        }
    }
}
