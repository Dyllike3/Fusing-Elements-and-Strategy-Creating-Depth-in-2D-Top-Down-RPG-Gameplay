using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class AreaEntry
    {
        public Transform entryPortal; // The entry portal for the area
        public AreaManager areaManager; // The manager for the area
        public GameObject astarPath; // A* Pathfinding object for this area
    }

    public List<AreaEntry> areas; // List of all areas with their entry portals
    public GameObject player; // Reference to the player object

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player GameObject not found. Ensure the player has the 'Player' tag assigned.");
            }
        }
    }

    private void Start()
    {
        AssignPlayerToRandomArea();
        Debug.Log("111");
    }

    public void AssignPlayerToRandomArea()
    {
        if (areas == null || areas.Count == 0 || player == null)
        {
            Debug.LogError("Areas list or player reference is not set!");
            return;
        }

        // Select a random area
        Debug.Log("222");
        int randomIndex = Random.Range(0, areas.Count);
        Debug.Log("333");
        AreaEntry selectedArea = areas[randomIndex];
        Debug.Log($"Random index selected: {randomIndex}, Area: {selectedArea.areaManager.name}");
        Debug.Log(areas.Count);
        Debug.Log(areas);

        // Deactivate all other areas and their pathfinding systems
        foreach (var area in areas)
        {
            Debug.Log(area + "What?");
            area.areaManager?.DeactivateArea();
            if (area.astarPath != null)
            {
                area.astarPath.SetActive(false);
            }
            Debug.Log("Deactivated");
        }

        // Activate the selected area
        selectedArea.areaManager?.ActivateArea();
        if (selectedArea.astarPath != null)
        {
            selectedArea.astarPath.SetActive(true);

            // Scan the A* Pathfinding graph
            AstarPath activePath = selectedArea.astarPath.GetComponent<AstarPath>();
            if (activePath != null)
            {
                Debug.Log($"Scanning pathfinding graph for {selectedArea.areaManager.name}");
                activePath.Scan();
            }
        }

        // Teleport the player to the entry portal
        if (selectedArea.entryPortal != null)
        {
            player.transform.position = selectedArea.entryPortal.position;
            Debug.Log($"Player assigned to area: {selectedArea.areaManager.name}, portal: {selectedArea.entryPortal.name}");
        }
        else
        {
            Debug.LogWarning($"Entry portal for area {selectedArea.areaManager.name} is not set.");
        }
    }


}
