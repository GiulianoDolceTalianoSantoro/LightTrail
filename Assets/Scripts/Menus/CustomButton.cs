using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    private RectTransform rectTransform;
    public bool isHovered = false;
    public bool isClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        IsOver();
        onClick();
    }

    bool IsOver()
    {
        var mousePosition = Input.mousePosition;

        if (mousePosition.x >= this.transform.position.x - rectTransform.rect.width / 2 &&
            mousePosition.x <= this.transform.position.x + rectTransform.rect.width &&
            mousePosition.y >= this.transform.position.y - rectTransform.rect.height / 2 &&
            mousePosition.y <= this.transform.position.y + rectTransform.rect.height)
        {
            isHovered =  true;
        }
        else
        {
            isHovered = false;
        }

        return isHovered;
    }

    void onClick()
    {
        if (Input.GetMouseButtonDown(0) && IsOver())
        {
            isClicked = true;
            Invoke("changeBool", .05f);
        }
    }

    void changeBool()
    {
        if (!isClicked)
        {
            isClicked = true;
        }
        else
        {
            isClicked = false;
        }
    }
}
