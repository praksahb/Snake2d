using UnityEngine;

public class ColliderController : MonoBehaviour
{
    public GameObject massGainer;
    public GameObject massBurner;
    public GameObject shieldBoost;
    //public GameObject scoreBoost;
    public GameObject speedBoost;


    private void Awake()
    {
     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if (playerController)
        {
        /* Food Trigger checks */
            if (gameObject == massGainer && playerController)
            {
                //Shift collision logic from playerController here
                playerController.GrowSnake();
                gameObject.SetActive(false);
            }

            if (gameObject == massBurner)
            {
                //Shift collision logic from playerController here
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

            if(gameObject == speedBoost)
            {
                playerController.StartSpeedBoost();
                gameObject.SetActive(false);
            }
        }
    }
}
