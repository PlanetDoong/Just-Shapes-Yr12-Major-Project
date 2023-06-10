using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Variables
    public GameController gc;       // Instance of game controller script
    public GameObject gameOverUI;   // Empty game object holding UI elements
    public Text scoreText;          // Text that displays the score


    void Start() {

        // Get game controller instance
        gc = GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Hide game over UI elements if the game is running or if its a tutorial phase
        if (gc.gameRunning || gc.tutorialPhase) {
            gameOverUI.SetActive(false);
        }
    }

    public void GameOver() {

        // Display game over UI elements
        gameOverUI.SetActive(true);

        // Update score text with player's score
        scoreText.text = gc.score.ToString();
    }

    // Retry button function
    public void RetryButton() {

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Quit button function
    public void QuitButton() {

        // Load the start menu scene
        SceneManager.LoadScene(0);
    }
}
