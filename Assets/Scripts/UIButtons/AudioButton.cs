using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioButton : BaseButton
{

    [SerializeField] Sprite audioOnSprite;
    [SerializeField] Sprite audioOffSprite;
    [SerializeField] SpriteRenderer audioSpriteRenderer;

    private bool _audioMode = true;
    public bool On { get {return _audioMode; } }

    void Start()
    {
        float scale = 0.075f;
        originalSize = new Vector3(scale, scale, 1);
    }

    public override void Clicked()
    {
        ToggleAudio();
    }

    private void ToggleAudio()
    {
        if (_audioMode)
        {
            AudioOff();
        }
        else
        {
            AudioOn();
        }
    }

    private void AudioOff()
    {
        _audioMode = false;
        audioSpriteRenderer.sprite = audioOffSprite;
    }

    private void AudioOn()
    {
        _audioMode = true;
        audioSpriteRenderer.sprite = audioOnSprite;
    }


}
