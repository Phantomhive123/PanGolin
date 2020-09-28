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
    public bool isInteracted = false;

    public ElementType elementType;

    public PlayerEvenetDelegate StopDelegate;

    protected override void LateUpdate()
    {
        base.LateUpdate();
        if (collisionState.wasGroundLastFrame && !isGrounded)
        {
            CallStopDeletage();
            isInteracted = true;
        }
        if (collisionState.becameGroundedThisFrame)
        {
            if (isInteracted && ComboManager.Instance.ComboIndex == 0)
            {
                ComboManager.Instance.ContinueDelegate();
            }
            if(this is Wood || this is Stone)
                isInteracted = false;
        }
    }

    public override void Hit(MobileObj another)
    {
        base.Hit(another);
        if (!isInteracted) return;
        if (another is BasicElement)
            Hit((BasicElement)another);
    }

    public override void BeHit(MobileObj another)
    {
        base.BeHit(another);
        BasicElement be = another.GetComponent<BasicElement>();
        if (!be || !be.isInteracted) return;
        BeHit(be);
    }

    public virtual void Hit(BasicElement another)
    {
        CallStopDeletage();
        if (this is Wood && another is Fire) return;
        ComboManager.Instance.ComboIndex++;
    }

    public virtual void BeHit(BasicElement another)
    {
        
    }

    protected void CallStopDeletage()
    {
        StopDelegate?.Invoke();
        StopDelegate = null;
    }
}
