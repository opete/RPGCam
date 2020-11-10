using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MicrophoneInput : MonoBehaviour
{
    private AudioSource audioSource;
    private Renderer rend;

    public float currentVolume;
    private float prevVolume;

    public float volumeTreshold = 10f;

    private int timeBuffer = 10;
    private TextureHandler textureHandler;
    private LoadAssets loadAssets;

    IEnumerator CoRecordingRestarter()
    {
        while (true)
        {
            if (Microphone.devices.Length > 0)
            {
                audioSource.clip = Microphone.Start(null, true, timeBuffer, AudioSettings.outputSampleRate);
                while (!(Microphone.GetPosition(null) > 0)) { }
                audioSource.Play();
            }
            yield return new WaitForSeconds(timeBuffer);
        }
    }


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();

        textureHandler = FindObjectOfType<TextureHandler>();
        loadAssets = FindObjectOfType<LoadAssets>();

        StartCoroutine(CoRecordingRestarter());
    }

    void Update()
    {
        if (audioSource.clip != null)
        {
            float[] spectrum = new float[64];
            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
            currentVolume = spectrum[32] * 100000f;

            if (!loadAssets.IsMuted)
            {
                if (currentVolume > volumeTreshold)
                {
                    textureHandler.State = State.talk;
                }
            }

            prevVolume = currentVolume;
        }
    }
}
