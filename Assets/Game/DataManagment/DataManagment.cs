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
    public Sprite imageMinigun;

    public bool FoundPistol;
    public Sprite imagePistol;

    public bool FoundReg;
    public Sprite imageReg;

    public bool FoundJuiceLife;
    public Sprite imageJuiceLife;

    public bool FoundJuiceClub;
    public Sprite imageJuiceClub;

    public bool FoundBoomerang;
    public Sprite imageBoomerang;

    public bool FoundCardWine;
    public Sprite imageCardWine;

    public bool FoundTequillaCard;
    public Sprite imageTequillaCard;

    
}