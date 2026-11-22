using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class DamageTrigger : MonoBehaviour
{

    public HealthSO health;

    public float damage = 10f;

    public string playerTag = "Player";

    public UnityEvent onDamaged;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        health.Subtract(damage);
        onDamaged.Invoke();
    }
}
