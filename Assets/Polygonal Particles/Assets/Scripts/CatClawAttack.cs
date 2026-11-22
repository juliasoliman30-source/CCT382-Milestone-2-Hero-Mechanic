using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// MECHANIC: Cat Claw Swipe (Physics.OverlapSphere)
///
/// Attach this to Kitty_001 (the root object that has the movement
/// script and Player Input). Every frame, it checks whether any
/// Collider on Target Layer is inside a small invisible sphere placed
/// in front of the cat's paws (the "claw hitbox"). Press the Attack
/// Key to swipe: the name of every target hit is logged to the Console.
///
/// In the Scene view, the wire sphere is green when nothing is in
/// range and red when something is. Targets get a yellow outline.
/// If Feedback Renderer is assigned, its colour also changes live in
/// the Game view.
/// </summary>
public class CatClawAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [Tooltip("Size of the claw hitbox. Kept small because the cat is small.")]
    public float attackRadius = 0.4f;

    [Tooltip("How far in front of the cat the hitbox is centered.")]
    public float attackForwardOffset = 0.4f;

    [Tooltip("Raises the hitbox from the cat's pivot (its feet) up to paw height.")]
    public float attackHeightOffset = 0.2f;

    [Tooltip("Only objects on this layer can be hit. Do NOT include Default, or the ground will count as a hit.")]
    public LayerMask targetLayer;

    [Tooltip("Key that triggers the swipe.")]
    public Key attackKey = Key.F;

    [Header("Live Feedback (optional)")]
    [Tooltip("Drag a Renderer here (e.g. the cat's body mesh) to see a colour change in the Game view too.")]
    public Renderer feedbackRenderer;
    public Color noTargetColor = Color.white;
    public Color targetInRangeColor = Color.red;

    // Stored each frame so OnDrawGizmos can show the same
    // result Update() just calculated, instead of running the query twice.
    private Collider[] detected;
    private bool isDetecting;

    // Where the hitbox sits in world space. Used by both Update and the gizmos
    // so the drawing always matches the real detection.
    private Vector3 HitboxCenter =>
        transform.position
        + transform.forward * attackForwardOffset
        + Vector3.up * attackHeightOffset;

    private void Update()
    {
        // Ask Unity: what Colliders on targetLayer are touching an
        // invisible sphere of this radius, centered in front of the cat?
        detected = Physics.OverlapSphere(HitboxCenter, attackRadius, targetLayer);
        isDetecting = detected.Length > 0;

        if (feedbackRenderer != null)
        {
            feedbackRenderer.material.color = isDetecting ? targetInRangeColor : noTargetColor;
        }

        // The project uses the new Input System (Player Input component),
        // so the old Input.GetKeyDown would throw an error. Keyboard.current
        // can be null if no keyboard is connected, hence the check.
        if (Keyboard.current != null && Keyboard.current[attackKey].wasPressedThisFrame)
        {
            Swipe();
        }
    }

    private void Swipe()
    {
        if (!isDetecting)
        {
            Debug.Log("Cat swiped at nothing.");
            return;
        }

        foreach (Collider hit in detected)
        {
            Debug.Log("Cat clawed: " + hit.gameObject.name);
        }
    }

    // Drawn all the time (not just when selected) so the hitbox is always
    // visible while you debug the attack.
    private void OnDrawGizmos()
    {
        Gizmos.color = isDetecting ? Color.red : Color.green;
        Gizmos.DrawWireSphere(HitboxCenter, attackRadius);

        if (detected == null) return;

        Gizmos.color = Color.yellow;
        foreach (Collider hit in detected)
        {
            Gizmos.DrawWireCube(hit.bounds.center, hit.bounds.size);
        }
    }
}
