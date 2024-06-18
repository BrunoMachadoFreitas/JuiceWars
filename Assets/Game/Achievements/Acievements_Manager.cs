using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Acievements_Manager : MonoBehaviour
{
    public GameObject itemPrefab; // Prefab genérico para os itens
    public Transform parentTransform; // Parent para os itens instanciados
    public int itemsPerRow = 3; // Quantidade de itens por linha
    public float itemSpacing = 100.0f; // Espaçamento entre itens
    public Vector2 startOffset = new Vector2(0, 0); // Deslocamento inicial para a primeira posição

    public List<Sprite> spritesItemsSaved = new List<Sprite>();
    void Start()
    {
        CheckAndInstantiateItems();
    }

    void CheckAndInstantiateItems()
    {
        // Lista para armazenar os sprites encontrados
        List<Sprite> foundItems = new List<Sprite>();

        // Verifica quais itens foram encontrados e adiciona à lista
        if (ControlMenu.instance.objectData.FoundMinigun)
        {
            foundItems.Add(spritesItemsSaved[2]);
        }

        if (ControlMenu.instance.objectData.FoundPistol)
        {
            foundItems.Add(spritesItemsSaved[1]);
        }

        if (ControlMenu.instance.objectData.FoundReg)
        {
            foundItems.Add(spritesItemsSaved[3]);
        }

        if (ControlMenu.instance.objectData.FoundJuiceLife)
        {
            foundItems.Add(spritesItemsSaved[0]);
        }

        if (ControlMenu.instance.objectData.FoundJuiceClub)
        {
            foundItems.Add(spritesItemsSaved[4]);
        }

        if (ControlMenu.instance.objectData.FoundBoomerang)
        {
            foundItems.Add(spritesItemsSaved[5]);
        }

        if (ControlMenu.instance.objectData.FoundCardWine)
        {
            foundItems.Add(spritesItemsSaved[6]);
        }

        if (ControlMenu.instance.objectData.FoundTequillaCard)
        {
            foundItems.Add(spritesItemsSaved[7]);
        }

        // Define o número de itens por linha e o espaçamento entre os itens
        int itemsPerRow = 3;
        float itemSpacing = 100f; // Ajuste esse valor conforme necessário
        Vector2 startOffset = new Vector2(0, 0); // Ajuste conforme necessário

        // Loop para instanciar e posicionar os itens
        for (int i = 0; i < foundItems.Count; i++)
        {
            // Calcula a linha e a coluna do item atual
            int row = i / itemsPerRow;
            int col = i % itemsPerRow;

            // Calcula a posição do item no grid
            Vector2 position = new Vector2(col * itemSpacing, -row * itemSpacing) + startOffset;

            // Instancia o item na posição calculada
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
