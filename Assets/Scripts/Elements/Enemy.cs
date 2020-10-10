using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ignite>())
            Destroy(gameObject);
        if (collision.GetComponent<Magnet>())
            Destroy(gameObject);
        if (collision.GetComponent<PlayerMovement>())
        {
            GameManager.Instance.GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ignite>())
            Destroy(gameObject);
        if (collision.gameObject.GetComponent<Magnet>())
            Destroy(gameObject);
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            GameManager.Instance.GameOver();
        }
    }
}
