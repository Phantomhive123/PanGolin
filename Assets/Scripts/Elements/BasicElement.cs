using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    empty,
    wood,
    stone,
    magnet,
    fire,
    earth
}


[RequireComponent(typeof(BoxCollider2D),typeof(Rigidbody2D))]
public class BasicElement : MonoBehaviour
{
    protected BoxCollider2D boxCollider;
    new protected Rigidbody2D rigidbody;

    public ElementType elementType = ElementType.empty;//后面应该改成静态的

    [SerializeField]
    protected float velocityJudgement = 0.01f;

    protected bool isLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Combine(BasicElement itemA, BasicElement itemB)
    {
        if (itemA.isLocked || itemB.isLocked) return;
        //itemA速度大于零的可能性更高
        float velocityA = itemA.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        float velocityB = itemA.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        BasicElement beHitObj = velocityA >= velocityB ? itemA : itemB;
        BasicElement movingObj = velocityA < velocityB ? itemA : itemB;
        beHitObj.BeHit(movingObj);
        movingObj.Hit(beHitObj);      
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("enter");
        BasicElement basicElement = collision.gameObject.GetComponent<BasicElement>();
        if (!basicElement) return;
        Combine(this, basicElement);
    }

    protected virtual void Hit(BasicElement element)
    {
        isLocked = true;
    }

    protected virtual void BeHit(BasicElement element)
    {
        isLocked = true;
    }
}
