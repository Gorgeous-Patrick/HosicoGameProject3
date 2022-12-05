using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class HasScreenshakeScript : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private float timer;
    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        EventBus.Subscribe<OnCollapseScreenShakeEvent>(OnCollapseDo);
    }

    private void OnCollapseDo(OnCollapseScreenShakeEvent obj)
    {
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = obj.intensity;
        timer = obj.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0f;
            }
        }
    }

    public void StartShaking(float intensity, float frequency)
    {
        Debug.Log("StartShaking");
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        perlin.m_FrequencyGain = frequency;
    }

    public void StopShaking()
    {
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 0f;
        perlin.m_FrequencyGain = 0f;
    }

    IEnumerator Shaking(float intensity, float frequency, float time)
    {
        yield return new WaitForSeconds(5f);
        StartShaking(intensity, frequency);
        yield return new WaitForSeconds(time);
        StopShaking();
    }

    public void ShakingCoroutine(float intensity = 3, float frequency = 5, float time = 0.5f)
    {
        StartCoroutine(Shaking(intensity, frequency, time));
    }

    void Start()
    {
        ShakingCoroutine();
    }
}
