using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D collider2d;

    private Direction currentDirection = Direction.Left;
    private int jumpCountAttempt = 0;

    private enum State { idle, jumping, falling }

    private enum Direction { Left, Right }

    private State frogState = State.idle;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float jumpDistance = 10f;

    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Movement();
        AnimationState();

        anim.SetInteger("state", (int)frogState);
    }

    private void Movement()
    {
        Direction newDirection = GetNewDirection(currentDirection);
        if(newDirection != currentDirection)
        {
            currentDirection = newDirection;
            UpdateFrogDirection(currentDirection == Direction.Left);
        }

        if (collider2d.IsTouchingLayers(ground))
        {

            if (jumpCountAttempt == 50)
            {
                Jump();
                jumpCountAttempt = 0;
            }
            jumpCountAttempt++;

        }
    }

    private Direction GetNewDirection(Direction currentDirection)
    {
        float frogPosition = transform.position.x;
        Direction newDirection = currentDirection;

        if (currentDirection == Direction.Left)
        {
            if (frogPosition <= 33)
            {
                newDirection = Direction.Right;
            } else
            {
                newDirection = Direction.Left;
            }
        } else
        {
            if(frogPosition >= 66)
            {
                newDirection = Direction.Right;
            } else
            {
                newDirection = Direction.Left;
            } 
        }
        return newDirection;
    }

    private void UpdateFrogDirection(bool newDirection)
    {
        if (newDirection)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    private void Jump()
    {
        int distance = -5;
        if (currentDirection == Direction.Right)
        {
            distance = 5;
        }
        rb.velocity = new Vector2(rb.velocity.x + distance, jumpDistance);
        frogState = State.jumping;
    }

    private void AnimationState()
    {
        if (frogState == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                frogState = State.falling;
            }
        }
        else if (frogState == State.falling)
        {
            if (collider2d.IsTouchingLayers(ground))
            {
                frogState = State.idle;
            }
        }
        else
        {
            frogState = State.idle;
        }
    }
}
