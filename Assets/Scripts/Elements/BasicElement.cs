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
    private float fallThrehold = 0.2f;
    public bool isInteracted = false;

    public ElementType elementType;

    public PlayerEvenetDelegate StopDelegate;

    protected override void LateUpdate()
    {
        base.LateUpdate();
        if (collisionState.wasGroundLastFrame && !isGrounded)
        {
            CallStopDeletage();
            //check判断是否激活
            CheckFalls();
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

    private void CheckFalls()
    {
        if (isInteracted) return;
        Vector2 dis = Vector2.down * fallThrehold;
        MoveVertically(ref dis);
        if (dis == Vector2.down * fallThrehold)
            StartCoroutine(DelaySetInteraction());
    }

    public override void Hit(MobileObj another)
    {
        base.Hit(another);
        if (another is BasicElement)
        {
            BasicElement bs = (BasicElement)another;
            if (!isInteracted) return;
            //if (this is Wood && another is Fire) return;
            Hit(bs);
        }
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
        ComboManager.Instance.ComboIndex++;
        Debug.Log(gameObject.name + " hit " + another.name);
    }

    public virtual void BeHit(BasicElement another)
    {
        CallStopDeletage();
    }

    protected void CallStopDeletage()
    {
        StopDelegate?.Invoke();
        //StopDelegate = null;
    }

    IEnumerator DelaySetInteraction()
    {
        yield return new WaitForFixedUpdate();
        isInteracted = true;
    }
}
