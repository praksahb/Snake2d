using UnityEngine;

public class ColliderController : MonoBehaviour
{

    public SpawnType spawnType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if (playerController)
        {
            ColliderController colCtrl = gameObject.GetComponent<ColliderController>();

            if (colCtrl != null)
            {
                /* Food Trigger checks */
                if (colCtrl.spawnType == SpawnType.massGainer)
                {
                    playerController.GrowSnake(playerController.playerType);
                    gameObject.SetActive(false);
                }

                if (colCtrl.spawnType == SpawnType.massBurner)
                {
                    playerController.ReduceSnakeSize();
                    gameObject.SetActive(false);
                }
                /* Power-up Trigger checks */
                // shield boost
                if (colCtrl.spawnType == SpawnType.shieldBoost)
                {
                    playerController.StartShieldBoost();
                    gameObject.SetActive(false);
                }

                // score boost
                if (colCtrl.spawnType == SpawnType.scoreBoost)
                {
                    playerController.StartScoreBoost();
                    gameObject.SetActive(false);
                }
                //speed boost
                if (colCtrl.spawnType == SpawnType.speedBoost)
                {
                    playerController.StartSpeedBoost();
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
