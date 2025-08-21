using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // arraste o mixer aqui
    [SerializeField] private Slider volumeSlider;

    private const string VOLUME_KEY = "MasterVolume";

    void Start()
    {
        // Carrega o valor salvo, ou usa 0 (volume cheio)
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 0f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        // Adiciona listener
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(VOLUME_KEY, volume);
        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
    }
}
