using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettingsController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] Button[] muteButton;
    [SerializeField] Slider[] sliderButton;
    [SerializeField] ChannelManager[] SOChanelManager;

    void Awake()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        for (int i = 0; i < SOChanelManager.Length; i++)
        {
            SOChanelManager[i].Initialize();
        }

        SOAudio();

    }

    public void SOAudio()
    {
        for (int i = 0; i < SOChanelManager.Length; i++)
        {
            SOChanelManager[i].Initialize();

            SOChanelManager[i].ApplyStoredVolume();

            sliderButton[i].SetValueWithoutNotify(SOChanelManager[i].currentVolume);

            muteButton[i].image.sprite = SOChanelManager[i].isMuted
                ? SOChanelManager[i].buttonMute[0]
                : SOChanelManager[i].buttonMute[1];
        }
    }
}
