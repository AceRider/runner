using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class UpdateSound : MonoBehaviour
    {
        List<AudioSource> soundEffect = new List<AudioSource>();
        // Start is called before the first frame update
        public void Start()
        {
            AudioSource[] allAS = GameObject.FindWithTag("gamedata").GetComponentsInChildren<AudioSource>();
            for (int i = 1; i < allAS.Length; i++)
                soundEffect.Add(allAS[i]);

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
