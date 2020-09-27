using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Ignite : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //这样以来有可能重复触发
        Wood wood = collision.gameObject.GetComponent<Wood>();
        if (wood)
        {
            Fire f = GetComponentInParent<Fire>();
            f.WaitForDisappear();
            wood.DelayBurn();
            return;
        }
    }
}
