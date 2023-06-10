using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start button function
    public void StartGame() {

        // Load the main game scene
        SceneManager.LoadScene(1);
    }

    // Quit button function
    public void QuitGame() {

        // Quit the game
        Application.Quit();
    }
}
