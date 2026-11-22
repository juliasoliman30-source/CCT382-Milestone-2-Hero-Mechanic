using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MeleeAttack : MonoBehaviour
{
    public float radius = 1f;
    public float forwardOffset = 1f;
    public float heightOffset = 1f;
    public LayerMask targetLayer;
    public Animator animator;
    public AudioSource swingSound;

    private void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        bool clicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool pressedF = Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame;

        if (clicked || pressedF)
        {
            animator.SetTrigger("Attack");

            if (swingSound != null)
            {
                swingSound.Play();
            }

            Attack();
        }
    }

    private void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(HitCenter(), radius, targetLayer);

        List<TargetDeath> alreadyHit = new List<TargetDeath>();

        foreach (Collider hit in hits)
        {

            TargetDeath target = hit.GetComponentInParent<TargetDeath>();
            if (target != null && !alreadyHit.Contains(target))
            {
                alreadyHit.Add(target);
                target.TakeHit();
            }
        }
    }

    private Vector3 HitCenter()
    {
        return transform.position + transform.forward * forwardOffset + Vector3.up * heightOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(HitCenter(), radius);
    }
}