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
    // TODO IMPLEMENT THIS BOOLEAN IS JUMPING
    // public bool isJumping;
    public Transform deathParticleSystem;
    public Sprite[] animationSprites;
    public int animationFrameRate = 7;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        // make the bird sleep so the game will start when we interact with the bird interface
        // rigid.Sleep();

        // adding the bird to the game manager so it could track the amount of current birds
        // making the game manager track that this bird is alive
        GameManager.instance.IncCurrentlyAliveBirdsCounter();

        // adding this gameobject to the camera to follow
        CameraFollow.targets.Add(this.gameObject);
    }

    void FixedUpdate()
    {
        // return if the bird is sleeping
        if (rigid.IsSleeping())
        {
            return;
        }
        // moving
        // changing only the x component of the velocity to the constant speed of the bird
        Vector2 tempVelocity = rigid.velocity;
        tempVelocity.x = speed;
        rigid.velocity = tempVelocity;

        // this is for making the bird look to the direction it is going
        rigid.rotation = Mathf.Atan2(rigid.velocity.y, rigid.velocity.x) * Mathf.Rad2Deg;
        // checking collisions
        if (rigid.position.y > maxYBoundry || rigid.position.y < minYBoundry)
        {
            Kill();
        }

        // Animation here
        render.sprite = animationSprites[(Time.frameCount / animationFrameRate) % animationSprites.Length];

    }

    public void Jump()
    {
        // This is how it should be implemented from game perspective
        // this is not good because when a lot of jumps it just stays at constant upwards speed
        rigid.velocity = new Vector2(rigid.velocity.x, jumpSpeed);
    }
    public void Kill()
    {
        // TODO CHANGE THIS CHECK OF WHO IS THE WINNER

        // if we are the last bird alive we should check if we are player or AI
        if (GameManager.instance.amountOfBirdsAlive == 1)
        {
            if (this.GetComponent<BirdPlayer>())
            {
                ScoreManager.AddPointToPlayer();
            }
            else
            {
                ScoreManager.AddPointToAI();
            }
        }
        GameManager.instance.DecCurrentlyAliveBirdsCounterAndCheckForEndRound();

        // checking if we our an evolutional AI Bird, if we are then call its kill function as well
        BirdEvolutionAI birdEvoAI = this.GetComponent<BirdEvolutionAI>();
        if (birdEvoAI)
        {
            birdEvoAI.KillEvolutionAIBird();
        }

        // removing this gameobject to the camera to stop following it
        CameraFollow.targets.Remove(this.gameObject);

        // instantiating the death explosion effect
        Instantiate(deathParticleSystem, transform.position, transform.rotation);
        // destory this bird gameobject
        Destroy(this.gameObject);
    }

    public Rigidbody2D GetRigid()
    {
        return this.rigid;
    }

    public float Score()
    {
        // a simple score function, based on the x axis of the object
        return this.transform.position.x;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // basically losing
        Kill();
    }

}
