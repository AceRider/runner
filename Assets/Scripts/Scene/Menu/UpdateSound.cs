using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Menu
{
    //Control the sound effect
    public class UpdateSound : MonoBehaviour
    {
        private List<AudioSource> soundEffect = new List<AudioSource>();
        
        public void Start()
        {
            AudioSource[] audioSourceArray = GameObject.FindWithTag("gamedata").GetComponentsInChildren<AudioSource>();

            for (int i = 1; i < audioSourceArray.Length; i++)
                soundEffect.Add(audioSourceArray[i]);

            Slider soundEffectSlider = this.GetComponent<Slider>();

            if (PlayerPrefs.HasKey("sfxvolume"))
            {
                soundEffectSlider.value = PlayerPrefs.GetFloat("sfxvolume");
                UpdateSoundVolume(soundEffectSlider.value);
            }
            else
            {
                soundEffectSlider.value = 1;
                UpdateSoundVolume(1);
            }
        }

        // Update is called once per frame
        public void UpdateSoundVolume(float value)
        {
            PlayerPrefs.SetFloat("sfxvolume", value);
            foreach (AudioSource sound in soundEffect)
            {
                sound.volume = value;
            }
        }
    }
}
