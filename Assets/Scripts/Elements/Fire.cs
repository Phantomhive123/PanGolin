using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : BasicElement
{
    public override void Hit(MobileObj another)
    {
        base.Hit(another);
        switch(another.ele)
    }

    public override void BeHit(MobileObj another)
    {
        base.BeHit(another);
    }

    /*
    protected override void Hit(BasicElement element)
    {
        base.Hit(element);//火是可以掉下来的，所以有可能火撞到别的东西
        switch (element.elementType)
        {
            //case ElementType.wood: Disappear(); return;
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

    private void WaitForDisappear()
    {
        Destroy(gameObject, 0.5f);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        //这样以来有可能重复触发
        Wood wood = collision.gameObject.GetComponent<Wood>();
        if (wood)
        {
            rigidbody.bodyType = RigidbodyType2D.Static;
            WaitForDisappear();
            wood.DelayBurn();
            return;
        }

        if (collision.gameObject.GetComponent<PlayerMovement>())
            GameManager.Instance.GameOver();
    }*/
}
