using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SchoolIDPrefab : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler//, IPointerDownHandler
{
    // Private variables
    private Canvas parentCanvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool enlarged;
    private bool canDrag = true;
    private int sortOrder;

    // Add TMP_Text components for common ID elements
    public Global.Customer idCustomer;
    public GameObject photoPrefab;
    public TMP_Text idName;
    public TMP_Text idNumber;
    public Canvas photoCanvas;
    public Canvas textCanvas;

    // Add TMP_Text components for special ID elements
    public TMP_Text idSchoolName;
    public TMP_Text idSchoolCity;
    public TMP_Text idSchoolSection;
    public TMP_Text idSchoolAdviser;
    public TMP_Text idSchoolYear;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Update sort order variable
        sortOrder = GetComponent<SpriteRenderer>().sortingOrder;

        // Keep text and photo above sprite
        textCanvas.sortingOrder = sortOrder;
        photoCanvas.sortingOrder = sortOrder + 1;

        if (Global.IsGamePaused())
        {
            canDrag = false;
        }
        else
        {
            canDrag = true;
        }
    }

    public void SetContent(Global.Customer customer, Canvas canvas)
    {
        // Common elements
        idCustomer = customer;
        idName.text = customer.IDUsed.IDFirstName + " " + customer.IDUsed.IDLastName;
        idNumber.text = "ID No. " + customer.IDUsed.IDNumber;
        parentCanvas = canvas;
        canvasGroup = canvas.GetComponent<CanvasGroup>();

        // Special elements
        idSchoolCity.text = customer.IDUsed.SchoolCity + " City";
        idSchoolSection.text = customer.IDUsed.SchoolSection;
        idSchoolAdviser.text = customer.IDUsed.SchoolAdviser;
        idSchoolYear.text = customer.IDUsed.SchoolYear;

        // ID photo elements
        IDPhotoPrefab photoScript = photoPrefab.GetComponent<IDPhotoPrefab>();
        photoScript.GenerateIDPhoto(customer, photoCanvas);
        photoScript.KidIDPhoto(customer.IDUsed.IDSex, customer.IDUsed.IDTypeNumber);

        // Change details for elementary and high schools
        switch (customer.IDUsed.IDTypeNumber)
        {
            case 5:
                idSchoolName.text = customer.IDUsed.SchoolName + " Elementary School";
                break;

            case 6:
                idSchoolName.text = customer.IDUsed.SchoolName + " High School";
                break;
        }

        // Update the sorting order of the sprite based on the sibling index
        GetComponent<SpriteRenderer>().sortingOrder = transform.GetSiblingIndex() + 1;
        textCanvas.sortingLayerName = "IDLayer";
        photoCanvas.sortingLayerName = "IDLayer";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }

        Debug.Log("OnBeginDrag");
        rectTransform.localScale = new Vector2(8f, 24f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }

        Debug.Log("OnDrag");
        Vector2 newPos = rectTransform.anchoredPosition + eventData.delta / parentCanvas.scaleFactor;

        // Get the canvas's RectTransform
        RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();

        // Calculate the minimum and maximum positions within the canvas
        float halfWidth = rectTransform.rect.width / 2f;
        float halfHeight = rectTransform.rect.height / 2f;
        float minX = canvasRect.rect.xMin + halfWidth;
        float maxX = canvasRect.rect.xMax - halfWidth;
        float minY = canvasRect.rect.yMin + halfHeight;
        float maxY = canvasRect.rect.yMax - halfHeight;

        // Clamp the position to stay within the canvas boundaries
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        rectTransform.anchoredPosition = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }

        Debug.Log("OnEndDrag");
        rectTransform.localScale = new Vector2(4f, 12f);
    }

    /*public void OnPointerDown(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }

        Debug.Log("OnPointerDown");
        if (!enlarged)
        {
            rectTransform.localScale = new Vector2(8f, 24f);
            enlarged = true;
        }
        else if (enlarged)
        {
            rectTransform.localScale = new Vector2(4f, 12f);
            enlarged = false;
        }
    }*/
}
