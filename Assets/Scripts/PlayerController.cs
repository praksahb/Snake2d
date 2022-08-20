using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public Transform snakeBodyPrefab;

    private Rigidbody2D snakeRigidBody;
    private Vector3 MoveDirectionVector;
    private Direction prevDirection, currentDirection = Direction.down;
    private float horizontal, vertical;
    private int foodEaten;

    private List<Transform> snakeBodyList;

    private void Awake()
    {
        snakeRigidBody = GetComponent<Rigidbody2D>();

        snakeBodyList = new List<Transform>();
        snakeBodyList.Add(gameObject.transform);
        GrowSnake("Initial");
        GrowSnake("Initial");
        GrowSnake("Initial");
        GrowSnake("Initial");
        GrowSnake("Initial");
    }

    private void Start()
    {
        InvokeRepeating("GrowSnake", 0.2f, 0.9f);
    }

    private void Update()
    {
        //input
        MovePlayerUpdate();
    } 

    //TimeStep : 0.02, moveSpeed = 0.1 initial
    //timeStep : 0.04, MoveSpeed = 0.2 initial, 0.5 good enough for end.
    
    private void FixedUpdate()
    {
        //physics
        MovePlayerFixedUpdate();
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
            Vector2 prevPosition = snakeBodyList[i-1].position;
            snakeBodyList[i].position = new Vector3(prevPosition.x, prevPosition.y, snakeBodyList[i].position.z);
        }
    }
    

    // can use bounds for checking if it has been crossed
    // similar to how bounds are used to create a random spawnArea
    private void BorderWrapAround()
    {
        float borderExtremeLeft = -27;
        float borderRight = 27;
        float borderTop = 15;
        float borderBottom = -15;

        if (transform.position.x >= borderRight && currentDirection == Direction.right)
        {
            ScreenWrappingOnXAxis();
        }
        if (transform.position.x <= borderExtremeLeft && currentDirection == Direction.left)
        {
            ScreenWrappingOnXAxis();
        }
        if (transform.position.y >= borderTop && currentDirection == Direction.up)
        {
            ScreenWrappingOnYAxis();
        }
        if (transform.position.y <= borderBottom && currentDirection == Direction.down)
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
        if(collision.CompareTag("Food"))
        {
            foodEaten++;
            GrowSnake();
        }
        if (collision.CompareTag("PlayerBody"))
            Debug.Log("Touched Yourself!");
    }

    private void GrowSnake()
    {
        Transform prevSnake = snakeBodyList[snakeBodyList.Count - 1];
        Vector3 position = new Vector3(prevSnake.position.x, prevSnake.position.y, 0);

        //Transform snakeBodyTransform = Instantiate(snakeBodyPrefab, prevSnake);
        Transform snakeBodyTransform = Instantiate(snakeBodyPrefab, position, Quaternion.identity);
        snakeBodyList.Add(snakeBodyTransform);
    }

    private void GrowSnake(string value)
    {
        if(value == "Initial")
        {
            Transform prevSnake = snakeBodyList[snakeBodyList.Count - 1];
            Vector3 position = new Vector3(prevSnake.position.x, prevSnake.position.y, 1);

            //Transform snakeBodyTransform = Instantiate(snakeBodyPrefab, prevSnake);
            Transform snakeBodyTransform = Instantiate(snakeBodyPrefab, position, Quaternion.identity);
            snakeBodyTransform.tag = "InitialPlayerBody";
            snakeBodyList.Add(snakeBodyTransform);
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

