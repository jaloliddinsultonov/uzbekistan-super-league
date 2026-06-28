using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUI : MonoBehaviour
{
    public void OnBackPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
