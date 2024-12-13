using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellData
{
    public string spellName;
    public string fancyName;
    public GameObject prefab;
    public string mainElement; // Primary element for interactions
}

public class SpellBook : MonoBehaviour
{
    public List<SpellData> spells; // List of all available spells

    private Dictionary<string, SpellData> spellDictionary = new Dictionary<string, SpellData>();

    private void Awake()
    {
        // Build dictionary for fast lookup
        foreach (var spell in spells)
        {
            string key = GenerateKey(spell.spellName);
            if (!spellDictionary.ContainsKey(key))
            {
                spellDictionary.Add(key, spell);
            }
            else
            {
                Debug.LogWarning($"Duplicate spell key detected: {key}. Skipping.");
            }
        }
    }

    public SpellData GetSpell(List<string> selectedElements)
    {
        string key = GenerateKey(selectedElements);
        if (spellDictionary.TryGetValue(key, out SpellData spell))
        {
            return spell;
        }
        Debug.LogWarning($"No spell found for key: {key}");
        return null;
    }

    private string GenerateKey(List<string> elements)
    {
        elements.Sort(); // Ensure consistent order
        return string.Join("_", elements);
    }

    private string GenerateKey(string spellName)
    {
        return spellName; // Use spell name directly for storage
    }
}
