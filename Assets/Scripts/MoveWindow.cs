using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
{

    [SerializeField] private RectTransform dragRectTrans;
    [SerializeField] private Canvas canvas;

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        dragRectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
            
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragRectTrans.SetAsLastSibling();
    }

}
