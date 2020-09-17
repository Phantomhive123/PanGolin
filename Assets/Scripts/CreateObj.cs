using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObj : MonoBehaviour
{
    [SerializeField]
    private GameObject aimObj;

    [SerializeField]
    private GameObject preview;

    private static GameObject currentAimObj;
    private static GameObject currentPreview;

    // Update is called once per frame
    void Update()
    {
        if (currentPreview != null) 
        {
            Vector3 Pos = Input.mousePosition;
            currentPreview.GetComponent<RectTransform>().position = Pos;
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (currentAimObj == null) return;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(aimObj, worldPos, Quaternion.identity);
        }
    }

    public void SetCurrentObj()
    {
        currentAimObj = aimObj;
        if(currentPreview)
            currentPreview.SetActive(false);
        currentPreview = preview;
        if(currentPreview)
            currentPreview.SetActive(true);
    }
}
