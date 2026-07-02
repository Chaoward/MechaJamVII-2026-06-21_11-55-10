using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private MechMove movement;
    [SerializeField] private AudioSource footstepAudio;
    [SerializeField] private AudioSource laserAudio;

    private void Update()
    {
        if (movement.isMoving && movement.isGrounded)
        {
            if (!footstepAudio.isPlaying)
                footstepAudio.Play();
        }
        else
        {
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();
        }
    }
}
