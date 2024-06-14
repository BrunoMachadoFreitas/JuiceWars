using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Acievements_Manager : MonoBehaviour
{
    public GameObject itemPrefab; // Prefab genérico para os itens
    public Transform parentTransform; // Parent para os itens instanciados
    public int itemsPerRow = 3; // Quantidade de itens por linha
    public float itemSpacing = 1.0f; // Espaçamento entre itens
    public Vector2 startOffset = new Vector2(0, 0); // Deslocamento inicial para a primeira posição
    DataInfo objectData = new DataInfo();
    void Start()
    {
        objectData = DataManagment.instance.LoadData();
        CheckAndInstantiateItems();
    }

    void CheckAndInstantiateItems()
    {
        List<Sprite> foundItems = new List<Sprite>();

        if (objectData.FoundMinigun)
        {
            foundItems.Add(DataManagment.instance.Base64ToSprite(objectData.imageMinigun));
        }

        if (objectData.FoundPistol)
        {
            foundItems.Add(DataManagment.instance.Base64ToSprite(objectData.imagePistol));
        }

        if (objectData.FoundReg)
        {
            foundItems.Add(DataManagment.instance.Base64ToSprite(objectData.imageReg));
        }

        if (objectData.FoundJuiceLife)
        {
            foundItems.Add(DataManagment.instance.Base64ToSprite(objectData.imageJuiceLife));
        }

        if (objectData.FoundJuiceClub)
        {
            foundItems.Add(DataManagment.instance.Base64ToSprite(objectData.imageJuiceClub));
        }

        if (objectData.FoundBoomerang)
        {
            foundItems.Add(DataManagment.instance.Base64ToSprite(objectData.imageBoomerang));
        }

        if (objectData.FoundCardWine)
        {
            foundItems.Add(DataManagment.instance.Base64ToSprite(objectData.imageCardWine));
        }

        if (objectData.FoundTequillaCard)
        {
            foundItems.Add(DataManagment.instance.Base64ToSprite(objectData.imageTequillaCard));
        }

        for (int i = 0; i < foundItems.Count; i++)
        {
            int row = i / itemsPerRow;
            int col = i % itemsPerRow;

            Vector2 position = new Vector2(col * itemSpacing, -row * itemSpacing) + startOffset;
            InstantiateItem(foundItems[i], position);
        }
    }

    void InstantiateItem(Sprite itemSprite, Vector2 position)
    {
        GameObject newItem = Instantiate(itemPrefab, parentTransform);
        newItem.GetComponent<Image>().sprite = itemSprite;
        newItem.transform.localPosition = position;
    }
}
