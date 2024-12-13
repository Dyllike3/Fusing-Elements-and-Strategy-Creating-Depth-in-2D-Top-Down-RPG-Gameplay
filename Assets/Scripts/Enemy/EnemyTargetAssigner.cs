using UnityEngine;
using Pathfinding;

public class EnemyTargetAssigner : MonoBehaviour
{
    private void Start()
    {
        // Find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Get the AI Destination Setter component
            AIDestinationSetter aiDestinationSetter = GetComponent<AIDestinationSetter>();

            if (aiDestinationSetter != null)
            {
                // Assign the player as the target
                aiDestinationSetter.target = player.transform;
            }
            else
            {
                Debug.LogWarning($"AIDestinationSetter not found on {gameObject.name}");
            }
        }
        else
        {
            Debug.LogError("Player not found in the scene. Make sure the player has the 'Player' tag assigned.");
        }
    }
}
