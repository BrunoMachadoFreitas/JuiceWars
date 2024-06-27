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
        public int Money;

        public float SoundValue;
        public float SoundMusicValue;


        //Items
        public bool FoundMinigun;

        public bool FoundPistol;

        public bool FoundReg;

        public bool FoundJuiceLife;

        public bool FoundJuiceClub;

        public bool FoundBoomerang;

        public bool FoundCardWine;

        public bool FoundTequillaCard;
        public bool FoundJuiceHoleCard;
        public bool FoundGreenTeaCard;
        public bool FoundCoffeeCard;
        public bool FoundBreakCardSpot;
        public bool FoundJuiceCollectCard;
        public bool FoundTonicOfThePhoenixCard;
        public bool FoundJuiceEmitterCard;

        public bool CanPlayGameSounds;
        public bool CanPlayGameMusic;

        //Display Settings
        public bool ShowShakeCamera;


    }
}