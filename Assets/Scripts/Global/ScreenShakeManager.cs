using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShakeManager : MonoBehaviour
{

  static ScreenShakeManager instance;

  void Awake()
  {
    if (instance != null && instance != this)
    {
      Destroy(gameObject);
      return;
    }
    else
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }

  CinemachineVirtualCamera virtualCamera;
  void Start()
  {
    virtualCamera = GetComponent<CinemachineVirtualCamera>();
  }

  void StartShaking(float amplitude, float frequency)
  {
    CinemachineBasicMultiChannelPerlin noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    noise.m_AmplitudeGain = amplitude;
    noise.m_FrequencyGain = frequency;
  }

  void StopShaking()
  {
    CinemachineBasicMultiChannelPerlin noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    noise.m_AmplitudeGain = 0;
    noise.m_FrequencyGain = 0;
  }
}
