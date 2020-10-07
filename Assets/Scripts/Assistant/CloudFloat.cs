using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudFloat : MonoBehaviour
{
    public Vector3[] aims;
    public float speed = 2f;

    private int currentIndex = 0;
    private float moveThreshold = 0.05f;

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - aims[currentIndex]).magnitude < moveThreshold)
        {
            currentIndex = (currentIndex + 1) % aims.Length;
        }
        else
            transform.Translate((aims[currentIndex] - transform.position).normalized * speed * Time.deltaTime);
    }
}
