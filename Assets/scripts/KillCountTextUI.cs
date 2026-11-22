using TMPro;
using UnityEngine;

public class KillCountTextUI : MonoBehaviour
{
    public KillCounterSO killCounter;
    public TMP_Text label;

    private void Awake()
    {
        if (label == null) label = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        label.text = "Kills: " + killCounter.killCount;
    }
}
