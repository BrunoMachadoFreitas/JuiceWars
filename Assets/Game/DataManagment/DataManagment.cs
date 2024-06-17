using UnityEngine;
using System.IO;

public class DataManagment : MonoBehaviour
{
    public static DataManagment instance;
    private string caminhoArquivo;
    public DataInfo objectData;
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
        // Definir o caminho do arquivo no armazenamento persistente do dispositivo
        caminhoArquivo = Application.persistentDataPath + "/dados.json";

        DontDestroyOnLoad(gameObject);
    }

    public void SaveData()
    {
        // Converter o objeto em uma string JSON
        string json = JsonUtility.ToJson(objectData);
        // Escrever a string JSON no arquivo
        File.WriteAllText(caminhoArquivo, json);
    }

    public DataInfo LoadData()
    {
        // Verificar se o arquivo existe
        if (File.Exists(caminhoArquivo))
        {
            // Ler a string JSON do arquivo
            string json = File.ReadAllText(caminhoArquivo);
            // Converter a string JSON de volta para um objeto
            DataInfo dados = JsonUtility.FromJson<DataInfo>(json);
            return dados;
        }
        else
        {
            // Se o arquivo não existir, retornar um novo objeto com valores padrão
            return new DataInfo();
        }
    }
    //public string SpriteToBase64(Sprite sprite)
    //{
    //    if (sprite == null)
    //    {
    //        return null;
    //    }

    //    Texture2D texture = sprite.texture;
    //    byte[] imageData = texture.EncodeToPNG(); // ou EncodeToJPG();
    //    return System.Convert.ToBase64String(imageData);
    //}


    //public Sprite Base64ToSprite(string base64)
    //{
    //    if (string.IsNullOrEmpty(base64))
    //    {
    //        return null;
    //    }

    //    byte[] imageData = System.Convert.FromBase64String(base64);
    //    Texture2D texture = new Texture2D(2, 2);
    //    texture.LoadImage(imageData);
    //    Rect rect = new Rect(0, 0, texture.width, texture.height);
    //    return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    //}

}
[System.Serializable]
public class DataInfo
{
    public int nivel;
    public float pontos;
    public string nome;

    public float SoundValue;


    //Items
    public bool FoundMinigun;
    public string imageMinigun;

    public bool FoundPistol;
    public string imagePistol;

    public bool FoundReg;
    public string imageReg;

    public bool FoundJuiceLife;
    public string imageJuiceLife;

    public bool FoundJuiceClub;
    public string imageJuiceClub;

    public bool FoundBoomerang;
    public string imageBoomerang;

    public bool FoundCardWine;
    public string imageCardWine;

    public bool FoundTequillaCard;
    public bool FoundJuiceHoleCard;
    public string imageTequillaCard;

    
}