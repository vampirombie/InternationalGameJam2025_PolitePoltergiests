using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioScript", menuName = "ScriptableObjects/Audio/Soundscriptableobjects", order = 2)]
public class Soundscriptableobjects : ScriptableObject
{
    [SerializeField] private AudioClip myAudio;
    [SerializeField] private AudioMixerGroup myGroup;

    private AudioSource sharedAudioSource;

    public void CreateOrPlaySound()
    {
        if (sharedAudioSource == null)
        {
            GameObject audioGameObject = new GameObject("SharedAudio_" + myAudio.name);
            sharedAudioSource = audioGameObject.AddComponent<AudioSource>();
            sharedAudioSource.outputAudioMixerGroup = myGroup;

            Object.DontDestroyOnLoad(audioGameObject);
        }

        sharedAudioSource.PlayOneShot(myAudio);
    }
}
