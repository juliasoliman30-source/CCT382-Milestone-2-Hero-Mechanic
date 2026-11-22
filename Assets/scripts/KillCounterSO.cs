using UnityEngine;

public class KillCounterSO : ScriptableObject
{
    public int killCount;
    public AudioClip killSound;
    public float minPitch = 1.0f;
    public float maxPitch = 1.6f;

    private void OnEnable()
    {
        killCount = 0; 
    }

    public void RegisterKill(GameObject enemy)
    {

        killCount++;

        if (killSound != null)
        {
            GameObject soundObject = new GameObject("KillSound");
            AudioSource source = soundObject.AddComponent<AudioSource>();
            source.clip = killSound;
            source.pitch = Random.Range(minPitch, maxPitch);
            source.Play();

            Destroy(soundObject, killSound.length / source.pitch);
        }
        Debug.Log(enemy.name + " killed. Total kills: " + killCount);
    }
}