using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : BasicElement
{
    [SerializeField]
    private float BurnDelayTime = 0.5f;

    private void Start()
    {
        elementType = ElementType.fire;
    }

 
    public override void Hit(BasicElement another)
    {
        base.Hit(another);
        switch(another.elementType)
        {
            case ElementType.wood:return;
            case ElementType.stone: Disappear(); return;
            case ElementType.fire: Disappear(); return;
            case ElementType.magnet: Disappear(); return;
            default:return;
        }
    }

    public override void BeHit(BasicElement another)
    {
        base.BeHit(another);
        switch (another.elementType)
        {
            case ElementType.wood: return;
            case ElementType.stone: Disappear(); return;
            case ElementType.fire: return;
            case ElementType.magnet: BeStone(); return;
            default: return;
        }
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }

    public void WaitForDisappear(Wood wood)
    {
        StartCoroutine(DelayDisappear(wood));
    }

    private void BeStone()
    {
        GameObject obj = Resources.Load<GameObject>("Stone");
        Instantiate(obj, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }

    IEnumerator DelayDisappear(Wood wood)
    {
        yield return new WaitForSeconds(BurnDelayTime);
        if (wood)
            Disappear();
    }
}
