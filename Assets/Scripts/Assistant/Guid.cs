using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Guid : MonoBehaviour
{
    public string note;
    public GameObject panel;
    public TMP_Text text;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovement>())
        {
            GameManager.Instance.GamePause();
            panel.SetActive(true);
            text.text = note;
            Destroy(gameObject);
        }
    }
}
