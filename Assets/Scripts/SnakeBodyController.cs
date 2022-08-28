using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyController : MonoBehaviour
{
    public SnakeType snakeBodyEnum;

    public PlayerController snakeHead;
    public GameObject snakeBodyPrefab;

    private List<GameObject> snakeBodyList;

    private void Awake()
    {
        snakeBodyList = new List<GameObject>();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        SnakeBodyFollowsHead();
    }

    private void SnakeBodyFollowsHead()
    {
        if (snakeBodyList.Count > 1)
        {
            snakeBodyList[0].transform.position = snakeHead.GetEndSnakeHeadPosition();

            for (int i = snakeBodyList.Count - 1; i > 0; i--)
            {
                snakeBodyList[i].transform.position = snakeBodyList[i - 1].transform.position;
            }
        }
    }

    public void GrowSnake()
    {
        if (snakeBodyList.Count > 0)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector3 position = snakeBodyList[snakeBodyList.Count - 1].transform.position;
                GameObject snakeBodyInstantiated = Instantiate(snakeBodyPrefab, position, Quaternion.identity);
                snakeBodyList.Add(snakeBodyInstantiated);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            Vector3 position = snakeHead.GetEndSnakeHeadPosition();
            GameObject snakeBodyInstantiated = Instantiate(snakeBodyPrefab, position, Quaternion.identity);
            snakeBodyList.Add(snakeBodyInstantiated);
        }
    }
}
