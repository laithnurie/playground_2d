using UnityEngine;

public class Frog : Enemy
{
    private Rigidbody2D rb;
    private Collider2D collider2d;

    private Direction currentDirection = Direction.Left;

    private enum State { idle, jumping, falling, hurt }

    private enum Direction { Left, Right }

    private State frogState = State.idle;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float jumpDistance = 10f;

    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;


    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Movement();
        if (frogState != State.hurt)
        {
            AnimationState();
        }

        anim.SetInteger("state", (int)frogState);
    }

    private void Movement()
    {
        Direction newDirection = GetNewDirection(currentDirection);
        if (newDirection != currentDirection)
        {
            currentDirection = newDirection;
            UpdateFrogDirection(currentDirection == Direction.Left);
        }
    }

    private Direction GetNewDirection(Direction currentDirection)
    {
        float frogPosition = transform.position.x;
        Direction newDirection = currentDirection;

        if (currentDirection == Direction.Left && frogPosition <= 33)
        {
            newDirection = Direction.Right;
        }
        else if (frogPosition >= 55)
        {
            newDirection = Direction.Left;
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

    // will be called from Idle animation event
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

    public override void Damage()
    {
        // you can do damage logic here if hurt or death
        frogState = State.hurt;
        rb.bodyType = RigidbodyType2D.Static;
    }

    public override int GetPoints()
    {
        return 100;
    }
}
