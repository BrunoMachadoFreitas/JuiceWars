using Game.Sounds.SoundScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GamePlay.Scripts
{
    public class AchievementsMenu : MonoBehaviour
    {

        [SerializeField] MainMenu mainMenu;
        public void startGame()
        {
        

        }

        public void HideAchievementsShowMainMenu()
        {
            mainMenu.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            SoundManager.instance.PlaySound(12);
        }

    }
}
