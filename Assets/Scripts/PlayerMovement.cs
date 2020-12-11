/************************************
 * Author: Emmett Hale
 * 
 * Purpose: Manages the players 
 * movement
 ************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jump_height = 3f;
    Vector3 velocity;

    public Transform groundchecker;

    public float groundDistance = 0.4f;

    public LayerMask groundMask;

    public Vector3 currentCheckpoint;
    public Quaternion currentCheckpointRotation;
    bool isGrounded = true;
    bool isSprinting = false;
    float time_since_step = 0;
    bool stepped = false;
    bool left = false;

    bool movementEnabled = true;

    float coolingDown = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //Turn of debug messages when in release
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
  Debug.unityLogger.logEnabled = false;
#endif
    }

    private void Start()
    {
        currentCheckpoint = transform.position;

        if(BlackoutPanel != null)
        {
            blackoutImage = BlackoutPanel.GetComponent<Image>();
        }
    }

    private bool isBlackout = false;

    public float blackoutTime = 3;
    float timeSinceBlackOut = 0;
    public float blackoutWaitTime = 3;

    public GameObject BlackoutPanel;
    private Image blackoutImage;

    //Kills the player, sending them to the checkpoint last saved
    public void Kill()
    {
        if (RoomManager.rooms != null)
        {
            foreach (RoomManager room in RoomManager.rooms)
            {
                room.PlayerDead();
            }
        }

        if (currentRoom != null)
        {
            currentRoom.DeactivateRoom();
        }

        //Enable Black Screen
        Color colorB = blackoutImage.color;
        colorB.a = 1;
        blackoutImage.color = colorB;

        //Set flag to fade black screen
        isBlackout = true;
        timeSinceBlackOut = 0;
        //Set player position to checkpoint
        controller.enabled = false;
        transform.position = currentCheckpoint;
        transform.rotation = currentCheckpointRotation;
        controller.enabled = true;

        if(PlayerInteract.instance != null && PlayerInteract.instance.IsUsingPhone())
        {
            PlayerInteract.instance.TogglePhone();
        }
    }

    [HideInInspector]
    public RoomManager currentRoom;

    // Update is called once per frame
    void Update()
    {
        //If player is out of bounds
        if(transform.position.y <= -500)
        {
            Kill();
        }

        //Handle blackout
        if(isBlackout)
        {
            timeSinceBlackOut += Time.deltaTime;

            Color colorB;

            if (timeSinceBlackOut >= blackoutWaitTime)
            {
                colorB = blackoutImage.color;
                colorB.a = 1 - (timeSinceBlackOut - blackoutWaitTime) / blackoutTime;
                blackoutImage.color = colorB;
            }

            if (timeSinceBlackOut >= blackoutTime + blackoutWaitTime)
            {
                isBlackout = false;
                timeSinceBlackOut = 0;

                colorB = blackoutImage.color;
                colorB.a = 0;
                blackoutImage.color = colorB;
            }
        }

        //Check if player is grounded
        isGrounded = Physics.CheckSphere(groundchecker.position, groundDistance, groundMask);

        //turn of gravity if on ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move;

        if (coolingDown > 0)
        {
            move = transform.right * 0.5f + transform.forward * 0.5f;

            coolingDown -= Time.deltaTime;
        }

        move = transform.right * x + transform.forward * z;

        if (movementEnabled)
        {
            //Move player
            controller.Move(move * speed * Time.deltaTime);

            //Check for jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jump_height * -2f * gravity);
            }


            if (!stepped && isGrounded && (move.x != 0 || move.z != 0)) //if We started moving
            {

                if (AudioManager.instance != null)
                {
                    if (left)
                    {
                        AudioManager.instance.Stop("Player Footsteps 1");
                        AudioManager.instance.Play("Player Footsteps 1");
                        left = false;
                    }
                    else
                    {
                        AudioManager.instance.Stop("Player Footsteps 2");
                        AudioManager.instance.Play("Player Footsteps 2");
                        left = true;
                    }

                }

                stepped = true;

            }
            else if (stepped && isGrounded && (move.x != 0 || move.z != 0)) //If we are still moving
            {
                time_since_step += Time.deltaTime;

                if (time_since_step >= 1.5 / speed)
                {
                    if (left)
                    {
                        AudioManager.instance.Stop("Player Footsteps 1");
                        AudioManager.instance.Play("Player Footsteps 1");
                        left = false;
                    }
                    else
                    {
                        AudioManager.instance.Stop("Player Footsteps 2");
                        AudioManager.instance.Play("Player Footsteps 2");
                        left = true;
                    }

                    time_since_step = 0;
                }
            }
            else //If We Stopped Moving
            {
                stepped = false;
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.Stop("Player Footsteps 1");
                    AudioManager.instance.Stop("Player Footsteps 2");
                }
            }
        }

        //Sprint
        if(Input.GetButtonDown("Fire3") && isGrounded && !isSprinting)
        {
            isSprinting = true;
            speed = speed * 2f;
        }

        //Stop sprint
        if(Input.GetButtonUp("Fire3") && isSprinting)
        {
            isSprinting = false;
            speed = speed / 2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if(x != 0 || z != 0)
        {
            coolingDown = 0.5f;
        }

    }

    public void ToggleMovement()
    {
        movementEnabled = !movementEnabled;
    }
}
