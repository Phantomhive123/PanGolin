using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateObj : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objs;
    [SerializeField]
    private GameObject[] previews;

    private int currentIndex = -1;

    // Update is called once per frame
    void Update()
    {
        if (currentIndex != -1 && currentIndex < previews.Length)
        {
            Vector3 Pos = Input.mousePosition;
            previews[currentIndex].GetComponent<RectTransform>().position = Pos;
        }
        
        //注意移动端判断方法
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (currentIndex != -1 && currentIndex < previews.Length)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0;
                Instantiate(objs[currentIndex], worldPos, Quaternion.identity);
            }
        }
    }

    public void SetCurrentIndex(int index)
    {
        if (currentIndex != -1) 
            previews[currentIndex].SetActive(false);
        currentIndex = index;
        if (currentIndex != -1)
            previews[currentIndex].SetActive(true);
    }
}
