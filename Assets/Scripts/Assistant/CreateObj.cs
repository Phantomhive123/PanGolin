#if UNITY_ANDROID && !UNITY_EDITOR
#define ANDROID
#endif

#if UNITY_IPHONE && !UNITY_EDITOR
#define IPHONE
#endif

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

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
#if IPHONE || ANDROID
			if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
            if (EventSystem.current.IsPointerOverGameObject())
#endif
            {
                return;
            }
            else
            {
                if (currentIndex != -1 && currentIndex < previews.Length)
                {
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    worldPos.z = 0;
                    Instantiate(objs[currentIndex], worldPos, Quaternion.identity);
                }
            }
        }
    }

    public void SetCurrentIndex(int index)
    {
#if IPHONE || ANDROID
		currentIndex = index;
        return;
#endif

        if (currentIndex != -1) 
            previews[currentIndex].SetActive(false);
        currentIndex = index;
        if (currentIndex != -1)
            previews[currentIndex].SetActive(true);
    }
}
