using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : BasicElement
{
    /*
    private Stone aimStone;

    private float moveSpeed = 10f;

    private void Update()
    {
        if (aimStone)
        {
            Vector3 dir = aimStone.transform.position - transform.position;
            rigidbody.velocity = dir.normalized * moveSpeed;
        }
        else
        {
            FindNextAim();
        }
    }

    protected override void Hit(BasicElement element)
    {
        base.Hit(element);
        switch (element.elementType)
        {
            case ElementType.stone: Disappear(); return;
            case ElementType.magnet: Disappear(); return; //磁石撞了磁石？？
            default: return;
        }
    }

    protected override void BeHit(BasicElement element)
    {
        base.BeHit(element);
        switch (element.elementType)
        {
            case ElementType.stone: Disappear(); return;
            case ElementType.magnet: Disappear(); return; //磁石撞了磁石？？
            default: return;
        }
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        FindNextAim();
    }

    private void FindNextAim()
    {
        Stone[] stones = FindObjectsOfType<Stone>();
        foreach (Stone s in stones)
        {
            if (aimStone == null)
                aimStone = s;
            else
            {
                if ((s.transform.position - transform.position).magnitude <= (aimStone.transform.position - transform.position).magnitude)
                {
                    aimStone = s;
                }
            }
        }
    }
    */
}
