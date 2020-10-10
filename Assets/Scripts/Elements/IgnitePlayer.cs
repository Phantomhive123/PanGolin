using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnitePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        if (collision.GetComponent<PlayerMovement>())
        {
            GameManager.Instance.GameOver();
        }
    }
}
