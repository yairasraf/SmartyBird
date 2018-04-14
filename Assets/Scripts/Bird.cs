using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Bird : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer render;
    public float speed = 8;
    public float jumpSpeed = 10;
    public float minYBoundry = 0.5f;
    public float maxYBoundry = 10.5f;
    public Transform deathParticleSystem;
    public Sprite[] animationSprites;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        // make the bird sleep so the game will start when we interact with the bird interface
        rigid.Sleep();
    }

    void FixedUpdate()
    {
        // return if the bird is sleeping
        if (rigid.IsSleeping())
        {
            return;
        }
        // moving 
        // same speed all the time
        //rigid.AddForce(Vector2.right * speed);
        // limiting the velocity
        //Vector2 tempVelocity = rigid.velocity;
        //tempVelocity.x = Mathf.Clamp(0, tempVelocity.x, maxSpeed);
        //rigid.velocity = tempVelocity;

        // changing only the x component of the velocity to the constant speed of the bird
        Vector2 tempVelocity = rigid.velocity;
        tempVelocity.x = speed;
        rigid.velocity = tempVelocity;

        // this is for making the bird look to the direction it is going
        //rigid.rotation = Mathf.Atan2(rigid.velocity.y, rigid.velocity.x) * Mathf.Rad2Deg;
        // checking collisions
        // TODO ADD A BETTER COLLISION CHECK
        if (rigid.position.y > maxYBoundry || rigid.position.y < minYBoundry)
        {
            Kill();
        }

        // Animation here
        render.sprite = animationSprites[(Time.frameCount / 15) % animationSprites.Length];

    }

    public void Jump()
    {
        // This is not how it should be implemented from game perspective
        // rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);

        // This is how it should be implemented from game perspective
        // this is not good because when a lot of jumps it just stays at constant upwards speed
        rigid.velocity = new Vector2(rigid.velocity.x, jumpSpeed);
    }
    public void Kill()
    {
        //Destroy(gameObject);
        Instantiate(deathParticleSystem, transform.position, Quaternion.identity);
        transform.position = Vector2.up * 5;
        rigid.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // basically losing
        Kill();
    }
}
