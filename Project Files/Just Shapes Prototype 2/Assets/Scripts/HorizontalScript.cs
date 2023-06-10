using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HorizontalScript : MonoBehaviour
{
    // Get reference variables
    Transform playerTrans;   // Player's Transform component
    GameController gc;       // Instance of Game Controller script
    Renderer attackRend;     // The attack's renderer
    Transform attackTrans;   // The attack's Transform

    // Lifetime of the attack
    public float lifetime;

    // Warning phase variables
    bool warningPhase;            // Is it the warning phase
    public float warningLength;   // Length of the warning phase
    float timeAcc;                // Time accumulation (used in the fade in)
    public float startLerp;       // Start value of the fade lerp
    public float endLerp;         // End value of the fade lerp
    public Color startColor;      // Start color of fade lerp
    public Color endColor;        // End color of fade lerp

    // Move phase variables
    bool movePhase;           // Is it move phase
    bool moveRight;            // Is the attack moving from left to right
    public float moveSpeed;   // Speed of the attack


    // Gets the direction the attack will move in (true = left to right, false = right to left)
    bool GetDirection() {

        // Get distance of player to left and right sides of screen
        float playerDistToLeftSide = Vector3.Distance(playerTrans.position, new Vector3(-18, playerTrans.position.y, 0));
        float playerDistToRightSide = Vector3.Distance(playerTrans.position, new Vector3(18, playerTrans.position.y, 0));

        // Test if player is closer to right or left side of screen
        if (playerDistToRightSide <= playerDistToLeftSide) {
            // True means go left to right
            return true;
        } else {
            // False means go right to left
            return false;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Get components for variables
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        attackRend = GetComponent<Renderer>();
        attackTrans = GetComponent<Transform>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        // Set up warning phase
        warningPhase = true;
        timeAcc = startLerp;

        // Set up move phase
        movePhase = false;
        moveRight = GetDirection();
    }

    // Update is called once per frame
    void Update()
    {
        // If currently warning phase
        if (warningPhase) {

            // If warning phase has ended
            if (timeAcc >= endLerp) {

                // Set color to endLerp in case the timeAcc goes over
                attackRend.material.color = Color.Lerp(startColor, endColor, endLerp);

                // End warning phase
                warningPhase = false;

                // Set up move phase
                if (moveRight) {
                    attackTrans.position = new Vector3(17, attackTrans.position.y, 0);
                } else {
                    attackTrans.position = new Vector3(-17, attackTrans.position.y, 0);
                }

                // Set color to full red and start move phase
                attackRend.material.color = endColor;
                movePhase = true;


            } else {

                // Set color for warning
                attackRend.material.color = Color.Lerp(startColor, endColor, timeAcc);

                // Time.deltaTime is the time since the last frame. In a perfect world, if the game was running
                // at 60 frames per second then after 1 second (ingnoring the division) timeAcc would equal 60
                // However, since fade-in ends when timeAcc >= endLerp (0.6), we'll never be able to change the
                // fade-in length from 0.6 seconds. To scale Time.deltaTime, it's divided by warningLength.
                timeAcc += Time.deltaTime / warningLength;
            }
        }


        // If currently move phase
        if (movePhase) {

            // If moving left to right...
            if (moveRight) {

                // Go in that direction
                attackTrans.Translate(Vector3.left * (Time.deltaTime * moveSpeed));

                // If attack has reached the middle of the screen
                if (attackTrans.position.x <= 0) {

                    // End the move phase
                    movePhase = false;
                    // Set the position to the center of the screen beacuse if the speed is really fast it can
                    // go past the middle
                    attackTrans.position = new Vector3(0, attackTrans.position.y, attackTrans.position.z);

                    // Waits for the length of lifetime, then destroys itself
                    StartCoroutine(Suicide());
                }
            
            // If moving right to left...
            } else {

                // Move in the other direction
                attackTrans.Translate(Vector3.right * (Time.deltaTime * moveSpeed));

                // If attack has reached the middle of the screen
                if (attackTrans.position.x >= 0) {

                    // End the move phase
                    movePhase = false;
                    // Set the position to the cente of the screen because if the speed is really fast it can
                    // go past the middle
                    attackTrans.position = new Vector3(0, attackTrans.position.y, attackTrans.position.z);

                    // Waits for the length of lifetime, then destroys itself
                    StartCoroutine(Suicide());
                }
            }
        }
    }

    IEnumerator Suicide() {
        // Wait for the length of lifetime
        yield return new WaitForSeconds(lifetime);

        // Destoy this game object
        Destroy(gameObject);
    }

    // If something enters the attacks collider...
    private void OnTriggerEnter(Collider other) {

        // Check if collided object has tag "Player"
        if (other.tag == "Player") {

            // End the game
            gc.EndGame();
        }
    }
}
