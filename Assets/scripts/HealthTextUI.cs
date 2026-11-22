using TMPro;
using UnityEngine;

public class HealthTextUI : MonoBehaviour
{
    public HealthSO health;

    public TMP_Text label;

    private void Awake()
    {
        if (label == null) label = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (health == null || label == null) return;
        label.text = "Health: " + health.currentHealth;
    }
}
