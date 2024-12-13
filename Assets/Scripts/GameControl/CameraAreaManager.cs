using UnityEngine;
using Cinemachine;

public class CameraAreaManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Reference to the virtual camera
    private CinemachineConfiner confiner;

    public PolygonCollider2D[] areaBounds; // Array of bounding shapes for each area

    private void Start()
    {
        // Get the Cinemachine Confiner component
        confiner = virtualCamera.GetComponent<CinemachineConfiner>();
        if (confiner == null)
        {
            Debug.LogError("Cinemachine Confiner not found on the virtual camera!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Check if the collision occurred with a boundary area
            foreach (PolygonCollider2D area in areaBounds)
            {
                if (collision.otherCollider == area)
                {
                    // Update the confiner's bounding shape
                    confiner.m_BoundingShape2D = area;
                    confiner.InvalidatePathCache(); // Refresh the confiner's data
                    Debug.Log($"Camera bounds updated to area: {area.gameObject.name}");
                    break;
                }
            }
        }
    }
}

