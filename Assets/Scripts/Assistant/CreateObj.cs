﻿#if UNITY_ANDROID && !UNITY_EDITOR
#define ANDROID
#endif

#if UNITY_IPHONE && !UNITY_EDITOR
#define IPHONE
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateObj : MonoBehaviour
{

    private static CreateObj _instance;
    [SerializeField]
    private GameObject[] objs;
    [SerializeField] 
    private GameObject[] previews;
    private int currentIndex = -1;

    public int[] maxNums;
    private List<int> currentNums;
    public Text[] texts;

    public static CreateObj Instance
    {
        get { return _instance; }
    }
    public int GetRemainWood()
    {
        return maxNums[0] - int.Parse(texts[0].text);
    }
    public int GetRemainStone()
    {
        return maxNums[1] - int.Parse(texts[1].text);
    }

    void Start()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        currentNums = new List<int>();
        for (int i = 0; i < maxNums.Length; i++)
        {
            currentNums.Add(maxNums[i]);
            texts[i].text = currentNums[i] + "";
        }

    }

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
                    if (currentNums[currentIndex] - 1 < 0) return;
                    Vector3 screenPos = previews[currentIndex].GetComponent<RectTransform>().position;
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                    if (!CheckGrid(worldPos)) return;
                    worldPos.z = 0;
                    currentNums[currentIndex]--;
                    texts[currentIndex].text = currentNums[currentIndex] + "";
                    Instantiate(objs[currentIndex], worldPos, Quaternion.identity);
                }
            }
        }
    }

    private void RenderSaveObject(ElementType type, Vector3 pos)
    {
        int index = Convert.ToInt32(type);
        previews[index].GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(pos);
        Instantiate(objs[index], pos, Quaternion.identity);
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
