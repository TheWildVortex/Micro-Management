using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SeniorIDPrefab : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler//, IPointerDownHandler
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
    public TMP_Text idBday;
    public TMP_Text idAge;
    public TMP_Text idSex;
    public TMP_Text idAddress;
    public TMP_Text idNumber;
    public TMP_Text idIssueDate;
    public Canvas photoCanvas;
    public Canvas textCanvas;

    // Add TMP_Text components for special ID elements
    public TMP_Text idContactName;
    public TMP_Text idContactNumber;

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
        switch (customer.IDUsed.IDSex)
        {
            case 0:
                idSex.text = "M";
                break;

            case 1:
                idSex.text = "F";
                break;
        }
        idAddress.text = customer.IDUsed.IDAddress;
        idNumber.text = customer.IDUsed.IDNumber;
        idBday.text = customer.IDUsed.IDBday.ToString("MM / dd / yyy");
        idAge.text = customer.IDUsed.IDAge.ToString();
        idIssueDate.text = customer.IDUsed.IDIssueDate.ToString("MM / dd / yyy");
        parentCanvas = canvas;
        canvasGroup = canvas.GetComponent<CanvasGroup>();

        // Special elements
        idContactName.text = customer.IDUsed.EmergencyContactName;
        idContactNumber.text = customer.IDUsed.EmergencyContactNumber;

        // ID photo elements
        IDPhotoPrefab photoScript = photoPrefab.GetComponent<IDPhotoPrefab>();
        photoScript.GenerateIDPhoto(customer, photoCanvas);

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
        rectTransform.localScale = new Vector2(25f, 25f);
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
        rectTransform.localScale = new Vector2(5.12f, 5.12f);
    }

   /* public void OnPointerDown(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }

        Debug.Log("OnPointerDown");
        if (!enlarged)
        {
            rectTransform.localScale = new Vector2(25f, 25f);
            enlarged = true;
        }
        else if (enlarged)
        {
            rectTransform.localScale = new Vector2(5.12f, 5.12f);
            enlarged = false;
        }
    }*/
}
