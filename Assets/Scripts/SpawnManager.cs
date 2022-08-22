using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager SingletonInstance { get; private set; }

    private GameObject massGainer;
    private GameObject massBurner;

    public GameObject FoodPrefabMassGainer;
    public GameObject FoodPrefabMassBurner;

    public BoxCollider2D SpawnArea;

    private void Awake()
    {
        CreateOrCheckSingleton(); 
    }
     
    private void Start()
    {
        
    }

    private void CreateOrCheckSingleton()
    {
        if (SingletonInstance != null && SingletonInstance != this)
        {
            Destroy(this);
        }
        else
        {
            SingletonInstance = this;
        }
    }

    private Vector3 RandomSpawnPosition()
    {
        Bounds bounds = SpawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y);
    }

    private void SpawnFoodMassGainer(Vector2 spawnPosition)
    {
        massGainer = Instantiate(FoodPrefabMassGainer, spawnPosition, Quaternion.identity);
        Destroy(massGainer, 8f);
    }

    private void SpawnFoodMassBurner(Vector2 spawnPosition)
    {
        massBurner = Instantiate(FoodPrefabMassBurner, spawnPosition, Quaternion.identity);
        Destroy(massBurner, 8f);
    }

    public void SpawnFoodManagerPublicHandler(int snakeLength)
    {
        //Random.value >= 0.5f ? SpawnFoodMassGainer(RandomSpawnPosition()) : SpawnFoodMassBurner(RandomSpawnPosition());

        if (snakeLength > 20)
        {
            if (Random.value >= 0.5f)
            {
                SpawnFoodMassGainer(RandomSpawnPosition());
            }
            else
            {
                SpawnFoodMassBurner(RandomSpawnPosition());
            }
        }
        else
        {
            SpawnFoodMassGainer(RandomSpawnPosition());
        }
    }

    //Spawn Power-ups - 3 seconds cooldown
    // Shield
    // Score Boost
    // Speed Boost

    private void SpawnShield()
    {

    }
}
