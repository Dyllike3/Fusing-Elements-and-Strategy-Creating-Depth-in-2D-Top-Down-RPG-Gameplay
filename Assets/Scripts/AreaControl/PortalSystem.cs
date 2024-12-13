using System.Collections;
using UnityEngine;
using Pathfinding;
using Cinemachine;

public class PortalSystem : MonoBehaviour
{
    public Transform targetPortal; // The portal to teleport the player to
    public AreaManager currentArea; // The area this portal belongs to
    public AreaManager targetArea; // The area the target portal belongs to
    public GameObject currentAstarPath; // A* pathfinding object in the current area
    public GameObject targetAstarPath; // A* pathfinding object in the target area
    public bool isExitPortal; // Is this the exit portal?
    private bool isExitUnlocked = false; // Tracks if the blue portal is unlocked
    public float unlockDuration = 60f; // Time in seconds to unlock the exit portal

    // Camera-related variables
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera
    public PolygonCollider2D currentCameraBounds; // Bounds of the current area
    public PolygonCollider2D targetCameraBounds; // Bounds of the target area

    private void Start()
    {
        // If this is the exit portal, initially lock it
        if (isExitPortal)
        {
            isExitUnlocked = false;
            StartCoroutine(UnlockExitPortalAfterTime());
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isExitPortal && isExitUnlocked)
            {
                // Deactivate the current area
                currentArea?.DeactivateArea();

                // DEactivate the current A* Path Finding
                if (currentAstarPath != null)
                {
                    currentAstarPath.SetActive(false);
                    Debug.Log("deactivate the currentAstarPath");
                }

                if (AstarPath.active != null)
                {
                    AstarPath.active = null;
                    Debug.Log("Reset A* Pathfinding singleton instance.");
                }


                // Activate the target area
                targetArea?.ActivateArea();

                // Activate the target A* Path Finding
                if (targetAstarPath != null)
                {
                    targetAstarPath.SetActive(true);

                    // Automatically scan the graph after activating it
                    AstarPath activePath = targetAstarPath.GetComponent<AstarPath>();
                    if (activePath != null)
                    {
                        activePath.Scan();
                        Debug.Log("Target A* Pathfinding graph scanned automatically.");
                    }
                    else
                    {
                        Debug.LogWarning("AstarPath component not found on target A* object.");
                    }
                }

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
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isExitPortal && isExitUnlocked)
            {
                StartCoroutine(HandleAreaTransition(collision));
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

    private IEnumerator HandleAreaTransition(Collider2D player)
    {
        // Step 1: Deactivate the current area and A* Pathfinding object
        currentArea?.DeactivateArea();
        if (currentAstarPath != null && AstarPath.active != null && AstarPath.active.gameObject == currentAstarPath)
        {
            currentAstarPath.SetActive(false);
            Debug.Log("Deactivated current A* Pathfinding object.");
        }

        yield return null;

        // Ensure the singleton instance is cleared
        if (AstarPath.active != null)
        {
            AstarPath.active = null;
            Debug.Log("Reset A* Pathfinding singleton instance.");
        }

        // Small delay to ensure proper deactivation before activation
        yield return null;

        // Step 2: Activate the target area and its A* Pathfinding object
        targetArea?.ActivateArea();
        if (targetAstarPath != null)
        {
            // Check if the current `AstarPath` instance is different from the target
            if (AstarPath.active == null || AstarPath.active.gameObject != targetAstarPath)
            {
                AstarPath.active = targetAstarPath.GetComponent<AstarPath>();
                Debug.Log("Reassigned A* Pathfinding singleton instance to target area.");
            }

            targetAstarPath.SetActive(true);

            // Automatically scan the graph for the new area
            AstarPath activePath = targetAstarPath.GetComponent<AstarPath>();
            if (activePath != null)
            {
                activePath.Scan();
                Debug.Log("Scanned target A* Pathfinding graph.");
            }
        }

        // Step 3: Teleport the player to the target portal
        player.transform.position = targetPortal.position;
        Debug.Log($"Player teleported to: {targetPortal.name}");

        // Step 4: Update the camera confiner to use the target area's bounds
        UpdateCameraBounds();
    }

    private void UpdateCameraBounds()
    {
        if (virtualCamera != null)
        {
            CinemachineConfiner confiner = virtualCamera.GetComponent<CinemachineConfiner>();
            if (confiner != null)
            {
                // Set the new bounds for the confiner
                confiner.m_BoundingShape2D = targetCameraBounds;
                confiner.InvalidatePathCache(); // Refresh the confiner's data
                Debug.Log($"Camera bounds updated to target area: {targetCameraBounds.gameObject.name}");
            }
            else
            {
                Debug.LogWarning("Cinemachine Confiner not found on the virtual camera.");
            }
        }
        else
        {
            Debug.LogWarning("Virtual Camera reference is missing.");
        }
    }



    private IEnumerator UnlockExitPortalAfterTime()
    {
        yield return new WaitForSeconds(unlockDuration); // Wait for the specified duration
        isExitUnlocked = true; // Unlock the exit portal
        Debug.Log("Exit portal unlocked!");
    }
}

