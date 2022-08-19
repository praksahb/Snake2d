using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderOnFood : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided  outer");
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Collided");
            Destroy(this.gameObject);
        }
    }
}
