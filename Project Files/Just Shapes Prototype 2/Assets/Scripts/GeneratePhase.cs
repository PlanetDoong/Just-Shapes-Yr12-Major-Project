using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePhase : MonoBehaviour
{
    // List of available attack types to choose from (allows for easy addition of new attacks)
    public List<string> attackTypes = new List<string>{"square", "horizontal"};

    // How often do you unlock a new attack type that can be randomly chosen from the attack types list
    public int scoreIncrementUnlock = 5;

    
    public string RandomStringFromList(List<string> stringList) {

        // Return string list with a random index (putting this in its own function allows for shorter lines)
        return stringList[Random.Range(0, stringList.Count)];
    }


    public List<string> GenerateNewPhase(int score, int phaseLength) {

        // Stores the unlocked attack types that can be randomly chosen from
        List<string> availableAttackTypes = new List<string>();

        // Stores what will be outputted as the next phase
        List<string> nextPhase = new List<string>();

        // For every attack type...
        for (int i = 0; i < attackTypes.Count; i++) {

            // If the player has a high enough score to unlock the attack
            if (score >= i*scoreIncrementUnlock) {

                // Add the attack to available attacks
                availableAttackTypes.Add(attackTypes[i]);
            }
        }

        // Repeat for every attack in phase length
        for (int i = 0; i < phaseLength; i++) {

            // Pick a random attack from available attack types
            nextPhase.Add(RandomStringFromList(availableAttackTypes));
        }

        return nextPhase;
    }
}
