using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpeedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Stage3.speedOn = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Stage3.speedOn = false;
    }
}
