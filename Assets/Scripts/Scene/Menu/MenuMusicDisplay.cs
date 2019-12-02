using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Menu
{
    //Control the music
    public class MenuMusicDisplay : MonoBehaviour
    {
        private List<AudioSource> music = new List<AudioSource>();
        
        public void Start()
        {
            AudioSource[] audioSourceArray = GameObject.FindWithTag("gamedata").GetComponentsInChildren<AudioSource>();
            music.Add(audioSourceArray[0]);

            Slider musicSlider = this.GetComponent<Slider>();

            if (PlayerPrefs.HasKey("musicvolume"))
            {
                musicSlider.value = PlayerPrefs.GetFloat("musicvolume");
                UpdateMusicVolume(musicSlider.value);
            }
            else
            {
                musicSlider.value = 1;
                UpdateMusicVolume(1);
            }
        }

        // Update is called once per frame
        public void UpdateMusicVolume(float value)
        {
            PlayerPrefs.SetFloat("musicvolume", value);
            foreach (AudioSource eachMusic in music)
            {
                eachMusic.volume = value;
            }
        }
    }
}
