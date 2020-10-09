using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : BasicElement
{
    [SerializeField]
    private float BurnDelayTime = 0.5f;
    public AudioClip burnAudio;

    private void Start()
    {
        elementType = ElementType.wood;
    }

    public override void Hit(BasicElement another)
    {
        base.Hit(another);
        switch (another.elementType)
        {
            case ElementType.wood: Disappear(); return;
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
            case ElementType.wood: Burn(); return;
            case ElementType.stone: Disappear(); return;
            case ElementType.fire: return;
            case ElementType.magnet: Disappear(); return;
            default: return;
        }
    }
    private void Disappear()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void Burn()
    {
        //应该播放动画哦
        AudioSource.PlayClipAtPoint(burnAudio, transform.position);
        GameObject obj = Resources.Load<GameObject>("Fire");
        StopAllCoroutines();
        InstantiateManager.Instance.CreateGameObj(obj, transform.position, transform.rotation, transform.parent);  
        Destroy(gameObject);
    }

    public void DelayBurn()
    {
        StopAllCoroutines();
        StartCoroutine(WaitForBurn());
    }

    IEnumerator WaitForBurn()
    {
        yield return new WaitForSeconds(BurnDelayTime);
        Burn();
    }
}
