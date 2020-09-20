using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanelButton : MonoBehaviour
{
    [SerializeField]
    private GameObject aimPos;

    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private GameObject anotherButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetPanelPos()
    {
        panel.GetComponent<RectTransform>().localPosition = aimPos.GetComponent<RectTransform>().localPosition;
        anotherButton.SetActive(true);
        gameObject.SetActive(false);
    }
}
