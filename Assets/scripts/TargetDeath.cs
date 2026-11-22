using System.Collections;
using UnityEngine;

public class TargetDeath : MonoBehaviour
{
    public KillCounterSO killCounter;
    public float dissolveDelay = 1f;
    public float dissolveDuration = 1f;

    public int hitsToKill = 3;

    private int hitsTaken;
    private bool isDead;

    public void TakeHit()
    {
        if (isDead) return;

        hitsTaken++;

        if (hitsTaken >= hitsToKill) Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log(name + " died");
        GetComponent<Collider>().enabled = false;
        StartCoroutine(Dissolve());
    }

    private IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(dissolveDelay);

        Material material = GetComponent<Renderer>().material;
        float time = 0f;

        while (time < dissolveDuration)
        {
            time += Time.deltaTime;
            material.SetFloat("_DissolveAmount", time / dissolveDuration);
            yield return null;
        }

        if (killCounter != null)
        {
            killCounter.RegisterKill(gameObject);
        }

        Destroy(gameObject);
    }
}
