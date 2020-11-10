using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    initial,
    idle,
    blink,
    angry,
    attack,
    talk
}


public class TextureHandler : MonoBehaviour
{
    public NPC npc;

    float blinkIntervalMin = 1f;
    float blinkIntervalMax = 5;
    float blinkTime = 0.1f;

    bool isBlinking;

    public bool readyToChangeFrame;
    private LoadAssets loadAssets;

    private int currentFrame = -1;

    Renderer rend;
    MicrophoneInput microphoneInput;

    public State state = State.idle;
    public State State
    {
        get { return state; }
        set
        {
            if (value != state)
            {
                prevState = state;
                state = value;

                if (npc != null)
                {
                    switch (state)
                    {
                        case State.idle:
                            currentFrame = 0;
                            rend.material.mainTexture = npc.textures[currentFrame];
                            break;
                        case State.blink:
                            if (npc.textures[1] != null)
                            {
                                currentFrame = 1;
                                rend.material.mainTexture = npc.textures[currentFrame];
                            }
                            break;
                        case State.talk:
                            loadAssets.objTalkIndicator.SetActive(true);
                            break;
                        case State.attack:
                            break;
                    }
                    //Debug.Log(State);
                }
            }
        }
    }

    private State prevState;

    IEnumerator CoBlink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(blinkIntervalMin, blinkIntervalMax));
            State = State.blink;
            yield return new WaitForSeconds(blinkTime);
            State = prevState;
        }
    }

    IEnumerator CoAttack()
    {
        State = State.attack;
        yield return new WaitForSeconds(Random.Range(blinkIntervalMin, blinkIntervalMax));
        State = State.angry;
        yield return null;
    }

    IEnumerator CoFrameChangeChecker()
    {
        while (true)
        {
            if (State == State.talk)
            {
                //Debug.Log(microphoneInput.currentVolume);
                if (microphoneInput.currentVolume < microphoneInput.volumeTreshold)
                {
                    loadAssets.objTalkIndicator.SetActive(false);
                    State = State.idle;
                }
                else
                {
                    int rnd = Random.Range(0, 3);

                    while (rnd == currentFrame)
                    {
                        rnd = Random.Range(0, 5);
                    }
                    if (npc != null)
                    {
                        switch (rnd)
                        {
                            case 0:
                                currentFrame = 0;
                                rend.material.mainTexture = npc.textures[currentFrame];
                                break;
                            case 1:
                            case 2:
                                if (npc.textures[2] != null)
                                {
                                    currentFrame = 2;
                                    rend.material.mainTexture = npc.textures[currentFrame];
                                }
                                break;
                            case 3:
                            case 4:
                                if (npc.textures[3] != null)
                                {
                                    currentFrame = 3;
                                    rend.material.mainTexture = npc.textures[currentFrame];
                                }
                                break;
                        }
                    }
                }
            }
//            Debug.Log(currentFrame);
            yield return new WaitForSeconds(0.1f + Random.Range(0, 0.1f));
        }
    }

    public void SetNPC(NPC npc)
    {
        this.npc = npc;

        State = State.idle;

    }


    void Start()
    {
        loadAssets = FindObjectOfType<LoadAssets>();
        rend = GetComponent<Renderer>();
        microphoneInput = FindObjectOfType<MicrophoneInput>();

        StartCoroutine(CoBlink());
        StartCoroutine(CoFrameChangeChecker());
    }



    void Update()
    {
        
    }
}
