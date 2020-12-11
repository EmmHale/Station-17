/************************************
 * Author: Emmett Hale
 * 
 * Purpose: Manages the mouse interaction
 * with the camera for the player
 ************************************/
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class LookScript : MonoBehaviour
{
    public float mouse_sensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouse_sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouse_sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void SetSensitivity(float value)
    {
        mouse_sensitivity = value;
    }

    public void SetRenderDistance(float value)
    {
        //If fog is not currently being used
        if (PlayerInteract.instance != null & PlayerInteract.instance.IsFogMoved())
        {
            RenderSettings.fogEndDistance = value;

            RenderSettings.fogStartDistance = RenderSettings.fogEndDistance - 2;
        }

        gameObject.GetComponent<Camera>().farClipPlane = value;
    }
}
