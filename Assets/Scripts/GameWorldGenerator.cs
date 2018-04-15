using UnityEngine;

public class GameWorldGenerator : MonoBehaviour
{

    public Transform[] randomizedObjects;
    //public Transform[] unRandomizedObjects;
    //public float screenChangeDistance = 10;

    // higher spawn rate means less objects - not so intuitive
    public float spawnDistance = 50;
    private float sumedScale = 2;
    // the distance where we last spawned objects
    private float lastSpawnPosDistance;
    // Use this for initialization
    void Start()
    {
        // at the first of the game we add the objects
        AddNextScreenObjects();
        // TODO SPAWN BASED ON DISTANCE AND NOT BASED ON TIMER
        lastSpawnPosDistance = transform.position.x;
        if ((randomizedObjects.Length & 1) == 1)
        {
            throw new System.Exception("Randomized objects length must be an even number");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromLastSpawnPos = transform.position.x;

        if (distanceFromLastSpawnPos - lastSpawnPosDistance > spawnDistance)
        {
            lastSpawnPosDistance = distanceFromLastSpawnPos;
            AddNextScreenObjects();
        }
    }

    void DeletePreviousObjects()
    {
        foreach (GameObject previousSpawnedObject in GameObject.FindGameObjectsWithTag("GeneratedWorld"))
        {
            Destroy(previousSpawnedObject);
        }
    }

    void AddNextScreenObjects()
    {
        DeletePreviousObjects();
        // we need to spawn the unrandomized objects
        //foreach (Transform unRandomizedObject in unRandomizedObjects)
        //{
        //    Instantiate(unRandomizedObject, unRandomizedObject.position, unRandomizedObject.rotation);
        //}

        // we need to randomize the objects scale
        //foreach (Transform randomizedObject in randomizedObjects)
        //{
        //    Instantiate(randomizedObject, randomizedObject.position, randomizedObject.rotation);
        //}


        // TODO BETTER SPAWNING

        for (int i = 0; i < randomizedObjects.Length; i += 2)
        {
            Transform upperWall = Instantiate(randomizedObjects[i], new Vector2(transform.position.x + (((i + 2) * (spawnDistance)) / randomizedObjects.Length), randomizedObjects[i].position.y), randomizedObjects[i].rotation);
            Transform floorWall = Instantiate(randomizedObjects[i + 1], new Vector2(transform.position.x + (((i + 2) * (spawnDistance)) / randomizedObjects.Length), randomizedObjects[i + 1].position.y), randomizedObjects[i + 1].rotation);

            float randomScaleAdditionNumber = Random.value * sumedScale;
            // now we have a number between 0 and sumedScale

            upperWall.localScale = new Vector3(1, randomScaleAdditionNumber, 1);
            // scaling the object using the completion of the random value to the sumedScale to make sure there is place for the bird to fly through
            floorWall.localScale = new Vector3(1, sumedScale - randomScaleAdditionNumber, 1);
        }

    }
}
