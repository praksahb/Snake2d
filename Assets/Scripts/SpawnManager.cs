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

    //public GameObject PowerupPrefabShieldSnake;
    //public GameObject PowerupPrefabScoreBoost;
    //public GameObject PowerupPrefabSpeedBoost;

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

    // ?recursive call
    private Vector3 RandomSpawnPosition(Bounds snakeBounds)
    {
        Bounds bounds = SpawnArea.bounds;
        Vector3 randomSpawnPosition;
        randomSpawnPosition.x = Random.Range(bounds.min.x, bounds.max.x);
        randomSpawnPosition.y = Random.Range(bounds.min.y, bounds.max.y);
        randomSpawnPosition.z = 0;

        return snakeBounds.Contains(randomSpawnPosition) ? RandomSpawnPosition(snakeBounds) : randomSpawnPosition;
    }


    private GameObject SpawnFoodMassGainer(Vector2 spawnPosition)
    {
        massGainer = Instantiate(FoodPrefabMassGainer, spawnPosition, Quaternion.identity);
        Destroy(massGainer, 8f);
        return massGainer;
    }

    private GameObject SpawnFoodMassBurner(Vector2 spawnPosition)
    {
        massBurner = Instantiate(FoodPrefabMassBurner, spawnPosition, Quaternion.identity);
        Destroy(massBurner, 8f);
        return massBurner;
    }

    public void SpawnFoodManagerPublicHandler(List<GameObject> snakeArrayList)
    {
        Bounds snakeBound = SnakeTotalBound(snakeArrayList);

        if (snakeArrayList.Count > 20)
        {
            if (Random.value >= 0.5f)
            {
                SpawnFoodMassGainer(RandomSpawnPosition(snakeBound));
            }
            else
            {
                SpawnFoodMassBurner(RandomSpawnPosition(snakeBound));
            }
        }
        else
        {
            SpawnFoodMassGainer(RandomSpawnPosition(snakeBound));
        }
    }

    private Bounds SnakeTotalBound(List<GameObject> snakeArrayList)
    {
        Bounds snakeBound = new Bounds();
        for (int i = 0; i < snakeArrayList.Count; i++)
            snakeBound.Encapsulate(snakeArrayList[i].GetComponent<Collider2D>().bounds);

        return snakeBound;
    }

    //Spawn Power-ups - 3 seconds cooldown
    // Shield
    // Score Boost
    // Speed Boost

    private void SpawnShield()
    {

    }
}
