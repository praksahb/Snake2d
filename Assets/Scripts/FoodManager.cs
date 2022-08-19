using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public static FoodManager SingletonInstance { get; private set; }

    private GameObject massGainer;

    public GameObject FoodPrefab;

    public BoxCollider2D SpawnArea;

    private void Awake()
    {
        CreateOrCheckSingleton(); 
    }
     
    private void Start()
    {
         //RandomizeSpawnLocation();
        InvokeRepeating("SpawnFoodAtRandomLocation", 2f, 8f);
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

    private void SpawnFoodAtRandomLocation()
    {
        SpawnFood(SpawnPosition());
    }

    private Vector3 SpawnPosition()
    {
        Bounds bounds = SpawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y);
    }

    private void SpawnFood(Vector2 spawnPosition)
    {
        massGainer = Instantiate(FoodPrefab, spawnPosition, Quaternion.identity);
        Destroy(massGainer, 8f);
    }
}
