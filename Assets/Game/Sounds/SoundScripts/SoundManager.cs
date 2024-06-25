using System.Collections.Generic;
using UnityEngine;

namespace Game.Sounds.SoundScripts
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        public List<AudioSource> GameSounds;
        public List<AudioSource> GameSoundsInGame;

        public AudioSource MainMusic;
        public AudioSource MainMusicX;
        public AudioSource MainMusicEnd;
        public AudioSource MainMusicEndX;

        public float Volume1;
        public float Volume2;
        public float Volume3;
        public float Volume4;
        public float Volume5;

        public bool canPlayGameSounds = true;
        public bool canPlayGameMusic = true;
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
            DontDestroyOnLoad(gameObject);
            MainMusicX = Instantiate(MainMusic);
            MainMusicEndX = Instantiate(MainMusicEnd);
        
        }



        // Start is called before the first frame update
        void Start()
        {
            canPlayGameMusic = ControlMenu.instance.objectData.CanPlayGameMusic;
            canPlayGameSounds = ControlMenu.instance.objectData.CanPlayGameSounds;

            if (canPlayGameMusic)
            {
                MainMusicX.Play();
            }
            for (int i = 0; i < GameSounds.Count; i++)
            {
                var AudioSource = Instantiate(GameSounds[i]);
                GameSoundsInGame.Add(AudioSource);
                GameSounds[i].volume = 1f;
                GameSoundsInGame[i].volume = ControlMenu.instance.objectData.SoundValue;
            }
            //if (ControlMenu.instance)
            //{
            //    ChangeVolume(ControlMenu.instance.objectData.SoundValue);
            //}
            //else
            //{
            //    ChangeVolume(.3f);
            //}
        
        }
        
        void ChangeVolume(float volume)
        {
            for (int i = 0; i < GameSounds.Count; i++)
            {
                GameSoundsInGame[i].volume -= volume;
            }

            Volume1 = GameSoundsInGame[0].volume;
            Volume2 = GameSoundsInGame[1].volume;
            Volume3 = GameSoundsInGame[2].volume;
            Volume4 = GameSoundsInGame[3].volume;
            Volume5 = GameSoundsInGame[4].volume;
        }

        public void PlaySound(int soundIndex)
        {
            if(canPlayGameSounds)
                GameSoundsInGame[soundIndex].PlayOneShot(GameSoundsInGame[soundIndex].clip);
        }
    }
}
