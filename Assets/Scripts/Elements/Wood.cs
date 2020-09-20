using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : BasicElement
{
    // Start is called before the first frame update
    protected override void Hit(BasicElement element)
    {
        base.Hit(element);
        switch(element.elementType)
        {
            case ElementType.wood: Disappear();return;
            case ElementType.stone: Disappear();return;
            //case ElementType.fire: Disappear(); return;
            case ElementType.magnet:Disappear();return;
            default:return;
        }
    }

    protected override void BeHit(BasicElement element)
    {
        base.BeHit(element);
        switch (element.elementType)
        {
            case ElementType.wood: Burn(); return;
            case ElementType.stone: return; //草被石头撞？
            //case ElementType.fire: Burn(); return;
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
        GameObject obj = Resources.Load<GameObject>("Fire");
        obj = Instantiate(obj, transform.parent);
        obj.transform.position = transform.position;
        Destroy(gameObject);
    }

    public void DelayBurn()
    {
        StartCoroutine(WaitForBurn());
    }

    IEnumerator WaitForBurn()
    {
        yield return new WaitForSeconds(0.5f);
        Burn();
    }
}
