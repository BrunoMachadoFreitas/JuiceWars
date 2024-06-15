using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public List<AudioSource> GameSounds;
    public List<AudioSource> GameSoundsInGame;

    public float Volume1;
    public float Volume2;
    public float Volume3;
    public float Volume4;
    public float Volume5;
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
    }



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameSounds.Count; i++)
        {
           var AudioSource = Instantiate(GameSounds[i]);
            GameSoundsInGame.Add(AudioSource);
            GameSounds[i].volume = 1f;
        }
        if (ControlMenu.instance)
        {
            ChangeVolume(ControlMenu.instance.AudioVolumeSoundsGeneral);
        }
        else
        {
            ChangeVolume(.3f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeVolume(float volume)
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
        GameSoundsInGame[soundIndex].PlayOneShot(GameSoundsInGame[soundIndex].clip);
    }
}
