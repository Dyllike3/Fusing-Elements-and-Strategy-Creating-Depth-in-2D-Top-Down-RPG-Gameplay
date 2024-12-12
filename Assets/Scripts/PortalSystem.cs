using System.Collections;
using UnityEngine;

public class PortalSystem : MonoBehaviour
{
    public Transform targetPortal; // The portal to teleport the player to
    public AreaManager currentArea; // The area this portal belongs to
    public AreaManager targetArea; // The area the target portal belongs to
    public bool isExitPortal; // Is this the exit portal?
    private bool isExitUnlocked = false; // Tracks if the blue portal is unlocked
    public float unlockDuration = 60f; // Time in seconds to unlock the exit portal

    private void Start()
    {
        // If this is the exit portal, initially lock it
        if (isExitPortal)
        {
            isExitUnlocked = false;
            StartCoroutine(UnlockExitPortalAfterTime());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isExitPortal && isExitUnlocked)
            {
                // Deactivate the current area
                currentArea?.DeactivateArea();

                // Activate the target area
                targetArea?.ActivateArea();
                // Teleport the player to the target portal
                collision.transform.position = targetPortal.position;
                Debug.Log($"Player teleported to: {targetPortal.name}");
            }
            else if (!isExitPortal)
            {
                Debug.Log($"Entered through entry portal: {gameObject.name}");
            }
            else
            {
                Debug.Log($"Portal '{gameObject.name}' is locked. Unlock in {unlockDuration} seconds.");
            }
        }
    }

    private IEnumerator UnlockExitPortalAfterTime()
    {
        yield return new WaitForSeconds(unlockDuration); // Wait for the specified duration
        isExitUnlocked = true; // Unlock the exit portal
        Debug.Log("Exit portal unlocked!");
    }
}

