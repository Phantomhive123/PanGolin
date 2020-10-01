using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateManager : MonoBehaviour
{
    private static InstantiateManager _instance;
    public static InstantiateManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            _instance = this;
    }

    public void CreateGameObj(GameObject aim, Vector3 pos, Quaternion rot, Transform parent)
    {
        StartCoroutine(DelayCreate(aim, pos, rot, parent));
    }

    IEnumerator DelayCreate(GameObject aim, Vector3 pos, Quaternion rot, Transform parent)
    {
        yield return new WaitForEndOfFrame();
        Instantiate(aim, pos, rot, parent);
    }
}
