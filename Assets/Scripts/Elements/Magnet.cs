using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : BasicElement
{
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
}
