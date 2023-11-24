using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorSound : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioErrorClip;
    [SerializeField, Range(0f, 100f)] private float _volume;
    private bool _canPlaySound = true;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        PlayerManager.OnSoundErrorReceived += SoundError;
    }
    
    private void Update()
    {
        _audioSource.volume = _volume / 100;
    }
        
    public void SoundError()
    {
        StartCoroutine(DelayToPlaySound());
    }
    
    private IEnumerator DelayToPlaySound()
    {
        if (_canPlaySound)
        {
            _canPlaySound = false;
            
            _audioSource.clip = _audioErrorClip;
            _audioSource.Play();
            _audioSource.loop = false;
            Debug.Log($"soltou o som");
            yield return new WaitForSeconds(5f);
            _canPlaySound = true;
        }
    }
}
