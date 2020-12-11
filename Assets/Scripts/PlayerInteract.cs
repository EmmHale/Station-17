/************************************
 * Author: Emmett Hale
 * 
 * Purpose: Manages the players 
 * interaction with other objects
 * 
 * ->Expand with customizable controls
 ************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract instance;
    private float baseFov;
    private float currentFov;
    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Debug.Log("Two player interacts!");
        }
        else
        {
            instance = this;
        }

        if(Phone)
        {
            phoneAnimator = Phone.GetComponent<Animator>();
        }

        //Set the base field of view
        baseFov = GetComponent<Camera>().fieldOfView;
        currentFov = baseFov;
        
        Cursor.visible = false;

        //Save current render distance
        baseRenderDistance = GetComponent<Camera>().farClipPlane;
    }

    public float zoom_rate = 0.5f;
    public float zoom_time = 0.5f;
    private float timeSinceZoom = 0;
    private bool zooming = false;

    private GameObject last_hit;
    public List<Shader> last_shaders;
    public GameObject uiInteractText;
    public GameObject uiInteractSlider;
    public Shader highlight_shader;
    public GameObject zoomLight;

    public GameObject Phone;
    private Animator phoneAnimator;

    
    
    // Update is called once per frame

    private float action_time = 0;
    private bool doing_long_action = false;
    private bool using_phone = false;
    private float timeSinceTogglePhone = 0;
    public float timeToFadeBlackPhone = 1f;
    private float baseRenderDistance;
    public float minRenderDistance = 5f;

    public GameObject ExitPanel;
    public GameObject BlackoutPanel;
    public GameObject InteractionPanel;
    public GameObject Crosshair;
    public GameObject BlackCube;

    //Getter for long action
    public bool IsDoingLongAction()
    {
        return doing_long_action;
    }

    //Getter for using phone
    public bool IsUsingPhone()
    {
        return using_phone;
    }

    //Layer mask for 
    public LayerMask mask;

    public bool IsFogMoved()
    {
        return timeSinceTogglePhone < timeToFadeBlackPhone;
    }

    void Update()
    {
        RaycastHit hit;
        var cameraCenter = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, GetComponent<Camera>().nearClipPlane));

        //Use fog
        if(doing_long_action && timeSinceTogglePhone < timeToFadeBlackPhone)
        {
            timeSinceTogglePhone += Time.deltaTime;

            RenderSettings.fogEndDistance = baseRenderDistance - ((baseRenderDistance - minRenderDistance) * (timeSinceTogglePhone / timeToFadeBlackPhone));
            RenderSettings.fogStartDistance = RenderSettings.fogEndDistance - 2;

            if (timeSinceTogglePhone >= timeToFadeBlackPhone)
            {
                RenderSettings.fogEndDistance = minRenderDistance;
            }
        }

        //Turn off fog
        if(!doing_long_action && timeSinceTogglePhone < timeToFadeBlackPhone)
        {
            timeSinceTogglePhone += Time.deltaTime;

            RenderSettings.fogEndDistance = minRenderDistance + ((baseRenderDistance - minRenderDistance) * (timeSinceTogglePhone / timeToFadeBlackPhone));
            RenderSettings.fogStartDistance = RenderSettings.fogEndDistance - 2;

            if (timeSinceTogglePhone >= timeToFadeBlackPhone)
            {
                RenderSettings.fogEndDistance = gameObject.GetComponent<Camera>().farClipPlane;
            }
        }

        //Check if there is an interactable
        if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 2, mask) && hit.transform.gameObject.GetComponent<Interactable>() != null)
        {
            GameObject obj = hit.transform.gameObject;

            //If there is
            if (last_hit != obj)
            {
                //If there was a different object saved
                if (last_hit != null)
                {

                    last_hit.GetComponent<Outline>().OutlineWidth = 0;
                    uiInteractText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                }

                //Save object
                last_hit = obj;

                last_hit.GetComponent<Outline>().OutlineWidth = 3;

                if (!using_phone)
                    uiInteractText.GetComponent<TMPro.TextMeshProUGUI>().text = last_hit.transform.gameObject.GetComponent<Interactable>().interactText;

            }
            else
            {
                //Set text to objects text
                uiInteractText.GetComponent<TMPro.TextMeshProUGUI>().text = last_hit.transform.gameObject.GetComponent<Interactable>().interactText;

                //if the player is doing a long action
                if (doing_long_action && !using_phone)
                {
                    //if the long action is done
                    if (action_time >= last_hit.GetComponent<Interactable>().interactTime)
                    {
                        //Remove fog
                        //Disable slider
                        //set flag
                        //Call complete action
                        uiInteractSlider.SetActive(false);
                        doing_long_action = false;
                        baseRenderDistance = gameObject.GetComponent<Camera>().farClipPlane;
                        timeSinceTogglePhone = 0;
                        ////////

                        action_time = 0;
                        last_hit.GetComponent<Interactable>().CompleteAction();
                        PlayerMovement.instance.ToggleMovement();
                    }
                    else
                    {
                        //if Long action is not done progress timer and bar
                        action_time += Time.deltaTime;

                        uiInteractSlider.GetComponent<Slider>().value = (action_time / last_hit.GetComponent<Interactable>().interactTime);
                    }
                }
            }

        }
        else
        {
            //If there is an object seen
            if (last_hit)
            {
                //Display info
                last_hit.GetComponent<Outline>().OutlineWidth = 0;
                uiInteractText.GetComponent<TMPro.TextMeshProUGUI>().text = "";

                //If doing long action
                if (doing_long_action && !using_phone)
                {
                    last_hit.GetComponent<Interactable>().ClearAction();
                    doing_long_action = false;
                    baseRenderDistance = gameObject.GetComponent<Camera>().farClipPlane;
                    timeSinceTogglePhone = 0;
                    /////

                    action_time = 0;
                    uiInteractSlider.SetActive(false);
                    PlayerMovement.instance.ToggleMovement();
                }

                last_hit = null;
            }
        }

        //If e is let go while doing long action that is not phone
        if (Input.GetKeyUp(KeyCode.E) && last_hit && doing_long_action && !using_phone)
        {
            //Clean up
            //Remove fog
            last_hit.GetComponent<Interactable>().ClearAction();
            doing_long_action = false;
            baseRenderDistance = gameObject.GetComponent<Camera>().farClipPlane;
            timeSinceTogglePhone = 0;
            action_time = 0;
            uiInteractSlider.SetActive(false);
            PlayerMovement.instance.ToggleMovement();
        }

        //If right click
        if (Input.GetMouseButtonDown(1))
        {
            //ZOOM
            if(!zooming)
            {
                timeSinceZoom = 0;
                zooming = true;
                Debug.Log("Starting Zoom");
            }
            else
            {
                zooming = false;
                timeSinceZoom = 0;
                GetComponent<Camera>().fieldOfView = baseFov - baseFov * (zoom_rate);
            }
        }

        //If Left Click
        if (Input.GetMouseButtonDown(0))
        {
            //Light
            zoomLight.SetActive(!zoomLight.activeSelf);
        }

        //Do zoom
        if (zooming)
        {
            if (timeSinceZoom < zoom_time)
            {
                Debug.Log(zoom_rate);
                Debug.Log(baseFov * zoom_rate);
                GetComponent<Camera>().fieldOfView = baseFov - baseFov * (zoom_rate * (timeSinceZoom / zoom_time));
                timeSinceZoom += Time.deltaTime;
            }
            else
            {
                GetComponent<Camera>().fieldOfView = baseFov - baseFov * (zoom_rate);
            }
        }
        else
        {
            if (timeSinceZoom < zoom_time)
            {
                Debug.Log(zoom_rate);
                Debug.Log(baseFov * zoom_rate);
                GetComponent<Camera>().fieldOfView = baseFov - baseFov * (zoom_rate) + baseFov * (zoom_rate * (timeSinceZoom / zoom_time));
                timeSinceZoom += Time.deltaTime;
            }
            else
            {
                GetComponent<Camera>().fieldOfView = baseFov;
            }
        }

        //If e pressed start action on hovered interactable
        if (Input.GetKeyDown(KeyCode.E) && last_hit && !using_phone)
        {
            Debug.Log("Interacting...");
            if (last_hit.GetComponent<Interactable>().PreformAction())
            {
                //If the action is long
                if (last_hit.GetComponent<Interactable>().isCharge && !doing_long_action)
                {
                    doing_long_action = true;

                    //////////
                    ///
                    baseRenderDistance = gameObject.GetComponent<Camera>().farClipPlane;
                    timeSinceTogglePhone = 0;

                    uiInteractSlider.SetActive(true);
                    PlayerMovement.instance.ToggleMovement();
                }
            }
            else
            {
                //Display Fail Text
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.Play("Interaction Error");
                }
            }

        }

        //If q toggle phone
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TogglePhone();
        }

        //Display settings menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPanel.SetActive(!ExitPanel.activeSelf);
            BlackoutPanel.SetActive(!BlackoutPanel.activeSelf);
            InteractionPanel.SetActive(!InteractionPanel.activeSelf);
            Crosshair.SetActive(!Crosshair.activeSelf);

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    //Exit game
    public void ExitGame()
    {
        Debug.Log("Exiting");
        Application.Quit();
    }

    //Toggle the phone
    public void TogglePhone()
    {
        Debug.Log("Toggling Phone");
        //Stop user toggling phone
        if (phoneAnimator.GetBool("IsTransition") == false)
        {
            if (using_phone && phoneAnimator.GetCurrentAnimatorStateInfo(0).IsName("Phone Up Animation"))
            {
                using_phone = false;
                doing_long_action = false;
                phoneAnimator.SetBool("IsUp", false);
                phoneAnimator.SetBool("IsDown", true);
                phoneAnimator.SetBool("IsTransition", true);

                //Start fog
                baseRenderDistance = gameObject.GetComponent<Camera>().farClipPlane;
                timeSinceTogglePhone = 0;
            }
            else if (phoneAnimator.GetCurrentAnimatorStateInfo(0).IsName("Phone Down Animation"))
            {
                if (doing_long_action)
                {
                    last_hit.GetComponent<Interactable>().ClearAction();
                    doing_long_action = false;
                    action_time = 0;
                    uiInteractSlider.SetActive(false);
                    PlayerMovement.instance.ToggleMovement();
                }

                using_phone = true;
                uiInteractText.GetComponent<TMPro.TextMeshProUGUI>().text = "";

                doing_long_action = true;
                phoneAnimator.SetBool("IsUp", true);
                phoneAnimator.SetBool("IsDown", false);
                phoneAnimator.SetBool("IsTransition", true);

                //remove fog
                baseRenderDistance = gameObject.GetComponent<Camera>().farClipPlane;
                timeSinceTogglePhone = 0;
            }
        }
        
    }
}
