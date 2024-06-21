using Game.Sounds.SoundScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GamePlay.Scripts
{
    public class MainMenu : MonoBehaviour
    {

        [SerializeField] SettingsPopUp MenuSettings;
        [SerializeField] GameObject canvasAchievements;
        public void startGame()
        {
            // Registra o callback para quando a cena for carregada
            //SceneManager.sceneLoaded += OnSceneLoaded;
            if (SoundManager.instance.MainMusicX.isPlaying)
            {

            }




            //Carrega a cena pelo �ndice(ou pelo nome)
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            //SceneManager.SetActiveScene(sceneJuice);

            //MainMenuX.gameObject.SetActive(false);
            SoundManager.instance.PlaySound(12);


        }

        public void HideMainMenuShowSettings()
        {
            MenuSettings.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            SoundManager.instance.PlaySound(12);
        }

        public void HideMainMenuShowAchievements()
        {
            canvasAchievements.SetActive(true);
            this.gameObject.SetActive(false);
            SoundManager.instance.PlaySound(12);
        }
    }
}
