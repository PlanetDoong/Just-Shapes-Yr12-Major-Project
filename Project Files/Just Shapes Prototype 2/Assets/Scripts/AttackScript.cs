using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackScript : MonoBehaviour
{
    // Reference to attack's renderer
    Renderer attackRend;

    // Reference to the game controller to access the end game function
    GameController gc;

    // Start and end colors for the Lerp
    // Start is background color, end is red
    public Color startColor;
    public Color endColor;

    // Where on lerp to start and end fade-in
    float startLerp = 0.1f;
    float endLerp = 0.6f;

    // Stores current position on lerp
    float timeAcc;

    // How long to wait after fade-in before suicide
    public float lifetime;

    // Is warning/fade-in happening?
    public bool warningPhase;

    // Length of the warning/fade-in
    public float warningLength;
    

    void Start()
    {
        // Get game controller script
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        // Get attack's renderer
        attackRend = GetComponent<Renderer>();

        // Set lerp's current position to the start lerp position
        timeAcc = startLerp;

        // Start the warning phase
        warningPhase = true;
    }


    void Update()
    {
        // If it's currently the warning phase...
        if (warningPhase) {

            // If time accumulation is greater or equal to end lerp (Fade has finished)
            if (timeAcc >= endLerp) {

                // Set the color to end lerp, just in case the time accumulation went over
                attackRend.material.color = Color.Lerp(startColor, endColor, endLerp);

                // End the warning phase
                warningPhase = false;

                // Start the Suicide coroutine
                StartCoroutine(Suicide());

            } else {

                // Set color to lerp of time accumulation
                attackRend.material.color = Color.Lerp(startColor, endColor, timeAcc);

                // Time.deltaTime is the time since the last frame. In a perfect world, if the game was running
                // at 60 frames per second then after 1 second (ingnoring the division) timeAcc would equal 60
                // However, since fade-in ends when timeAcc >= endLerp (0.6), we'll never be able to change the
                // fade-in length from 0.6 seconds. To scale Time.deltaTime, it's divided by warningLength.
                timeAcc += Time.deltaTime / warningLength;
            }
        }
    }

    IEnumerator Suicide() {

        // Set the color to white, then wait 0.1 seconds. This handles the flash
        attackRend.material.color = Color.white;
        yield return new WaitForSeconds(0.1f);

        // Set the attack's color to full red and move it to z = 0 so the player can collide with it
        attackRend.material.color = endColor;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        // Wait for the duration of lifetime
        yield return new WaitForSeconds(lifetime);

        // Make the attack the same color as the background, then delete it
        attackRend.material.color = startColor;
        Destroy(gameObject);
    }

    // This is run when the attack collides with something
    private void OnTriggerEnter(Collider other) {
        
        // If the object collided with has the tag "Player"...
        if (other.tag == "Player") {
            
            // Run the EndGame function in the Game Controller
            gc.EndGame();
        }
    }
}
