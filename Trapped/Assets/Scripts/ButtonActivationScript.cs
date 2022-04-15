using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivationScript : MonoBehaviour
{
    public GameObject PressEUI;

    AudioSource objectSound;

    bool canBeActivated = false;
    public bool isActivated = false;

    private void Start()
    {
        PressEUI.SetActive(false);
        objectSound = GetComponent<AudioSource>();
        objectSound.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(isActivated == false)
            {
                PressEUI.SetActive(true);
            }
            canBeActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PressEUI.SetActive(false);
            canBeActivated = false;
        }
    }

    private void Update()
    {
        if(canBeActivated == true)
        {

            if (Input.GetKeyDown(KeyCode.E) && isActivated == false)
            {
                isActivated = true;
                Debug.Log("Activé");
                FindObjectOfType<AudioManager>().Play("Scratch");
                objectSound.Stop();
            }
        }
    }
}
