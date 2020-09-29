using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : BasicElement
{
    private Stone aimStone;
    [SerializeField]
    private float moveSpeed = 10f;
    private float startGravity = 5f;

    private void Start()
    {
        elementType = ElementType.magnet;
        startGravity = gravityModifier;
    }

    public override void Hit(BasicElement another)
    {
        base.Hit(another);
        switch (another.elementType)
        {
            case ElementType.wood: return;
            case ElementType.stone: Disappear(); return;
            case ElementType.fire: Disappear(); return;
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
            case ElementType.stone: Disappear(); return;
            case ElementType.fire: BeStone(); return;
            case ElementType.magnet: Disappear(); return;
            default: return;
        }
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }

    private void BeStone()
    {
        GameObject obj = Resources.Load<GameObject>("Stone"); 
        obj = Instantiate(obj, transform.parent);
        obj.transform.position = transform.position;
        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update();
        if (aimStone)
        {
            Vector2 dir = aimStone.transform.position - transform.position;
            targetVelocity += dir.normalized * moveSpeed;
        }
        else
            FindNextAim();

    }
        
    private void FindNextAim()
    {
        Stone[] stones = FindObjectsOfType<Stone>();
        foreach (Stone s in stones)
        {
            if (aimStone == null)
            {
                aimStone = s;
                continue;
            }
            
            if ((s.transform.position - transform.position).magnitude <= (aimStone.transform.position - transform.position).magnitude)
            {
                
                aimStone = s;
            }
        }
        isInteracted = (aimStone != null);
        gravityModifier = isInteracted ? 0f : startGravity;
    }
}
