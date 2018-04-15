using System.Collections.Generic;
using UnityEngine;

public class GameWorldGenerator : MonoBehaviour
{
    // TODO IMPORTANT - MAKE THE WHOLE WORLD MOVE AND THE BIRD STAY AT THEIR PLACE, GOOD FOR PERFORMANCE AND ENDLESS MAYBE


    public static GameWorldGenerator instance; // singleton for this class
    public Transform upperWall;
    public Transform floorWall;
    // higher spawn rate means more objects per distance
    public float spawnRate = 0.1f;
    public float spawnOffsetFromPosition = 50;
    public int maxObjectsSpawned = 30;
    public Queue<Transform> spawnedObjectsQueue;
    public Transform oldestSpawnedUpperWall;
    public Transform oldestSpawnedFloorWall;


    private float sumedScale = 2;
    // the distance where we last spawned objects
    private float lastSpawnPosDistance;
    // Use this for initialization
    void Start()
    {
        // singleton handeling
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // validating input amount
        if (maxObjectsSpawned <= 2)
        {
            throw new System.Exception("Error, max objects spawned must be larger than 2");
        }


        // creating the queue for the spawned objects to destroy them later
        spawnedObjectsQueue = new Queue<Transform>(maxObjectsSpawned);

        // storing the starting position x
        lastSpawnPosDistance = transform.position.x;

        // at the first of the game we add the objects
        AddNextScreenObjects();

        // AT THE START OF THE GAME WE ALSO NEED TO SPAWN SOME AT THE START A FEW WALLS


    }

    // Update is called once per frame
    void Update()
    {
        if (spawnRate == 0)
        {
            // we can not have a spawn rate zero because it would be divison by zero
            return;
        }
        float distanceFromLastSpawnPos = transform.position.x;

        if (distanceFromLastSpawnPos - lastSpawnPosDistance > (1 / spawnRate))
        {
            lastSpawnPosDistance = distanceFromLastSpawnPos;
            AddNextScreenObjects();
        }
    }

    void DeletePreviousObjects()
    {

        if (spawnedObjectsQueue.Count >= maxObjectsSpawned)
        {
            Destroy(spawnedObjectsQueue.Dequeue().gameObject);
            Destroy(spawnedObjectsQueue.Dequeue().gameObject);
        }
    }

    void AddNextScreenObjects()
    {
        DeletePreviousObjects();

        // storing the spawned objects
        oldestSpawnedUpperWall = Instantiate(upperWall, new Vector2(transform.position.x + spawnOffsetFromPosition, upperWall.position.y), upperWall.rotation);
        oldestSpawnedFloorWall = Instantiate(floorWall, new Vector2(transform.position.x + spawnOffsetFromPosition, floorWall.position.y), floorWall.rotation);

        // scaling the spawned objects
        float randomScaleAdditionNumber = Random.value * sumedScale;
        // now we have a number between 0 and sumedScale

        oldestSpawnedUpperWall.localScale = new Vector3(1, randomScaleAdditionNumber, 1);
        // scaling the object using the completion of the random value to the sumedScale to make sure there is place for the bird to fly through
        oldestSpawnedFloorWall.localScale = new Vector3(1, sumedScale - randomScaleAdditionNumber, 1);


        // enqueue the objects in the queue of destroying
        spawnedObjectsQueue.Enqueue(oldestSpawnedUpperWall);
        spawnedObjectsQueue.Enqueue(oldestSpawnedFloorWall);

    }
}
