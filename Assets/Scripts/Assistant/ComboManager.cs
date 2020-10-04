using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    private static ComboManager _instance;
    public static ComboManager Instance
    {
        get { return _instance; }
    }
    public Text comboText;

    private int _comboIndex = 0;
    public int ComboIndex
    {
        get { return _comboIndex; }
        set
        {
            _comboIndex = value;
            comboText.gameObject.SetActive(true);
            comboText.text = "Combo：" + _comboIndex + "!";
            ContinueCombo();
        }
    }

    public PlayerEvenetDelegate ContinueDelegate;

    // Start is called before the first frame update
    void Awake()
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
        comboText.gameObject.SetActive(false);
    }
}
