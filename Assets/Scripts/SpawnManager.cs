using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager SingletonInstance { get; private set; }

    public int MinSnakeLength;

    //private float shieldCooldownTimer;
    //private float scoreCooldownTimer;
    //private float speedCooldownTimer;

    public GameObject FoodPrefabMassGainer;
    public GameObject FoodPrefabMassBurner;

    public GameObject ShieldPrefabPowerup;
    public GameObject ScorePrefabPowerup;
    public GameObject SpeedPrefabPowerup;

    public PlayerController playerController;

    public BoxCollider2D SpawnArea;

    //public int powerupCooldownTime = 3;
    public float spawnFoodTimer = 3f;
    public float spawnPowerupTimer = 5f;
    private void Awake()
    {
        CreateOrCheckSingleton();
    }

    private void Start()
    {
    }

    private void Update()
    {
        SpawnFoodRepeating();
        SpawnPowerupRepeating();
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

    private void SpawnFoodRepeating()
    {
        if (spawnFoodTimer > 0)
        {
            spawnFoodTimer -= Time.deltaTime;
        }
        else
        {
            SpawnFoodPublicHandler(playerController.GetSnakeTotalBound());
            spawnFoodTimer = 3f;
        }
    }
    private void SpawnPowerupRepeating()
    {
        if (spawnPowerupTimer > 0)
        {
            spawnPowerupTimer -= Time.deltaTime;
        }
        else
        {
            SpawnPowerUpPublicHandler(playerController.GetSnakeTotalBound());
            spawnPowerupTimer = 5f;
        }
    }

    // Always return a random x,y position values in SpawnArea 
    // and tries not to spawn on snake body position
    private Vector3 RandomSpawnPosition(Bounds snakeBounds)
    {
        Bounds bounds = SpawnArea.bounds;
        Vector3 randomSpawnPosition;
        randomSpawnPosition.x = Random.Range(bounds.min.x, bounds.max.x);
        randomSpawnPosition.y = Random.Range(bounds.min.y, bounds.max.y);
        randomSpawnPosition.z = 0;

        return snakeBounds.Contains(randomSpawnPosition) ? RandomSpawnPosition(snakeBounds) : randomSpawnPosition;
    }

    private void SpawnFood(Vector2 spawnPosition, GameObject foodPrefab)
    {
        GameObject food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        Destroy(food, 8f);
    }
    private void SpawnPowerup(Vector2 spawnPosi, GameObject powerUpPrefab)
    {
        GameObject powerUp = Instantiate(powerUpPrefab, spawnPosi, Quaternion.identity);
        Destroy(powerUp, 8f);
    }

    /* 
     * Public methods for Food Spawner
     */

    public void SpawnFoodPublicHandler(Bounds snakeBound)
    {
        if (playerController.GetSnakeLength() > MinSnakeLength)
        {
            // Random.value returns float value from 0 till 1
            // float HalfProbabilityArea = 0.5f;
            if (Random.value >= 0.5f)
            {
                SpawnFood(RandomSpawnPosition(snakeBound), FoodPrefabMassGainer);
            }
            else
            {
                SpawnFood(RandomSpawnPosition(snakeBound), FoodPrefabMassBurner);
            }
        }
        else
        {
            SpawnFood(RandomSpawnPosition(snakeBound), FoodPrefabMassGainer);
        }
    }

    //Spawn Power-ups - 3 seconds cooldown
    // Shield
    // Score Boost
    // Speed Boost

    /* Public Handlers - Power ups
     */

    public void SpawnPowerUpPublicHandler(Bounds snakeBound)
    {
        int randomInt = Random.Range(0, 2);
        //editing here
        // using 0 till other power ups are made
        switch (2)
        {
            case 0:
                //shield
                SpawnPowerup(RandomSpawnPosition(snakeBound), ShieldPrefabPowerup);
                //shieldCooldownTimer = powerupCooldownTime;
                break;

            case 1:
                //score
                SpawnPowerup(RandomSpawnPosition(snakeBound), ScorePrefabPowerup);
                //scoreCooldownTimer = powerupCooldownTime;
                break;

            case 2:
                //speed
                SpawnPowerup(RandomSpawnPosition(snakeBound), SpeedPrefabPowerup);
                //speedCooldownTimer = powerupCooldownTime;
                break;
        }
    }
}
