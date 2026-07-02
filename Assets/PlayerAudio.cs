using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    [SerializeField] private AudioSource footstepAudio;
    [SerializeField] private AudioSource laserAudio;
    [SerializeField] private AudioSource jumpAudio;

    private void Update()
    {
        var movement = player.GetComponent<MechMove>();
        var targeting = player.GetComponent<MechTargeting>();

        InputAction jumpAction = InputHandler.curActionMap.FindAction("Jump");

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

        if (targeting.isFiring)
        {
            laserAudio.Play();
        }

        if (jumpAction.WasPressedThisFrame() && movement.isGrounded)
        {
            jumpAudio.Play();
        }


    }
}
