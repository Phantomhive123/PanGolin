#if UNITY_ANDROID && !UNITY_EDITOR
#define ANDROID
#endif

#if UNITY_IPHONE && !UNITY_EDITOR
#define IPHONE
#endif

using System;
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
            previews[currentIndex].GetComponent<RectTransform>().position = GetNearestGrid(Pos);
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
                    Vector3 screenPos = previews[currentIndex].GetComponent<RectTransform>().position;
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                    if (!CheckGrid(worldPos)) return;
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

    private Vector3 GetNearestGrid(Vector3 mousePos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        float x = Mathf.Floor(worldPos.x);
        float y = Mathf.Floor(worldPos.y);
        Vector3 ans = new Vector3(x + 0.5f, y + 0.5f, worldPos.z);
        mousePos = Camera.main.WorldToScreenPoint(ans);
        return mousePos;
    }

    private bool CheckGrid(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast( pos, Vector2.zero);
        return hit.collider == null;
    }
}
