using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class cinemaChinecameraShake : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cineMachineVirtualCamera;
    private float timeCount;
    private void Start()
    {

    }

    public IEnumerator shake(float amplitudeGain, float frequeancyGain)
    {
        cineMachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;//5
        cineMachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequeancyGain;//.2
        yield return new WaitForSeconds(.2f);
        cineMachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        cineMachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
    }

    public void ShakeCamera(float amplG, float freqG)
    {
        StartCoroutine(shake(amplG, freqG));
    }
}
