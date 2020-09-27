using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : BasicElement
{
    [SerializeField]
    private float BurnDelayTime = 0.5f;

    private void Start()
    {
        elementType = ElementType.wood;
    }

    public override void Hit(BasicElement another)
    {
        base.Hit(another);
        switch (another.elementType)
        {
            case ElementType.wood: Disappear(); return; //？
            case ElementType.stone: CallContinueDelegate(); Disappear(); return;
            case ElementType.fire: Disappear(); return;
            case ElementType.magnet: CallContinueDelegate(); Disappear(); return;
            default: return;
        }
    }

    public override void BeHit(BasicElement another)
    {
        base.BeHit(another);
        switch (another.elementType)
        {
            case ElementType.wood: Burn(); return;
            case ElementType.stone: Disappear(); return;
            case ElementType.fire: return;
            case ElementType.magnet: Disappear(); return;
            default: return;
        }
    }
    private void Disappear()
    {
        Destroy(gameObject);
    }

    private void Burn()
    {
        //应该播放动画哦
        GameObject obj = Resources.Load<GameObject>("Fire");
        obj = Instantiate(obj, transform.parent);
        obj.transform.position = transform.position;
        //传递combo
        Destroy(gameObject);
    }

    public void DelayBurn()
    {
        StartCoroutine(WaitForBurn());
    }

    IEnumerator WaitForBurn()
    {
        yield return new WaitForSeconds(BurnDelayTime);
        Burn();
    }
}
