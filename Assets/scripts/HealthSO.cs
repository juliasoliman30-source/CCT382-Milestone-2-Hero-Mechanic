using UnityEngine;

public class HealthSO : ScriptableObject
{
    [SerializeField] private float startingHealth = 100f;

    public float currentHealth = 100f;

    private void OnEnable()
    {
        currentHealth = startingHealth;
    }

    public void Subtract(float amount)
    {
        float before = currentHealth;
        currentHealth = Mathf.Max(0f, currentHealth - amount);
        Debug.Log($"[HealthSO] {name}: health {before} -> {currentHealth} (subtracted {amount})");
    }
}
