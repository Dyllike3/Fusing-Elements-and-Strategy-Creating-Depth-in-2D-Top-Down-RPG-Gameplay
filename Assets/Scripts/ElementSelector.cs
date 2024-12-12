using UnityEngine;
using System.Collections.Generic;

public class ElementSelector : MonoBehaviour
{
    public List<ElementBall> elementBalls; // Reference to all element balls
    private int currentHighlightIndex = 0;
    private List<string> selectedElements = new List<string>();
    private string primaryElement = null;
    private string lastSecondaryElement = null;

    private float highlightDelay = 0.5f; // Adjust the delay in seconds (e.g., 0.5 = half a second)
    private float highlightTimer = 0f;  // Timer to track delay

    private HashSet<string> invalidElements = new HashSet<string>(); // Tracks unselectable elements

    private bool thirdPress;

    public SpellBook spellBook;
    public Transform firePoint; // Position to spawn the spell prefab

    void Update()
    {
        highlightTimer -= Time.deltaTime;
        if (highlightTimer <= 0f)
        {
            HandleHighlightCycle();
            highlightTimer = highlightDelay; // Reset the timer
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectElement();
        }

        if (Input.GetKeyDown(KeyCode.E) && selectedElements.Count > 0)
        {
            CastSpell();
        }
    }

    private void HandleHighlightCycle()
    {
        // Turn off the current highlight
        if (currentHighlightIndex >= 0 && currentHighlightIndex < elementBalls.Count)
        {
            elementBalls[currentHighlightIndex].Highlight(false);
        }

        // Check if there are any valid elements
        bool hasValidElements = false;
        foreach (var ball in elementBalls)
        {
            if (IsBallSelectable(ball))
            {
                hasValidElements = true;
                break;
            }
        }

        if (!hasValidElements)
        {
            foreach (var ball in elementBalls)
            {
                ball.Highlight(false);
            }
            // No valid elements, stop all highlighting
            currentHighlightIndex = -1;
            Debug.Log("No valid elements remaining. Press E to cast the spell.");
            return;
        }

        // Find the next valid element
        for (int i = 0; i < elementBalls.Count; i++)
        {
            currentHighlightIndex = (currentHighlightIndex + 1) % elementBalls.Count;

            if (IsBallSelectable(elementBalls[currentHighlightIndex]))
            {
                elementBalls[currentHighlightIndex].Highlight(true); // Highlight the valid element
                return; // Exit after finding a valid highlight
            }
        }

        // If no valid elements found during cycling, stop highlighting
        currentHighlightIndex = -1;
        Debug.Log("Highlighting stopped.");
    }

    private bool IsBallSelectable(ElementBall ball)
    {
        if (thirdPress)
        {
            return false;
        }

        // Check if the ball conflicts with the last selected secondary element
        if (lastSecondaryElement != null && IsConflict(lastSecondaryElement, ball.elementName))
        {
            return false;
        }

        // Prevent selecting the same secondary element twice
        if (selectedElements.Contains(ball.elementName) && ball.elementName != primaryElement)
        {
            return false;
        }

        if (invalidElements.Contains(ball.elementName) && ball.elementName != primaryElement)
        {
            return false;
        }

        return true; // Ball is valid
    }

    private void UpdateInvalidElements()
    {
        invalidElements.Clear();

        // Mark conflict elements as invalid based on the last secondary element
        if (lastSecondaryElement != null && !thirdPress)
        {
            foreach (var ball in elementBalls)
            {
                foreach (var selectedBall in selectedElements)
                {
                    if (selectedBall != ball.elementName && IsConflict(lastSecondaryElement, ball.elementName))
                    {
                        invalidElements.Add(ball.elementName);
                    }
                }
            }
        }
        else if (thirdPress)
        {
            foreach (var ball in elementBalls)
            {
                invalidElements.Add(ball.elementName);
            }
        }
    }

    private void SelectElement()
    {
        // If no valid element is highlighted, return early
        if (currentHighlightIndex == -1 || currentHighlightIndex >= elementBalls.Count)
        {
            Debug.Log("No valid elements to select.");
            return;
        }

        ElementBall selectedBall = elementBalls[currentHighlightIndex];

        if (primaryElement == null)
        {
            // Select Primary Element
            primaryElement = selectedBall.elementName;
        }
        else
        {
            if (lastSecondaryElement == null)
            {
                // Select Secondary Element
                lastSecondaryElement = selectedBall.elementName;
            }
            else
            {
                lastSecondaryElement = null;
                thirdPress = true;
            }
        }

        selectedElements.Add(selectedBall.elementName);
        Debug.Log("Selected Elements: " + string.Join(", ", selectedElements));

        // Refresh invalid elements
        UpdateInvalidElements();

        // Check if any valid elements remain
        HandleHighlightCycle(); // Automatically stop highlights if none remain
    }

    private void CastSpell()
    {
        Debug.Log("Casting Spell: " + string.Join(" ", selectedElements));

        // Fetch spell data from the SpellBook
        SpellData spell = spellBook.GetSpell(selectedElements);

        if (spell != null)
        {
            GameObject spellPrefab = spell.prefab;

            // Get mouse position and direction
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Vector3 direction = (mousePosition - firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            // Instantiate the spell
            GameObject spellObject = Instantiate(spellPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
            spellObject.SetActive(true);

            // Configure the EnemyDamager on the instantiated spell
            EnemyDamager damager = spellObject.GetComponent<EnemyDamager>();
            if (damager != null)
            {
                damager.damageAmount = 20f; // Set default damage (adjust as needed)
                damager.shouldKnockback = false; // Example: no knockback
                damager.damageOverTime = false; // Example: no over-time damage
                damager.lifeTime = 5f; // Example lifetime
                damager.destroyOnContact = true; // Example: destroy on contact
            }

            Debug.Log($"Casted {spell.spellName}");
        }
        else
        {
            Debug.LogWarning("No spell matched the selected elements.");
        }

        ResetSelection();
    }

    private void ResetSelection()
    {
        selectedElements.Clear();
        primaryElement = null;
        lastSecondaryElement = null;
        thirdPress = false;
        invalidElements.Clear();
        currentHighlightIndex = 0;

        foreach (var ball in elementBalls)
        {
            ball.Highlight(false);
        }
    }

    private bool IsConflict(string elementA, string elementB)
    {
        Dictionary<string, string[]> conflicts = new Dictionary<string, string[]>
        {
            { "Metal", new string[] { "Wood", "Fire" } },
            { "Wood", new string[] { "Earth", "Metal" } },
            { "Water", new string[] { "Fire", "Earth" } },
            { "Fire", new string[] { "Metal", "Water" } },
            { "Earth", new string[] { "Wood", "Water" } }
        };

        if (conflicts.ContainsKey(elementA))
        {
            return System.Array.Exists(conflicts[elementA], conflict => conflict == elementB);
        }
        return false;
    }
}
