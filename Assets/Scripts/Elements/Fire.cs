using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : BasicElement
{
    protected override void Hit(BasicElement element)
    {
        base.Hit(element);//火是可以掉下来的，所以有可能火撞到别的东西
        switch (element.elementType)
        {
            case ElementType.wood: Disappear(); return;
            case ElementType.fire: Disappear();return;
            default: return;
        }
    }

    protected override void BeHit(BasicElement element)
    {
        base.BeHit(element);
        switch (element.elementType)
        {
            case ElementType.stone: Disappear(); return;
            case ElementType.magnet: Disappear(); return;
            default: return;//草撞火是草消失，火不变，火撞草是火消失，草变火?
        }
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }
}
