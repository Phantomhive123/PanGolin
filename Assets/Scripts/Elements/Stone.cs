using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : BasicElement
{
    private void Start()
    {
        elementType = ElementType.stone;
    }

    public override void Hit(BasicElement another)
    {
        base.Hit(another);
        switch (another.elementType)
        {
            case ElementType.wood: return;
            case ElementType.stone: Disappear(); return;
            case ElementType.fire: return;
            case ElementType.magnet: Disappear(); return;
            default: return;
        }
    }

    public override void BeHit(BasicElement another)
    {
        base.BeHit(another);
        switch (another.elementType)
        {
            case ElementType.wood: return;
            case ElementType.stone: BeMagnet(); return;
            case ElementType.fire: return;
            case ElementType.magnet: Disappear(); return;
            default: return;
        }
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }

    private void BeMagnet()
    {
        GameObject obj = Resources.Load<GameObject>("Magnet");
        Instantiate(obj, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }
}
