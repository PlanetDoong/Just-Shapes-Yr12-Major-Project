using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Attack Prefabs
    public GameObject squareAttack;
    public GameObject horizontalAttack;

    // Instance of the Game Over script
    public GameOverScript gameOver;

    // Variables related to attack spawning
    public float spawnAttackRepeat;        // Time delay between attack spawns
    public float phaseGap;                 // Time delay between phases
    public bool gameRunning = false;       // Is the game running
    public int score = 0;                  // To keep track of the score
    public int phaseSize = 5;              // How many attacks in each phase

    // Variables related to tutorial
    public bool tutorialPhase = false;     // Is it the tutorial phase
    public float tutorialStartTimeBuffer;  // Time delay between start of game and start of tutorial phase
    public float tutorialReadTime;         // How long to leave up each tutorial text
    public List<Text> tutorialTexts;       // List of tutorial texts to display

    // Player game object
    GameObject player;

    // Player's transform (could be gotten from player game object but
    // doing this makes lines shorter and easier to read)
    Transform playerTrans;

    // Instance of generate phase script
    GeneratePhase phaseGenerator;

    // Start is called before the first frame update
    void Start()
    {
        // Populate variables with instances of objects
        player = GameObject.FindGameObjectWithTag("Player");
        playerTrans = player.GetComponent<Transform>();
        phaseGenerator = GetComponent<GeneratePhase>();
        gameOver = GetComponent<GameOverScript>();

        // Start the tutorial phase and make sure all tutorial texts are hidden
        tutorialPhase = true;
        foreach (Text tutText in tutorialTexts) {
            tutText.enabled = false;
        }

        // If statement used in testing to skip the tutorial
        if (gameRunning) {

            // Start the game
            StartCoroutine(ExecutePhase(phaseGenerator.GenerateNewPhase(score, phaseSize)));

        } else if (tutorialPhase) {

            // Start the tutorial
            StartCoroutine(DoTutorial());
        }
    }


    IEnumerator ExecutePhase(List<string> nextPhase) {

        // For each attack in the next phase string
        foreach (string attack in nextPhase) {

            // Wait the spawn delay
            yield return new WaitForSeconds(spawnAttackRepeat);

            // Make sure the game is still running
            if (gameRunning) {
                if (attack == "square") {

                    // Spawn in square attack at player's position
                    Instantiate(squareAttack, new Vector3(playerTrans.position.x, playerTrans.position.y, 10), squareAttack.transform.rotation);

                } else if (attack == "horizontal") {

                    // Spawn in horizontal attack at player's y position
                    Instantiate(horizontalAttack, new Vector3(0, playerTrans.position.y, 10), horizontalAttack.transform.rotation);

                } else {

                    // I love self depricating error messages, it really makes me feel responsible for whatever wrong I did
                    Debug.Log("You messed up the if statement for the attack types retard. What kind of an attack is " + attack + "?");
                }
            }
        }

        // At the end of the phase, make sure the game is still running
        if (gameRunning) {

            // Add 1 to the score
            score += 1;

            // Wait for the phase gap
            yield return new WaitForSeconds(phaseGap);

            // Execute the next phase
            StartCoroutine(ExecutePhase(phaseGenerator.GenerateNewPhase(score, phaseSize)));
        }
    }

    // Ends the game
    public void EndGame() {

        // Sets game running to false
        gameRunning = false;

        // Puts all on screen attacks in an array by searching for all gameo bjects with the tag "Attack"
        GameObject[] attacksOnScreen = GameObject.FindGameObjectsWithTag("Attack");

        // Go through each one and delete them
        foreach (GameObject attack in attacksOnScreen) {
            Destroy(attack);
        }

        // Run game over function in game over script (its handles the UI stuff)
        gameOver.GameOver();
    }


    // Do the tutorial
    IEnumerator DoTutorial() {

        // Wait the start time buffer
        yield return new WaitForSeconds(tutorialStartTimeBuffer);

        // For every tutorial text
        foreach (Text tutText in tutorialTexts) {

            // Show it on screen, wait the read time, then hide it again
            tutText.enabled = true;
            yield return new WaitForSeconds(tutorialReadTime);
            tutText.enabled = false;
        }

        // Set game running to true and tutorial phase to false
        gameRunning = true;
        tutorialPhase = false;

        // Start the game
        StartCoroutine(ExecutePhase(phaseGenerator.GenerateNewPhase(score, phaseSize)));

    }
}
