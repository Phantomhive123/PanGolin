using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotate : MonoBehaviour
{
    public float rotateSpeed = 2f;

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.fixedDeltaTime);
    }
}
