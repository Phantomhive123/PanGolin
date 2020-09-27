using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayerEvenetDelegate();

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

    public ElementType elementType;

    public PlayerEvenetDelegate StopDelegate;
    public PlayerEvenetDelegate ContinueDelegate;

    protected override void LateUpdate()
    {
        base.LateUpdate();
        if (collisionState.wasGroundLastFrame && !isGrounded)
        {
            CallStopDeletage();
        }
        if (collisionState.becameGroundedThisFrame)
        {
            CallContinueDelegate();
            //改combonum
            ComboNum = -1;
        }
    }

    public override void Hit(MobileObj another)
    {
        base.Hit(another);
        if (ComboNum < 0) return;
        if (another is BasicElement)
            Hit((BasicElement)another);
    }

    public override void BeHit(MobileObj another)
    {
        base.BeHit(another);
        BasicElement be = another.GetComponent<BasicElement>();
        if (!be || be.ComboNum < 0) return;
        BeHit(be);
    }

    public virtual void Hit(BasicElement another)
    {
        CallStopDeletage();
        CallContinueDelegate();
    }

    public virtual void BeHit(BasicElement another)
    {
        
    }

    protected void CallStopDeletage()
    {
        StopDelegate?.Invoke();
        StopDelegate = null;
    }

    protected void CallContinueDelegate()
    {
        ContinueDelegate?.Invoke();
        ContinueDelegate = null;
    }
}
