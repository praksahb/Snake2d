using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager SingletonInstance { get; private set; }

    private GameObject massGainer;
    private GameObject massBurner;

    private float shieldCooldownTimer;
    private float scoreCooldownTimer;
    private float speedCooldownTimer;

    public GameObject FoodPrefabMassGainer;
    public GameObject FoodPrefabMassBurner;

    public GameObject ShieldPrefabPowerup;
    public GameObject ScorePrefabPowerup;
    public GameObject SpeedPrefabPowerup;

    public BoxCollider2D SpawnArea;

    public int powerupCooldownTime = 3;

    private void Awake()
    {
        CreateOrCheckSingleton(); 
    }
     
    private void Start()
    {
        
    }

    private void Update()
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

    /* 
     * Public methods for Food Spawner
     */

    //public GameObject SpawnFoodPublicHandler(List<GameObject> snakeArrayList)
    //{
    //    Bounds snakeBound = SnakeTotalBound(snakeArrayList);

    //    if (snakeArrayList.Count > 20)
    //    {
    //        if (Random.value >= 0.5f)
    //        {
    //           return SpawnFoodMassGainer(RandomSpawnPosition(snakeBound));
    //        }
    //        else
    //        {
    //           return SpawnFoodMassBurner(RandomSpawnPosition(snakeBound));
    //        }
    //    }
    //    else
    //    {
    //        return SpawnFoodMassGainer(RandomSpawnPosition(snakeBound));
    //    }
    //}

    public GameObject SpawnFoodMassGainer(Bounds snakeBound)
    {
        return SpawnFoodMassGainer(RandomSpawnPosition(snakeBound));
    }
    public GameObject SpawnFoodMassBurner(Bounds snakeBound)
    {
        return SpawnFoodMassBurner(RandomSpawnPosition(snakeBound));
    }

    //private Bounds SnakeTotalBound(List<GameObject> snakeArrayList)
    //{
    //    Bounds snakeBound = new Bounds();
    //    for (int i = 0; i < snakeArrayList.Count; i++)
    //        snakeBound.Encapsulate(snakeArrayList[i].GetComponent<Collider2D>().bounds);

    //    return snakeBound;
    //}

    //Spawn Power-ups - 3 seconds cooldown
    // Shield
    // Score Boost
    // Speed Boost

    private GameObject SpawnPowerup(Vector2 spawnPosi, GameObject powerUpPrefab)
    {
        GameObject powerUp = Instantiate(powerUpPrefab, spawnPosi, Quaternion.identity);
        Destroy(powerUp, 8f);
        return powerUp;
    }

    /* Public Handlers - Power ups
     */

    public GameObject PowerupShieldBoost(Bounds snakeBound)
    {
        return SpawnPowerup(RandomSpawnPosition(snakeBound), ShieldPrefabPowerup);
    }

    public GameObject PowerupScoreBoost(Bounds snakeBound)
    {
        return SpawnPowerup(RandomSpawnPosition(snakeBound), ScorePrefabPowerup);
    }
    public GameObject PowerupSpeedBoost(Bounds snakeBound)
    {
        return SpawnPowerup(RandomSpawnPosition(snakeBound), SpeedPrefabPowerup);
    }

    //public void SpawnPowerUpPublicHandler(List<GameObject> snakeListArray)
    //{
    //    Bounds snakeBound = SnakeTotalBound(snakeListArray);
    //    int randomInt = Random.Range(0, 2);
    //    switch (randomInt)
    //    {
    //        case 0:
    //            //shield
    //            if(shieldCooldownTimer <= 0)
    //            {
    //                SpawnPowerup(RandomSpawnPosition(snakeBound), shieldPowerup, ShieldPrefabPowerup);
    //                shieldCooldownTimer = powerupCooldownTime;
    //            }
    //            break;

    //        case 1:
    //            //score
    //            if (scoreCooldownTimer <= 0)
    //            {
    //                SpawnPowerup(RandomSpawnPosition(snakeBound), scorePowerup, ScorePrefabPowerup);
    //                scoreCooldownTimer = powerupCooldownTime;
    //            }
    //            break;

    //        case 2:
    //            //speed
    //            if (speedCooldownTimer <= 0)
    //            {
    //                SpawnPowerup(RandomSpawnPosition(snakeBound), speedPowerup, SpeedPrefabPowerup);
    //                speedCooldownTimer = powerupCooldownTime;
    //            }
    //            break;
    //    }
    //}
}
