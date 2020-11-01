using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D collider2d;

    private enum State { idle, running, jumping, falling, hurt }

    private State playerState = State.idle;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpDistance = 20f;
    private float recoilDistance = 8f;

    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryText;

    [SerializeField] private int points = 0;
    [SerializeField] private Text pointsText;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (playerState != State.hurt)
        {
            Movement();
        }
        
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

    private void OnCollisionEnter2D(Collision2D collisionObject)
    {
        if (collisionObject.gameObject.tag == "Enemy")
        {
            if(playerState == State.falling)
            {
                GameObject collisionGameObject = collisionObject.gameObject;
                Enemy enemy = collisionGameObject.GetComponent<Enemy>();
                enemy.Damage();
                UpdatePoints(enemy.GetPoints());
                Jump();
            } else
            {
                playerState = State.hurt;
                float collisionXPosition = collisionObject.gameObject.transform.position.x;
                float playerXPosition = transform.position.x;

                if(collisionXPosition > playerXPosition)
                {
                    // enemy to right
                    rb.velocity = new Vector2(-recoilDistance, rb.velocity.y);
                    print("to the right meh");
                } else
                {
                    // enemy to left
                    rb.velocity = new Vector2(recoilDistance, rb.velocity.y);
                    print("to the left");
                }
            }
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
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpDistance);
        playerState = State.jumping;
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
        else if (playerState == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
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

    private void UpdatePoints(int pointsToAdd)
    {
        points += pointsToAdd;
        pointsText.text = points.ToString();
    }

}
