using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

  public static AudioManager instance;
  Dictionary<string, AudioClip> audioSources;
  [SerializeField] List<AudioClip> audioClips;
  [SerializeField] List<string> audioNames;

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(gameObject);
      return;
    }
  }

  void Start()
  {
    audioSources = new Dictionary<string, AudioClip>();
    if (audioClips.Count != audioNames.Count)
    {
      Debug.LogError("AudioManager: audioClips and audioNames are not the same size");
    }
    for (int i = 0; i < audioClips.Count; i++)
    {
      audioSources.Add(audioNames[i], audioClips[i]);
      // Debug.Log(audioNames[i]);
    }
    
  }

  public void playSound(string name, float volume = 1.0f)
  {
    AudioSource.PlayClipAtPoint(audioSources[name], Camera.main.transform.position, volume);
    Debug.Log("play sound" + name);
  }

}
