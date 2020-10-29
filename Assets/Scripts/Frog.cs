using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D collider2d;

    private bool headingLeft = true;
    private int jumpCountAttempt = 0;

    private enum State { idle, jumping, falling }

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
        
        if (collider2d.IsTouchingLayers(ground))
        {
            if(jumpCountAttempt == 50)
            {
                Jump();
                jumpCountAttempt = 0;
            }
            jumpCountAttempt++;
            
        }
        if (headingLeft)
        {
            transform.localScale = new Vector2(1, 1);
        } else
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    private void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpDistance);
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
