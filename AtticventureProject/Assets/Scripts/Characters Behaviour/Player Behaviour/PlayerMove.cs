using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class PlayerMove : MonoBehaviour
{
    public static PlayerData data;
    protected Vector2 movement;

    protected Rigidbody2D rb2D;
    protected Animator animator;
    protected HealthManager health;

    private int xAnimation, yAnimation;

    virtual protected void Awake() {
        data = PlayerManager.Instance.data;
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<HealthManager>();
    }

    virtual protected void OnEnable() {}
    virtual protected void OnDisable() {}
    virtual protected void Movement(Vector2 movement) {}

    protected void HandleMovementAnimations() {
        animator.SetFloat("Speed", movement.sqrMagnitude);
        if (movement.x != 0 || movement.y != 0)
        {
            xAnimation = Mathf.RoundToInt(movement.x);
            yAnimation = Mathf.RoundToInt(movement.y);
            animator.SetFloat("Horizontal", xAnimation);
            animator.SetFloat("Vertical", yAnimation);
        }
    }
}
