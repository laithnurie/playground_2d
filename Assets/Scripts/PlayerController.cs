using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D collider2d;

    private enum State { idle, running, jumping, falling }

    private State playerState = State.idle;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jump = 20f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryText;

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
        anim.SetInteger("state", (int)playerState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        if (Input.GetButtonDown("Jump") && collider2d.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            playerState = State.jumping;
        }
    }

    private void AnimationState()
    {
        if (playerState == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                playerState = State.falling;
            }
        }
        else if (playerState == State.falling)
        {
            if (collider2d.IsTouchingLayers(ground))
            {
                playerState = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            playerState = State.running;
        }
        else
        {
            playerState = State.idle;
        }
    }

}
