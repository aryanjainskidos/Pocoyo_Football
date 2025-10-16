using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleSnapScroll : MonoBehaviour
{
    public RectTransform ViewPort;
    public RectTransform Content;
    public float swipeTime = 0.12f;

    int TopLimitIndex = 0;
    int TargetIndex = 0;
   
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;

        float viewPortWidth = ViewPort.sizeDelta.x;
        float elementWidth = Content.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;

        int numOfViewElements = Mathf.RoundToInt(viewPortWidth / elementWidth);

        int children = Content.childCount;
        int totalVisibleElement = 0;
        for (int i = 0; i < children; i++)
        {
            if (Content.GetChild(i).gameObject.activeSelf)
            {
                totalVisibleElement++;
            }
        }

        TopLimitIndex = totalVisibleElement - numOfViewElements;
    }

    // Update is called once per frame
    void Update()
    {
        //if(CurrentIndex != TargetIndex)
        //{
        //    Content.localPosition -= new Vector3(150f * Time.deltaTime, 0, 0);
        //}
    }

    public void Next()
    {
        TargetIndex = Mathf.Clamp( TargetIndex + 1, 0, TopLimitIndex);
        Content.anchoredPosition = new Vector3( -150f * TargetIndex, 0f, 0f);
    }

    public void Prev()
    {
        TargetIndex = Mathf.Clamp(TargetIndex - 1, 0, TopLimitIndex);
        Content.anchoredPosition = new Vector3( -150f * TargetIndex, 0f, 0f);
    }
}
