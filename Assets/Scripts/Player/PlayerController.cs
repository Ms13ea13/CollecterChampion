using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerBase
{
    private Animator playerAnim;

    [SerializeField] private string HorizontalKey;
    [SerializeField] private string VertivalKey;

    [SerializeField] private Vector3 moveMentControlInput;

    private PlayerRayCast playerRayCast;
    private BoxCollider playerBoxColider;

    private AudioSource playerAudioSource;//

    void Start()
    {
        //transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        playerAnim = GetComponent<Animator>();
        playercontrol = GetComponentInParent<CharacterController>();
        playerRayCast = GetComponent<PlayerRayCast>();
        rigibody = GetComponentInParent<Rigidbody>();
        playerBoxColider = GetComponentInParent<BoxCollider>();
        playerAudioSource = GetComponent<AudioSource>();//
    }

    void Update()
    {
        MovePlayer();
        playerRayCast.ShootRayCast();

        //if (playercontrol.isGrounded == true && cc.velocity.magnitude>2f&& playerAudioSource.isPlaying == false)
        //{
        //    playerAudioSource.Play();

        //}
    }

    private void MovePlayer()
    {
        if (!playercontrol)
            return;

        moveMentControlInput = new Vector3(Input.GetAxis(HorizontalKey), 0, Input.GetAxis(VertivalKey));

        if (playercontrol.isGrounded)
        {
            velocity.y = 0f;

            if (moveMentControlInput != Vector3.zero)
            {
                transform.forward = moveMentControlInput;
                velocity.x /= 1 + drag.x * Time.deltaTime;
                velocity.z /= 1 + drag.z * Time.deltaTime;
                playerAnim.SetBool("isRun", true);

                if (!playerAudioSource.isPlaying)
                {
                    playerAudioSource.Play();
                }
            }
            else
            {
                playerAnim.SetBool("isRun", false);
            }
        }

        velocity.y /= 1 + drag.y * Time.deltaTime;
        velocity.y -= gravity * Time.deltaTime;

       
        playercontrol.Move(velocity * Time.deltaTime + moveMentControlInput * Time.deltaTime * speed);
    }
}