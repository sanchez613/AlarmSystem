using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Alarm : MonoBehaviour
{
    [SerializeField] private float _volumeUpStep = 0.05f;
    [SerializeField] private float _volumeDownStep = 0.1f;

    private AudioSource _audioSource;
    private float _minVolume = 0;
    private float _maxVolume = 1;
    Coroutine _volumeUpJob;
    Coroutine _volumeDownJob;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void AlarmOn()
    {
        if (_volumeDownJob != null)
            StopCoroutine(_volumeDownJob);

        _audioSource.Play();
        _volumeUpJob = StartCoroutine(VolumeChange(_volumeUpStep, _maxVolume));
    }

    public void AlarmOff()
    {
        if (_volumeUpJob != null)
            StopCoroutine(_volumeUpJob);

        _volumeDownJob = StartCoroutine(VolumeChange(_volumeDownStep, _minVolume));

        if (_audioSource.volume == 0)
            _audioSource.Stop();
    }

    private IEnumerator VolumeChange(float step, float targetVolume)
    {
        var vaitingTime = new WaitForSeconds(1f);

        while (_audioSource.volume != targetVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetVolume, step);
            yield return vaitingTime;
        }
    }
}
