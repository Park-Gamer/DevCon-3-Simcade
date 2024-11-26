using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject rules;
    public void PlayGame()
    {
        StartCoroutine(DelayForAudio()); 
    }

    void Start()
    {
        rules.SetActive(false);
    }

    public void OnControls()
    {
        rules.SetActive(true);
    }
    public void OffControls()
    {
        rules.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator DelayForAudio() // Countdown till draw timer
    {
        yield return new WaitForSeconds(1f); 
        SceneManager.LoadScene("SampleScene");
    }
}
