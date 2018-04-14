using UnityEngine;

public class GameWorldGenerator : MonoBehaviour
{

    public Transform[] randomizedObjects;
    //public Transform[] unRandomizedObjects;
    //public float screenChangeDistance = 10;

    // higher spawn rate means less objects - not so intuitive
    public float spawnRate = 2;
    private float sumedScaleAddition = 1;
    // Use this for initialization
    void Start()
    {
        // TODO SPAWN BASED ON DISTANCE AND NOT BASED ON TIMER
        InvokeRepeating("AddNextScreenObjects", 0, spawnRate);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddNextScreenObjects()
    {
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
        Transform upperWall = Instantiate(randomizedObjects[0], new Vector2(transform.position.x + 10, randomizedObjects[0].position.y), randomizedObjects[0].rotation);
        Transform floorWall = Instantiate(randomizedObjects[1], new Vector2(transform.position.x + 10, randomizedObjects[1].position.y), randomizedObjects[1].rotation);

        float randomScaleAdditionNumber = Random.value * 2;
        // now we have a number between 0 and 2

        upperWall.localScale = new Vector3(1, randomScaleAdditionNumber, 1);
        // scaling the object using the completion of the random value to the sumedScale to make sure there is place for the bird to fly through
        floorWall.localScale = new Vector3(1, 2 - randomScaleAdditionNumber, 1);

    }
}
