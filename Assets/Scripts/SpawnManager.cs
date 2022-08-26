using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager SingletonInstance { get; private set; }

    private float SpawnFoodTimer;
    private float SpawnPowerupTimer;
    private float CooldownTimer;

    public GameObject FoodPrefabMassGainer;
    public GameObject FoodPrefabMassBurner;

    public GameObject ShieldPrefabPowerup;
    public GameObject ScorePrefabPowerup;
    public GameObject SpeedPrefabPowerup;

    public PlayerController playerController;

    public BoxCollider2D SpawnArea;

    public int MinSnakeLength;
    public float spawnFoodTimer = 3f;
    public float destroyMassGainerTimer = 8f;
    public float destroyMassBurnerTimer = 6f;
    public float spawnPowerupTimer = 5f;
    public float cooldownTimer = 3f;

    private void Awake()
    {
        CreateOrCheckSingleton();
    }

    private void Start()
    {
        SpawnFoodTimer = spawnFoodTimer;
        SpawnPowerupTimer = spawnPowerupTimer;
        CooldownTimer = 0;
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
        if (SpawnFoodTimer > 0)
        {
            SpawnFoodTimer -= Time.deltaTime;
        }
        else
        {
            SpawnFoodPublicHandler(playerController.GetSnakeTotalBound());
            SpawnFoodTimer = spawnFoodTimer;
        }
    }
    private void SpawnPowerupRepeating()
    {
        if (SpawnPowerupTimer > 0)
            SpawnPowerupTimer -= Time.deltaTime;
        else if (CooldownTimer > 0)
            CooldownTimer -= Time.deltaTime;
        else
        {
            SpawnPowerUpPublicHandler(playerController.GetSnakeTotalBound());
            SpawnPowerupTimer = spawnPowerupTimer;
            CooldownTimer = cooldownTimer;
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

    private void SpawnFood(Vector2 spawnPosition, GameObject foodPrefab, float destroyTime)
    {
        GameObject food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        Destroy(food, destroyTime);
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
            // float ProbabilityArea4MassGainer = 0.6f;
            if (Random.value >= 0.4f)
            {
                SpawnFood(RandomSpawnPosition(snakeBound), FoodPrefabMassGainer, destroyMassGainerTimer);
            }
            // ProbabilityArea4MassBurner = any value less than 0.4f
            else
            {
                SpawnFood(RandomSpawnPosition(snakeBound), FoodPrefabMassBurner, destroyMassBurnerTimer);
            }
        }
        else
        {
            SpawnFood(RandomSpawnPosition(snakeBound), FoodPrefabMassGainer, destroyMassGainerTimer);
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
        switch (randomInt)
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