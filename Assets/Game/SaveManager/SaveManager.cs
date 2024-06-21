using System.IO;
using UnityEngine;

namespace Game.SaveManager
{
    public abstract class SaveManager
    {
        static string filePath;
        public static DataInfo objectData;

        public static void SaveData()
        {
            string json = JsonUtility.ToJson(objectData);

            File.WriteAllText(filePath, json);
        }

        public static DataInfo LoadData()
        {
            filePath = Application.persistentDataPath + "/dados.json";

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                DataInfo data = JsonUtility.FromJson<DataInfo>(json);
                return data;
            }
            else
            {
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
        public float SoundMusicValue;


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
        public bool FoundBreakCardSpot;
        public bool FoundJuiceCollectCard;

        public bool CanPlayGameSounds;
        public bool CanPlayGameMusic;


        public string imageTequillaCard;


    }
}