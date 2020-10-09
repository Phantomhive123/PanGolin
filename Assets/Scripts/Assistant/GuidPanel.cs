using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidPanel : MonoBehaviour
{
    private static GuidPanel _instance;
    public static GuidPanel Instance
    {
        get { return _instance; }
    }

    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null) 
            _instance = this;
        else 
            Destroy(gameObject);
    }

    public void SetText(string note)
    {
        text.text = note;
    }
}
