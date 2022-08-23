using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public GameObject snakeBodyPrefab;
    public BoxCollider2D BorderWrappingCollider;

    private Rigidbody2D snakeRigidBody;
    private Vector3 MoveDirectionVector;
    private Direction prevDirection, currentDirection = Direction.down;
    private float horizontal, vertical;
    private int foodEaten;

    private List<GameObject> snakeBodyList;

    private IEnumerator coroutine;
    private bool stopSpawn;
    private GameObject spawnedFoodMassGainer;
    private GameObject spawnedFoodMassBurner;

    private void Awake()
    {
        snakeRigidBody = GetComponent<Rigidbody2D>();

        AwakeHelperFunctions();
    }

    private void AwakeHelperFunctions()
    {
        snakeBodyList = new List<GameObject>();
        snakeBodyList.Add(gameObject);
        InitializeSnakeBodyList(25);
    }

    private void InitializeSnakeBodyList(int minimumSnakeLength)
    {
        for (int i = 0; i < minimumSnakeLength; i++)
        {
            GrowSnake("Initial");
        }
    }

    private void Start()
    {
        coroutine = WaitAndSpawnFood(2.5f);
        StartCoroutine(coroutine);
    }

    private void Update()
    {
        //input
        MovePlayerUpdate();
    } 
   
    private void FixedUpdate()
    {
        //physics
        MovePlayerFixedUpdate();
    }

    IEnumerator WaitAndSpawnFood(float waitTime)
    {
        while(stopSpawn != true)
        {
            SpawnFoodConditional();
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }

    private void SpawnFoodConditional()
    {
        Bounds snakeBound = SnakeTotalBound(snakeBodyList);

        if(snakeBodyList.Count > 20)
        {
            if(Random.value > 0.5f)
            {
                spawnedFoodMassGainer = SpawnManager.SingletonInstance.SpawnFoodMassGainer(snakeBound);
            } else
            {
                spawnedFoodMassBurner = SpawnManager.SingletonInstance.SpawnFoodMassBurner(snakeBound);
            }
        }
    }

    private Bounds SnakeTotalBound(List<GameObject> snakeArrayList)
    {
        Bounds snakeBound = new Bounds();
        for (int i = 0; i < snakeArrayList.Count; i++)
            snakeBound.Encapsulate(snakeArrayList[i].GetComponent<Collider2D>().bounds);

        return snakeBound;
    }

    private void MovePlayerFixedUpdate()
    {
        SetupTransformViaDirection();
        SnakeBodyFollowsHead();
    }

    private void MovePlayerUpdate()
    {
        SetupDirectionViaInputProvided();
        GetInputMovement();
        BorderWrapAround();
    }

    private void SnakeBodyFollowsHead()
    {
        // loop in reverse order.
        // n item gets position of n - 1 item
        //only x,y values.  z values remain same for each item
        for (int i = snakeBodyList.Count - 1; i > 0; i--)
        {
            //get x, y values only for snake body
            Vector2 prevPosition = snakeBodyList[i-1].transform.position;
            snakeBodyList[i].transform.position = new Vector3(prevPosition.x, prevPosition.y, snakeBodyList[i].transform.position.z);
        }
    }
    
    private void BorderWrapAround()
    {

        Bounds bounds = BorderWrappingCollider.bounds;
                
        if (transform.position.x <= bounds.min.x && currentDirection == Direction.left)
        {
            ScreenWrappingOnXAxis();
        }       
        if (transform.position.y >= bounds.max.y && currentDirection == Direction.up)
        {
            ScreenWrappingOnYAxis();
        }
        if (transform.position.x >= bounds.max.x && currentDirection == Direction.right)
        {
            ScreenWrappingOnXAxis();
        }

        if (transform.position.y <= bounds.min.y && currentDirection == Direction.down)
        {
            ScreenWrappingOnYAxis();
        }
    }

    private void ScreenWrappingOnXAxis()
    {
        Vector3 position = transform.position;
        position.x = position.x * -1;
        transform.position = position;
    }

    private void ScreenWrappingOnYAxis()
    {
        Vector3 position = transform.position;
        position.y = position.y * -1;
        transform.position = position;
    }

    private void GetInputMovement()
    {
        //take input for direction
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void SetupDirectionViaInputProvided()
    {
        if (horizontal == 1 && prevDirection != Direction.left)
        {
            prevDirection = currentDirection;
            currentDirection = Direction.right;
        }
        if (horizontal == -1 && prevDirection != Direction.right)
        {
            prevDirection = currentDirection;
            currentDirection = Direction.left;
        }
        if (vertical == 1 && prevDirection != Direction.down)
        {
            prevDirection = currentDirection;
            currentDirection = Direction.up;

        }
        if (vertical == -1 && prevDirection != Direction.up)
        {
            prevDirection = currentDirection;
            currentDirection = Direction.down;
        }
    }

    // top-down feel = -90, 180, 90, 0
    // ?sideways feel = 0, 90, 0, 90

    private void SetupTransformViaDirection()
    {
        float rotateImageLeftByEulerAngle = -90f;
        float rotateImageUpwardsByEulerAngle = 180f;
        float rotateImageRightByEulerAngle = 90f;
        float rotateImageDownwardsByEulerAngle = 0f;

        if (currentDirection == Direction.left)
        {
            TransformModifyViaDirection(new Vector2(-1, 0));
            transform.rotation = Quaternion.Euler(0f, 0f, rotateImageLeftByEulerAngle);
        }
        if (currentDirection == Direction.up)
        {
            TransformModifyViaDirection(new Vector2(0, 1));
            transform.rotation = Quaternion.Euler(0f, 0f, rotateImageUpwardsByEulerAngle);
        }
        if (currentDirection == Direction.right)
        {
            TransformModifyViaDirection(new Vector2(1, 0));
            transform.rotation = Quaternion.Euler(0f, 0f, rotateImageRightByEulerAngle);
        }
        if (currentDirection == Direction.down)
        {
            TransformModifyViaDirection(new Vector2(0, -1));
            transform.rotation = Quaternion.Euler(0f, 0f, rotateImageDownwardsByEulerAngle);
        }
    }

    private void TransformModifyViaDirection(Vector2 directionValue)
    {
        Vector3 position = transform.position;
        MoveDirectionVector = new Vector3(MoveSpeed * directionValue.x, MoveSpeed * directionValue.y);
        position += MoveDirectionVector;
        snakeRigidBody.MovePosition(position);
        //transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.Equals(spawnedFoodMassGainer))
        {
            foodEaten++;
            GrowSnake();
        }

        if (collision.gameObject.Equals(spawnedFoodMassBurner))
        {
            ReduceSnakeSize();
        }

        if (collision.CompareTag("PlayerBody"))
            ReloadLevel();
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ReduceSnakeSize()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject snakeBodyObject = snakeBodyList[snakeBodyList.Count - 1];
            snakeBodyList.RemoveAt(snakeBodyList.Count - 1);
            Destroy(snakeBodyObject);
        }
    }

    private void GrowSnake()
    {   
        Transform prevSnake = snakeBodyList[snakeBodyList.Count - 1].transform;
        Vector3 position = new Vector3(prevSnake.position.x, prevSnake.position.y, 0);

        GameObject snakeBodyTransform = Instantiate(snakeBodyPrefab, position, Quaternion.identity);
        snakeBodyList.Add(snakeBodyTransform);
    }

    private void GrowSnake(string value)
    {
        if(value == "Initial")
        {
            Transform prevSnake = snakeBodyList[snakeBodyList.Count - 1].transform;
            Vector3 position = new Vector3(prevSnake.position.x, prevSnake.position.y, 1);

            GameObject snakeBodyObject = Instantiate(snakeBodyPrefab, position, Quaternion.identity);
            snakeBodyObject.tag = "InitialPlayerBody";
            snakeBodyList.Add(snakeBodyObject);
        }
    }
}

public enum Direction
{
    left, 
    up,
    right,
    down,
}

