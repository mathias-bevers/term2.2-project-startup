using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class PersonalCursorUI : MonoBehaviour
{
    [SerializeField] Image cursor;
    [SerializeField] float sensitivityX = 10000;
    [SerializeField] float sensitivityY = 10000;

    Vector2 cursorPos;
    Canvas c;

    public Vector2 getCursorPos
    {
        get
        {
            Rect rect = ((RectTransform)c.transform).rect ;
            return  new Vector2(Utils.Map(cursorPos.x, -(rect.width * 0.5f), rect.width * 0.5f, 0, Screen.width), Utils.Map(cursorPos.y, -(rect.height * 0.5f), rect.height * 0.5f, 0, Screen.height));
        }
    }

    private void OnEnable()
    {
        c = GetComponent<Canvas>();
        Rect rect = ((RectTransform)c.transform).rect;
        cursorPos = new Vector2(Utils.Map(Input.mousePosition.x, 0, Screen.width, -(rect.width*0.5f), rect.width * 0.5f), Utils.Map(Input.mousePosition.y, 0, Screen.height, -(rect.height * 0.5f), rect.height * 0.5f));
       // cursorPos = Vector2.zero;
    }

    private void OnDisable()
    {
        cursorPos = Vector2.zero;
    }

    float timeAccelerated = 0;

    private void Update()
    {
        if (cursor == null) return;
        float actWidth = ((RectTransform)c.transform).rect.width * 0.5f;
        float actHeight = ((RectTransform)c.transform).rect.height * 0.5f;
        Vector2 inputAxis = new Vector2(Input.GetAxisRaw("CameraX"), Input.GetAxisRaw("CameraY")).normalized;
        if (inputAxis.magnitude > 0.19f)
            timeAccelerated += Time.deltaTime;
        else timeAccelerated = 0;
        Vector2 sensitivyAxis = new Vector2(sensitivityX, sensitivityY) * Time.deltaTime * Mathf.Clamp(Utils.Map(timeAccelerated, 1, 1.5f, 1, 1.8f),1,1.8f);
        Vector2 axis = inputAxis * sensitivyAxis;
        cursorPos += axis;
        cursorPos = new Vector2(Mathf.Clamp(cursorPos.x, -actWidth, actWidth), Mathf.Clamp(cursorPos.y, -actHeight, actHeight)) ;

       ((RectTransform)cursor.transform).localPosition = cursorPos;

    }
}
