using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioPlayGameClip;
    [SerializeField] private AudioClip _audioMenuClip;
    [SerializeField, Range(0f, 100f)] private float _volume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void SoundPlayGame()
    {
        _audioSource.clip = _audioPlayGameClip;
        _audioSource.Play();
    }

    public void SoundMenu()
    {
        _audioSource.clip = _audioMenuClip;
        _audioSource.Play();
    }

    private void Update()
    {
        _audioSource.volume = _volume / 100;
    }
}
