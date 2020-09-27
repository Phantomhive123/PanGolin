using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    private static ComboManager _instance;
    public static ComboManager Instance
    {
        get { return _instance; }
    }

    private int _comboIndex = 0;
    public int ComboIndex
    {
        get { return _comboIndex; }
        set
        {
            ContinueCombo();
            _comboIndex = value;
            Debug.Log("Combo:" + ComboIndex);
        }
    }

    public PlayerEvenetDelegate ContinueDelegate;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) 
            _instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame

    private void ContinueCombo()
    {
        StopAllCoroutines();
        StartCoroutine(DelayCleanCombo());
    }

    IEnumerator DelayCleanCombo()
    {
        yield return new WaitForSeconds(1f);
        ContinueDelegate?.Invoke();
        _comboIndex = 0;
    }
}
