// --- ARCHIVO: ChannelManager.cs (ACTUALIZADO CON PLAYERPREFS) ---

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ChannelManager", menuName = "ScriptableObjects/Audio/ChannelManager", order = 1)]
public class ChannelManager : ScriptableObject
{
    [SerializeField] string channelVolume; 
    public float currentVolume;
    [SerializeField] private AudioMixer myMixer;
    public Sprite[] buttonMute;
    public bool isMuted;

    private string volumeKey;
    private string muteKey;

    public void Initialize()
    {
        volumeKey = channelVolume + "_Volume";
        muteKey = channelVolume + "_Muted";

        if (PlayerPrefs.HasKey(volumeKey))
        {
            currentVolume = PlayerPrefs.GetFloat(volumeKey);
            isMuted = PlayerPrefs.GetInt(muteKey) == 1;
        }
        else
        {
            PlayerPrefs.SetFloat(volumeKey, currentVolume);
            PlayerPrefs.SetInt(muteKey, isMuted ? 1 : 0);
        }
    }

    public void UpdateVolume(float value)
    {
        currentVolume = value;
        if (!isMuted)
        {
            myMixer.SetFloat(channelVolume, Mathf.Log10(currentVolume) * 20f);
        }

        PlayerPrefs.SetFloat(volumeKey, currentVolume);
    }

    public void ApplyStoredVolume()
    {
        if (isMuted)
        {
            myMixer.SetFloat(channelVolume, -80f);
        }
        else
        {
            myMixer.SetFloat(channelVolume, Mathf.Log10(currentVolume) * 20f);
        }
    }

    public void muteAudio(Button botton)
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            myMixer.SetFloat(channelVolume, -80f);
            botton.image.sprite = buttonMute[0];
        }
        else
        {
            myMixer.SetFloat(channelVolume, Mathf.Log10(currentVolume) * 20f);
            botton.image.sprite = buttonMute[1];
        }

        PlayerPrefs.SetInt(muteKey, isMuted ? 1 : 0);
    }
}