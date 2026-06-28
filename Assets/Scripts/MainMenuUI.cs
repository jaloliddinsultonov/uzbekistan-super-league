using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenuUI : MonoBehaviour
{
    public void OnStartPressed()
    {
        SceneManager.LoadScene("ClubSelect");
    }

    public void OnCreditsPressed()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnSettingsPressed()
    {
        SceneManager.LoadScene("Settings");
    }
}