using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action<BirdState> OnBirdStateChange;

    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private float jumpForce = 5.0f;

    [SerializeField]
    private LayerMask CollisionMask;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private BirdState currentBirdState;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        SetNewState(BirdState.Idle);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (currentBirdState == BirdState.Dead)
                SetNewState(BirdState.Idle);
            else if (currentBirdState == BirdState.Idle)
                SetNewState(BirdState.Flying);

            if(currentBirdState == BirdState.Flying)
                Jump();
        }
    }

    private void SetNewState(BirdState state)
    {
        currentBirdState = state;
        OnBirdStateChange?.Invoke(state);

        switch(state)
        {
            case BirdState.Idle:
                rigidBody.simulated = false;
                rigidBody.linearVelocityY = 0;
                animator.SetTrigger("Reset");
                animator.SetBool("isFlying", false);
                transform.position = startPosition;
                transform.rotation = startRotation;
                AudioManager.Instance.PlayResetClip();
                break;

            case BirdState.Flying:
                rigidBody.simulated = true;
                animator.SetBool("isFlying", true);
                break;

            case BirdState.Dead:
                animator.SetTrigger("Died");
                AudioManager.Instance.PlayDeathClip();
                break;
        }
    }

    private void Jump()
    {
        rigidBody.linearVelocityY = jumpForce;
        AudioManager.Instance.PlayJumpClip();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentBirdState == BirdState.Dead)
            return;

        var collisionShouldStopBird = (CollisionMask & (1 << collision.gameObject.layer)) != 0;

        //Return early if the collision wasn't important
        if (!collisionShouldStopBird)
            return;

        SetNewState(BirdState.Dead);
    }
}
