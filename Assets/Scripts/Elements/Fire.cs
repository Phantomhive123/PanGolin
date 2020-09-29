using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : BasicElement
{
    [SerializeField]
    private float BurnDelayTime = 0.5f;
    private float startGravity = 0f;

    private Coroutine delayFall;

    private void Start()
    {
        elementType = ElementType.fire;
        startGravity = gravityModifier;
        gravityModifier = 0;
        delayFall = StartCoroutine(DelayFall());
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

    public void CancelDelayFall()
    {
        StopCoroutine(delayFall);
    }

    private void BeStone()
    {
        GameObject obj = Resources.Load<GameObject>("Stone"); 
        obj = Instantiate(obj, transform.parent);
        obj.transform.position = transform.position;
        Destroy(gameObject);
    }

    IEnumerator DelayDisappear(Wood wood)
    {
        yield return new WaitForSeconds(BurnDelayTime);
        if (wood)
            Disappear();
        else
            gravityModifier = startGravity;
    }

    IEnumerator DelayFall()
    {
        yield return new WaitForSeconds(0.1f);
        gravityModifier = startGravity;
    }
}
