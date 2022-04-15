using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] switches;

    [SerializeField]
    int noOfSwitches = 0;

    [SerializeField]
    Text switchCount;

    private void Start()
    {
        GetNoOfSwitches();
    }

    public int GetNoOfSwitches()
    {
        int x = 0;

        for(int i = 0; i < switches.Length; i++)
        {
            if(switches[i].GetComponent<ButtonActivationScript>().isActivated == false)
            {
                x++;
            }
            else if(switches[i].GetComponent<ButtonActivationScript>().isActivated == true)
            {
                noOfSwitches--;
            }
        }

        noOfSwitches = x;
        
        return noOfSwitches;
    }

    public void Win()
    {
        if(noOfSwitches <= 0)
        {
            SceneManager.LoadScene("EndScene");
            Debug.Log("Gagné");
        }
    }

    private void Update()
    {
        Win();

        switchCount.text = GetNoOfSwitches().ToString();
    }
}
