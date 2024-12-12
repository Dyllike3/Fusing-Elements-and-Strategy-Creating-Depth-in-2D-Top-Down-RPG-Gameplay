using UnityEngine;

public class ElementBall : MonoBehaviour
{
    public string elementName; // Element name (e.g., Metal, Wood)
    public GameObject highlightEffect; // Effect for highlighting the ball

    private void Start()
    {
        // Ensure highlightEffect is off at the start
        if (highlightEffect != null)
        {
            highlightEffect.SetActive(false);
        }
    }

    public void Highlight(bool state)
    {
        // Activate or deactivate the child GameObject for the highlight effect
        if (highlightEffect != null)
        {
            highlightEffect.SetActive(state);
        }
    }
}
