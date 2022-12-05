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
        StartCoroutine(test());
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
    IEnumerator Shaking(float intensity, float frequency, float time)
    {
        StartShaking(intensity, frequency);
        yield return new WaitForSeconds(time);
        StopShaking();
    }

    public void ShakingCoroutine(float intensity = 3, float frequency = 5, float time = 0.5f)
    {
        StartCoroutine(Shaking(intensity, frequency, time));
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Shaking");
        ShakingCoroutine();
    }
}
