using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float MoveSpeedBoosted = 1.25f;
    public GameObject snakeBodyPrefab;
    public BoxCollider2D borderWrappingCollider;
    public float powerUpTimer = 4f;
    public float flexibleCooldownTimer = 3f;

    private Rigidbody2D snakeRigidBody;
    private Vector3 MoveDirectionVector;
    private Direction prevDirection = Direction.down, currentDirection = Direction.down;
    private float horizontal, vertical;
    //private int foodEaten;

    private float originalMoveSpeed;

    private List<GameObject> snakeBodyList;

    private bool IsShieldOn;
    private bool IsSpeedBoosted;

    private float shieldPowerUpTimer;
    private float SpeedPowerupTimer;

    private enum Direction
    {
        left,
        up,
        right,
        down,
    }

    private void Awake()
    {
        InitializeSnake();
    }
    private void Start()
    {
        originalMoveSpeed = MoveSpeed;
    }
    private void Update()
    {
        UpdateHelper();
    }
    private void FixedUpdate()
    {
        //physics
        MovePlayerFixedUpdate();
    }

    // Private Awake initializer
    private void InitializeSnake()
    {
        snakeRigidBody = GetComponent<Rigidbody2D>();
        snakeBodyList = new List<GameObject>
        {
            this.gameObject
        };
        InitializeSnakeBodyList(1);
    }

    private void InitializeSnakeBodyList(int minimumSnakeLength)
    {
        for (int i = 0; i < minimumSnakeLength; i++)
        {
            GrowSnake("Initial");
        }
    }


    // Private Update - helper functions

    private void UpdateHelper()
    {
        //input mapping
        MovePlayerUpdate();

        // power up timers
        if (IsShieldOn)
        {
            ShieldTimer();
        }

        if (IsSpeedBoosted)
        {
            SpeedTimer();
        }
    }
    private void MovePlayerUpdate()
    {
        SetupDirectionViaInputProvided();
        GetInputMovement();
        BorderWrapAround();
    }

    private void ShieldTimer()
    {
        if (shieldPowerUpTimer > 0)
        {
            shieldPowerUpTimer -= Time.deltaTime;
        }
        else
        {
            IsShieldOn = false;
            shieldPowerUpTimer = 0;
        }
    }

    private void SpeedTimer()
    {
        if (SpeedPowerupTimer > 0)
        {
            SpeedPowerupTimer -= Time.deltaTime;
        }
        else
        {
            IsSpeedBoosted = false;
            MoveSpeed = originalMoveSpeed;
            //reset timer to 0 
            SpeedPowerupTimer = 0;
        }
    }

    // Private FixedUpdate - helper/middleware
    private void MovePlayerFixedUpdate()
    {
        SetupTransformViaDirection();
        SnakeBodyFollowsHeadReverse();
    }

    /* * * * Player Movement Controller */

    /* Input related */

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

    // Physics related
    /* change in transform according to input */

    // top-down feel = -90, 180, 90, 0
    // ?sideways feel = 0, 90, 0, 90
    /* 
     * Rotate Snake Head ie. player according to Direction 
     */
    private void SetupTransformViaDirection()
    {
        float rotateImageLeftByEulerAngle = -90f;
        float rotateImageUpwardsByEulerAngle = 180f;
        float rotateImageRightByEulerAngle = 90f;
        float rotateImageDownwardsByEulerAngle = 0f;

        if (currentDirection == Direction.left)
        {
            TransformModifyViaDirection(Vector2.left);
            transform.rotation = Quaternion.Euler(0f, 0f, rotateImageLeftByEulerAngle);
        }
        if (currentDirection == Direction.up)
        {
            TransformModifyViaDirection(Vector2.up);
            transform.rotation = Quaternion.Euler(0f, 0f, rotateImageUpwardsByEulerAngle);
        }
        if (currentDirection == Direction.right)
        {
            TransformModifyViaDirection(Vector2.right);
            transform.rotation = Quaternion.Euler(0f, 0f, rotateImageRightByEulerAngle);
        }
        if (currentDirection == Direction.down)
        {
            TransformModifyViaDirection(Vector2.down);
            transform.rotation = Quaternion.Euler(0f, 0f, rotateImageDownwardsByEulerAngle);
        }
    }
    /* Private Main Functions */
    /* Player Movement - Related
 * Main function for moving the snakeHead gameObject
 * via modifying the transform.position every frame/Update
 */
    private void TransformModifyViaDirection(Vector2 directionValue)
    {
        Vector3 position = transform.position;
        MoveDirectionVector = new Vector3(MoveSpeed * directionValue.x, MoveSpeed * directionValue.y);
        position += MoveDirectionVector;
        snakeRigidBody.MovePosition(position);
    }

    /* Private Move Snake Body functions
     * main function(s) for moving snakeBody
     * i =0 , snakeHead
     * i >= 0++, snakeBody
     * nth item gets position of (n - 1)th item
     * only x,y values.  z values remain same for each item
     */

    private void SnakeBodyFollowsHeadReverse()
    {
        for (int i = snakeBodyList.Count - 1; i > 0; i--)
        {
            //get x, y values only for snake body
            Vector2 prevPosition = snakeBodyList[i - 1].transform.position;
            snakeBodyList[i].transform.position = new Vector3(prevPosition.x, prevPosition.y, snakeBodyList[i].transform.position.z);
        }
    }
    private void SnakeBodyFollowsHead()
    {
        for (int i = 0; i < snakeBodyList.Count - 1; i++)
        {
            Vector2 position = snakeBodyList[i].transform.position;
            snakeBodyList[i + 1].transform.position = new Vector3(position.x, position.y, snakeBodyList[i].transform.position.z);
        }
    }

    /* * * * Player Interaction with spawned Objects - Food, Powerups
    */

    //Kill snake on contact with itself
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBody"))
        {
            if (!IsShieldOn)
            {
                ReloadLevel();
            }
        }
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /* * * * Public Helper functions for ColliderController */
    public void ReduceSnakeSize()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject snakeBodyObject = snakeBodyList[snakeBodyList.Count - 1];
            snakeBodyList.RemoveAt(snakeBodyList.Count - 1);
            Destroy(snakeBodyObject);
        }
    }

    /* Grow Snake 5 times */

    public void GrowSnake()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 position = GrowSnakeDeltaPosition();
            GameObject snakeBodyTransform = Instantiate(snakeBodyPrefab, position, Quaternion.identity);
            snakeBodyList.Add(snakeBodyTransform);
        }
    }

    public void StartShieldPowerup()
    {
        IsShieldOn = true;
        shieldPowerUpTimer = powerUpTimer;
    }

    public void StartSpeedBoost()
    {
        IsSpeedBoosted = true;
        MoveSpeed = MoveSpeedBoosted;
        SpeedPowerupTimer = powerUpTimer * 2;
    }

    /* * * * Public Helper functions for SpawnManager */
    public Bounds GetSnakeTotalBound()
    {
        Bounds snakeBound = new Bounds();
        for (int i = 0; i < snakeBodyList.Count; i++)
            snakeBound.Encapsulate(snakeBodyList[i].GetComponent<Collider2D>().bounds);
        return snakeBound;
    }

    public int GetSnakeLength()
    {
        return snakeBodyList.Count;
    }

    /* Private Helper Functions */

    // Helper function for implementing Screen wrapping
    /*
        * * * * * * * * * * * Screen Wrapping * * * * * * * * * * * * * * * * * * * * 
        * get bounds of your camera's total viewing area by using a direct 
        * reference of the bounds of a collider2d setup in the editor
        * Alternatively, attach boxcollider2d directly on camera component
        * can be called directly from scripts without needing any public references
        * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    */

    private void BorderWrapAround()
    {
        Bounds bounds = borderWrappingCollider.bounds;

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
        position.x *= -1;
        transform.position = position;
    }

    private void ScreenWrappingOnYAxis()
    {
        Vector3 position = transform.position;
        position.y *= -1;
        transform.position = position;
    }


    // Helper function for GrowSnake
    private Vector3 GrowSnakeDeltaPosition()
    {
        Transform prevPrevtransform = snakeBodyList[snakeBodyList.Count - 2].transform;
        Transform prevSnake = snakeBodyList[snakeBodyList.Count - 1].transform;
        Vector3 deltaPosition = prevPrevtransform.position - prevSnake.position;
        return new Vector3(prevSnake.position.x + deltaPosition.x, prevSnake.position.y + deltaPosition.y, 0);
    }

    private void GrowSnake(string value)
    {
        if (value == "Initial")
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 deltaPosition = new Vector2(0, 0.5f);
                Transform prevSnake = snakeBodyList[snakeBodyList.Count - 1].transform;
                Vector3 position = new Vector3(prevSnake.position.x, prevSnake.position.y + deltaPosition.y, 0);

                GameObject snakeBodyObject = Instantiate(snakeBodyPrefab, position, Quaternion.identity);
                snakeBodyObject.tag = "InitialPlayerBody";
                snakeBodyList.Add(snakeBodyObject);
            }
        }
    }
}