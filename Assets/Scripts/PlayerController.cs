using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public SnakeType snakeTypeEnum = SnakeType.snakeHead;
    private Direction prevDirection = Direction.down, currentDirection = Direction.down;
    
    public float MoveSpeed;
    public float MoveSpeedBoosted = 1.25f;
    public float powerUpTimer = 4f;
    public int scoreIncrementer = 5;
    public int scoreDecrementerNegativeValue = -4;

    public ScoreController scoreController;
    public SnakeBodyController snakeBodyController;

    public GameObject snakeBodyPrefab;
    public BoxCollider2D screenWrappingCollider;

    private Rigidbody2D snakeRigidBody;
    private Vector3 MoveDirectionVector;
    private float horizontal, vertical;

    private float originalMoveSpeed;

    private List<GameObject> snakeHeadList;

    private bool IsShieldOn;
    private bool IsSpeedBoostOn;
    private bool IsScoreBoostOn = false;

    private float shieldPowerUpTimer;
    private float speedPowerupTimer;
    private float scoreBoostTimer;
    private bool IsPaused;

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

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
            TogglePauseResume();
        }
    }

    private void TogglePauseResume()
    {
        if(IsPaused)
        {
            Time.timeScale = 0;
        }
        if(!IsPaused)
        {
            Time.timeScale = 1;
        }
    }

    private void FixedUpdate()
    {
        MovePlayerFixedUpdate();
    }

    // Private Awake initializer
    private void InitializeSnake()
    {
        snakeRigidBody = GetComponent<Rigidbody2D>();
        snakeHeadList = new List<GameObject>
        {
            this.gameObject
        };
        InitializeSnakeBodyList();
    }

    private void InitializeSnakeBodyList()
    {
        //grows by 5 - for loop creates 5 snake bodies and adds to ListArray
            GrowSnake("Initial");
    }

    /* Private Update - Main functions */
    private void MovePlayerUpdate()
    {
        SetupDirectionViaInputProvided();
        GetInputMovement();
        BorderWrapAround();
    }

    // Private Update - helper functions
    private void UpdateHelper()
    {
        //input mapping
        MovePlayerUpdate();

        // power up timers
        if (IsShieldOn)
        {
            ShieldBoostTimer();
        }

        if (IsScoreBoostOn)
        {
            ScoreBoostTimer();
        }

        if (IsSpeedBoostOn)
        {
            SpeedBoostTimer();
        }
    }

    private void ShieldBoostTimer()
    {
        if (shieldPowerUpTimer > 0)
        {
            shieldPowerUpTimer -= Time.deltaTime;
        }
        else
        {
            IsShieldOn = false;
            //reset timer to 0 
            shieldPowerUpTimer = 0;
        }
    }

    private void ScoreBoostTimer()
    {
        if (scoreBoostTimer > 0)
        {
            scoreBoostTimer -= Time.deltaTime;
        }
        else
        {
            IsScoreBoostOn = false;
            scoreIncrementer /= 2;
            scoreDecrementerNegativeValue *= 2;
            scoreBoostTimer = 0;
        }
    }

    private void SpeedBoostTimer()
    {
        if (speedPowerupTimer > 0)
        {
            speedPowerupTimer -= Time.deltaTime;
        }
        else
        {
            IsSpeedBoostOn = false;
            MoveSpeed = originalMoveSpeed;
            speedPowerupTimer = 0;
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
        for (int i = snakeHeadList.Count - 1; i > 0; i--)
        {
            snakeHeadList[i].transform.position = snakeHeadList[i - 1].transform.position;
        }
    }
    private void SnakeBodyFollowsHead()
    {
        for (int i = 0; i < snakeHeadList.Count - 1; i++)
        {
            snakeHeadList[i + 1].transform.position = snakeHeadList[i].transform.position;
        }
    }

    /* * * * Player Interaction with spawned Objects - Food, Powerups
    */

    //Kill snake on contact with self
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SnakeBodyController Collider = collision.GetComponent<SnakeBodyController>();

        PlayerController playerController = gameObject.GetComponent<PlayerController>();

        if(Collider != null)
        {
            if(Collider.snakeBodyEnum == playerController.snakeTypeEnum)
            {
                ReloadLevel();
            }
        }
        
        //if (pcCollider.snakeTypeEnum == SnakeType.snakeBody)
        //{
        //    if (!IsShieldOn)
        //    {
        //        ReloadLevel();
        //    }
        //}
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /* PUBLIC methods */

    public void KillPlayer()
    {
        if (!IsShieldOn)
            ReloadLevel();
    }

    /* * * * Public Helper functions for ColliderController */

    public void ReduceSnakeSize()
    {
        scoreController.ScoreUpdater(scoreDecrementerNegativeValue);

        for (int i = 0; i < 5; i++)
        {
            GameObject snakeBodyObject = snakeHeadList[snakeHeadList.Count - 1];
            snakeHeadList.RemoveAt(snakeHeadList.Count - 1);
            Destroy(snakeBodyObject);
        }
    }

    /* Grow Snake 5 times */

    public void GrowSnake()
    {
        scoreController.ScoreUpdater(scoreIncrementer);

        //for (int i = 0; i < 5; i++)
        //{
        //    Vector3 position = GrowSnakeDeltaPosition();
        //    GameObject snakeBodyTransform = Instantiate(snakeBodyPrefab, position, Quaternion.identity);
        //    snakeHeadList.Add(snakeBodyTransform);
        //}

        snakeBodyController.GrowSnake();
    }

    public void StartShieldPowerup()
    {
        IsShieldOn = true;
        shieldPowerUpTimer = powerUpTimer;
    }

    public void StartScoreBoost()
    {
        IsScoreBoostOn = true;
        scoreIncrementer *= 2;
        scoreDecrementerNegativeValue /= 2;
        scoreBoostTimer = powerUpTimer * 2;
    }

    public void StartSpeedBoost()
    {
        IsSpeedBoostOn = true;
        MoveSpeed = MoveSpeedBoosted;
        speedPowerupTimer = powerUpTimer * 2;
    }

    public Vector3 GetEndSnakeHeadPosition()
    {
        return snakeHeadList[snakeHeadList.Count - 1].transform.position;
    }

    /* * * * Public Helper functions for SpawnManager */
    public Bounds GetSnakeTotalBound()
    {
        Bounds snakeBound = new Bounds();
        for (int i = 0; i < snakeHeadList.Count; i++)
            snakeBound.Encapsulate(snakeHeadList[i].GetComponent<Collider2D>().bounds);
        return snakeBound;
    }

    public int GetSnakeLength()
    {
        return snakeHeadList.Count;
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
        Bounds bounds = screenWrappingCollider.bounds;

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
        Transform prevPrevtransform = snakeHeadList[snakeHeadList.Count - 2].transform;
        Transform prevSnake = snakeHeadList[snakeHeadList.Count - 1].transform;
        Vector3 deltaPosition = prevPrevtransform.position - prevSnake.position;
        return new Vector3(prevSnake.position.x + deltaPosition.x, prevSnake.position.y + deltaPosition.y, 0);
    }

    private void GrowSnake(string value)
    {
        if (value == "Initial")
        {
            for (int i = 0; i < 5; i++)
            {
                // 0.5f is moveSpeed
                // initially moving downwards
                Vector2 deltaPosition = new Vector2(0, 1f);
                Transform prevSnake = snakeHeadList[snakeHeadList.Count - 1].transform;
                Vector3 position = new Vector3(prevSnake.position.x, prevSnake.position.y + deltaPosition.y);

                GameObject snakeBodyObject = Instantiate(snakeBodyPrefab, position, Quaternion.identity);
                snakeHeadList.Add(snakeBodyObject);
            }
        }
    }
}