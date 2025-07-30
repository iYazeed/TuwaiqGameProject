using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMainMenuGame : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}