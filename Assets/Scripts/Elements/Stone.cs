using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : BasicElement
{
    // Start is called before the first frame update
    protected override void Hit(BasicElement element)
    {
        base.Hit(element);
        switch (element.elementType)
        {
            case ElementType.stone: Disappear(); return;
            case ElementType.magnet: Disappear(); return;
            default: HitOtherObj(); return;
        }
    }

    protected override void BeHit(BasicElement element)
    {
        base.BeHit(element);
        switch (element.elementType)
        {
            case ElementType.wood: FallDown();return;
            case ElementType.stone: BeMagnet(); return;
            case ElementType.magnet: Disappear(); return;
            default: return;
        }
    }

    private void HitOtherObj()
    {
        Debug.Log("Stop for a moment!");
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }

    private void FallDown()
    {
        Debug.Log("fall down!");
    }

    private void BeMagnet()
    {
        GameObject obj = Resources.Load<GameObject>("Magnet");
        obj = Instantiate(obj, transform.parent);
        obj.transform.position = transform.position;
        Destroy(gameObject);
    }
}
