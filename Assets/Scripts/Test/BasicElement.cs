using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    empty,
    wood,
    stone,
    magnet,
    fire
}

public class BasicElement : BoxObj
{
    [SerializeField]
    private int _comboNum = -1;
    public int ComboNum
    {
        get { return _comboNum; }
        set { _comboNum = value; }
    }

    /*
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hit!");
        BasicElement basicElement = collision.gameObject.GetComponent<BasicElement>();
        if (!basicElement) return;
        Combine(basicElement);
    }

    private void Combine(BasicElement element)
    {
        if (ComboNum < 0 && element.ComboNum < 0)
            return;
        if (ComboNum >= 0 && element.ComboNum >= 0)
            return;

        if (ComboNum > 0)
            Hit(element);
        else
            BeHit(element);
    }
    */

    public override void Hit(MobileObj another)
    {
        base.Hit(another);
        if (ComboNum < 0) return;
        Debug.Log("Hit:" + gameObject.name);
    }

    public override void BeHit(MobileObj another)
    {
        base.BeHit(another);
        BasicElement be = another.GetComponent<BasicElement>();
        if (!be || be.ComboNum < 0) return;
        Debug.Log("BeHit:" + gameObject.name);
    }
}
