using System;
using System.Collections.Generic;
using UnityEngine;

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
        InvokeRepeating("CallSpawnManager", 0.5f, 2f);
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

    //call spawnManager at fixed time interval
    private void CallSpawnManager()
    {
        SpawnManager.SingletonInstance.SpawnFoodManagerPublicHandler(snakeBodyList);
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
        if(collision.CompareTag("FoodMassGainer"))
        {
            foodEaten++;
            GrowSnake();
        }
        if (collision.CompareTag("PlayerBody"))
            Debug.Log("Touched Yourself!");

        if(collision.CompareTag("FoodMassBurner"))
        {
            ReduceSnakeSize();
        }
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

