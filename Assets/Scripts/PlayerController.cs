using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int MoveSpeed;
    //private Rigidbody2D rigidbody2D;
    private Vector3 MoveDirectionVector;
    private Direction prevDirection, currentDirection;
    private float horizontal, vertical;


    private void Awake()
    {
       // rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        MovePlayer();
    }

    private void MovePlayer()
    {
        GetInputMovement();
        SetupDirectionViaInputProvided();
        SetupTransformViaDirection();

        BorderWarping();
    }

    //Screen size = -9,5,9,-5 in clockwise == 18*10
    private void BorderWarping()
    {
        if (transform.position.x >= 9 && currentDirection == Direction.right)
        {
            ScreenWarpingOnXAxis();
        }
        if (transform.position.x <= -9 && currentDirection == Direction.left)
        {
            ScreenWarpingOnXAxis();
        }
        if (transform.position.y >= 5 && currentDirection == Direction.up)
        {
            ScreenWarpingOnYAxis();
        }
        if (transform.position.y <= -5 && currentDirection == Direction.down)
        {
            ScreenWarpingOnYAxis();
        }
    }

    private void ScreenWarpingOnXAxis()
    {
        Vector3 position = transform.position;
        position.x = position.x * -1;
        transform.position = position;
    }
    private void ScreenWarpingOnYAxis()
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

    private void SetupTransformViaDirection()
    {
        if (currentDirection == Direction.left)
        {
            TransformModifyViaDirection(new Vector2(-1, 0));
        }
        if (currentDirection == Direction.right)
        {
            TransformModifyViaDirection(new Vector2(1, 0));
        }
        if (currentDirection == Direction.up)
        {
            TransformModifyViaDirection(new Vector2(0, 1));
        }
        if (currentDirection == Direction.down)
        {
            TransformModifyViaDirection(new Vector2(0, -1));
        }
    }

    private void TransformModifyViaDirection(Vector2 directionValue)
    {
        Vector3 position = transform.position;
        MoveDirectionVector = new Vector3(MoveSpeed * Time.deltaTime * directionValue.x, MoveSpeed * Time.deltaTime * directionValue.y);
        position += MoveDirectionVector;
        transform.position = position;
    }
}

public enum Direction
{
    left, 
    up,
    right,
    down,
}

