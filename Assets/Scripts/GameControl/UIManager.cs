using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider healthBar; // Reference to the health bar slider
    public Text exitTimerText; // Reference to the exit timer text
    public Text currentElementText; // Reference to the current element text
    public Text survivalTimerText; // Reference to the survival timer text

    private PlayerController playerController; // Reference to the player's health
    private float survivalTime = 0f; // Tracks how long the player has lived
    private AreaManager currentAreaManager; // Reference to the current area

    private PortalSystem exitPortal; // Reference to the exit portal's unlock timer

    private void Start()
    {
        // Find the player and assign references
        playerController = FindObjectOfType<PlayerController>();

        // Find the current area's AreaManager
        currentAreaManager = FindObjectOfType<AreaManager>();

        // Find the exit portal in this area
        PortalSystem[] portals = FindObjectsOfType<PortalSystem>();
        foreach (var portal in portals)
        {
            if (portal.isExitPortal)
            {
                exitPortal = portal;
                break;
            }
        }
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateExitTimer();
        UpdateCurrentElement();
        UpdateSurvivalTimer();
    }

    private void UpdateHealthBar()
    {
        if (playerController != null)
        {
            healthBar.value = playerController.GetHealthPercentage();
        }
    }

    private void UpdateExitTimer()
    {
        if (exitPortal != null && exitPortal.isExitPortal)
        {
            float remainingTime = Mathf.Max(0, exitPortal.unlockDuration - Time.timeSinceLevelLoad);
            exitTimerText.text = $"Exit Opens In: {remainingTime:F1} seconds";
        }
    }

    private void UpdateCurrentElement()
    {
        if (currentAreaManager != null)
        {
            currentElementText.text = $"Current Element: {currentAreaManager.areaElement}";
        }
    }

    private void UpdateSurvivalTimer()
    {
        survivalTime += Time.deltaTime;
        survivalTimerText.text = $"Survival Time: {survivalTime:F1} seconds";
    }
}
