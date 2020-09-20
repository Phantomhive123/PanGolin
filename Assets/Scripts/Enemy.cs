using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.GetComponent<Fire>())
            Destroy(gameObject);
        if (collision.GetComponent<PlayerMovement>())
        {
            GameManager.Instance.GameOver();
        }
    }
}
