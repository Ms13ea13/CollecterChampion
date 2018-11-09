using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerBase
{
    [SerializeField] private string HorizontalKey;
    [SerializeField] private string VertivalKey;

    [SerializeField] private Vector3 moveMentControlInput;

    private PlayerRayCast playerRayCast;

    void Start()
    {
        playercontrol = GetComponentInParent<CharacterController>();
        playerRayCast = GetComponent<PlayerRayCast>();
        rigibody = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            var tempSliderValue = 0f;
            var SetFoodOnFireValue = 100f;
            var cookTimer = 60f;
            LeanTween.value(tempSliderValue, SetFoodOnFireValue + 50f, cookTimer)
                .setOnUpdate(Value =>
                {
                    Debug.Log("val: " + Value);
                    tempSliderValue = Value;
                })
                .setOnComplete(() => { Debug.Log("food is Overcooked"); });
        }

        MovePlayer();
        playerRayCast.ShootRayCast();
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
            }
        }

        velocity.y /= 1 + drag.y * Time.deltaTime;
        velocity.y -= gravity * Time.deltaTime;

        playercontrol.Move(velocity * Time.deltaTime + moveMentControlInput * Time.deltaTime * speed);
    }
}