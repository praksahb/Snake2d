using UnityEngine;

public class ColliderController : MonoBehaviour
{
    public GameObject massGainer;
    public GameObject massBurner;
    public GameObject shieldBoost;
    public GameObject scoreBoost;
    public GameObject speedBoost;

    public SnakeBodyController snakeBodyController;


    /* Upgrade the project Collisions using Interface & Enums to make it scalable */


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();


        if (playerController)
        {
            if (gameObject == snakeBodyController)
            {
                playerController.KillPlayer();

            }

            /* Food Trigger checks */
            if (gameObject == massGainer)
            {
                playerController.GrowSnake();
                gameObject.SetActive(false);
            }

            if (gameObject == massBurner)
            {
                playerController.ReduceSnakeSize();
                gameObject.SetActive(false);
            }
        /* Power-up Trigger checks */
            if (gameObject == shieldBoost)
            {
                playerController.StartShieldPowerup();
                gameObject.SetActive(false);
            }

            // score boost
            if(gameObject == scoreBoost)
            {
                playerController.StartScoreBoost();
                gameObject.SetActive(false);
            }

            if(gameObject == speedBoost)
            {
                playerController.StartSpeedBoost();
                gameObject.SetActive(false);
            }
        }
    }
}
