using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mouvement")]

    public CharacterController controller;
    float speed;
    public float walkSpeed = 12f;
    public float runSpeed = 20;
    public float airSpeed = 8f;
    public float jumpHeight = 3f;
    bool isRunning;

    [Header("Gravity")]

    Vector3 velocity;
    public float gravity = -9.81f;

    [Header("Grouncheck")]

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    [Header("Camera")]

    public Camera playerCamera;
    public float walkFov = 80f;
    public float runFov = 90f;
    public float transitionTime = 0.5f;

    [Header("Footstep")]

    public AudioSource soundWalking;
    public AudioSource soundRunning;


    [Header("UI")]

    public GameObject deathMenuUI;

    private void Start()
    {
        deathMenuUI.SetActive(false);
    }

    private void Update()
    {
        //Move

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        //Run

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Z))
        {
            speed = runSpeed;
            isRunning = true;
        }
        else if (!isGrounded)
        {
            speed = airSpeed;
        }
        else
        {
            speed = walkSpeed;
            isRunning = false;
        }

        if ((Input.GetKeyUp(KeyCode.LeftShift) || !Input.GetKey(KeyCode.Z)) && playerCamera.fieldOfView == 90)
        {
            StartCoroutine(ChangeFov(runFov, walkFov, transitionTime));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.Z))
        {
            StartCoroutine(ChangeFov(walkFov, runFov, transitionTime));
        }


        //Jump

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }


        //Gravity

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Groundcheck

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Edge glitch fix

        if (isGrounded)
        {
            controller.stepOffset = 0.5f;
        }
        else
        {
            controller.stepOffset = 0f;
        }

        //Sound

        if((x!=0||z!=0) && !isRunning && isGrounded && soundWalking.isPlaying == false && soundRunning.isPlaying == false)
        {
            soundWalking.Play();
        }

        if(isRunning == true && isGrounded && soundRunning.isPlaying == false && soundWalking.isPlaying == false)
        {
            soundRunning.Play();
        }
    }

    IEnumerator ChangeFov(float fovStart, float fovEnd, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            playerCamera.fieldOfView = Mathf.Lerp(fovStart, fovEnd, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        playerCamera.fieldOfView = fovEnd;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Death");
            deathMenuUI.SetActive(true);
            controller.enabled = false;
            playerCamera.GetComponent<MouseLook>().lookEnabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}