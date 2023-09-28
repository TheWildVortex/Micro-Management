using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LoanAppPrefab : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler//, IPointerDownHandler
{
    // Private variables
    private Canvas parentCanvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool enlarged;
    private bool canDrag = true;
    private SpriteRenderer sprite;
    private int sortOrder;

    // Add TMP_Text components for each text element
    public Global.Customer loanAppCustomer;
    public TMP_Text loanAppName;
    public TMP_Text loanAppBusiness;
    public TMP_Text loanAppAmount;
    public TMP_Text loanAppID;
    public Canvas textCanvas;
    public LayerMask panelLayerMask;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Update sort order variable
        sortOrder = GetComponent<SpriteRenderer>().sortingOrder;

        // Keep text above sprite
        textCanvas.sortingOrder = sortOrder;

        // Constantly check if game is paused
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
        loanAppCustomer = customer;
        loanAppName.text = customer.FirstName + " " + customer.LastName;
        loanAppBusiness.text = customer.Business;
        loanAppAmount.text = customer.LoanAmount.ToString("n2");
        loanAppID.text = customer.IDUsed.IDType;
        parentCanvas = canvas;
        canvasGroup = canvas.GetComponent<CanvasGroup>();

        // Update the sorting order of the sprite based on the sibling index
        GetComponent<SpriteRenderer>().sortingOrder = transform.GetSiblingIndex() + 1;
        textCanvas.sortingLayerName = "DocumentLayer";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }

        Debug.Log("OnBeginDrag");
        rectTransform.localScale = new Vector2(10f, 10f);
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

        // Get the position of the pointer in world space
        Vector3 pointerPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        pointerPosition.z = 0f; // Make sure the z-coordinate is 0 for 2D physics

        // Cast a ray to check for collisions with panels
        RaycastHit2D hit = Physics2D.Raycast(pointerPosition, Vector2.zero, 10f, panelLayerMask);
        Debug.Log("Raycast Origin: " + pointerPosition);
        Debug.Log("Hit: " + hit.collider);

        if (hit.collider != null)
        {
            Debug.Log("Hit collider: " + hit.collider.gameObject.name);

            // If dragged onto the passed panel
            if (hit.collider.gameObject.name == "PassPanel")
            {
                loanAppCustomer.Approved = true;
                Debug.Log(loanAppCustomer.Approved);
                loanAppCustomer.AmountEarned = ApplyMultipliers(loanAppCustomer);
            }
        }

        // If dragged outside passed panel
        else
        {
            loanAppCustomer.Approved = false;
            Debug.Log("FailPanel");

            // Reset amount earned
            loanAppCustomer.AmountEarned = loanAppCustomer.LoanAmount;
            Debug.Log("Amount Earned: " + loanAppCustomer.AmountEarned.ToString("n2"));
        }
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
            rectTransform.localScale = new Vector2(10f, 10f);
            enlarged = true;
        }
        else if (enlarged)
        {
            rectTransform.localScale = new Vector2(5.12f, 5.12f); 
            enlarged = false;
        }
    }*/

    private float ApplyMultipliers(Global.Customer customer)
    {
        // Adjust amount earned based on Real status
        if (customer.Real)
        {
            customer.AmountEarned += customer.LoanAmount * Global.amountEarnedMultiplier;
            Debug.Log("Amount Earned: " + customer.AmountEarned.ToString("n2"));
        }
        else
        {
            customer.AmountEarned -= customer.LoanAmount * (Global.amountEarnedMultiplier / 2);
            Debug.Log("Amount Earned: " + customer.AmountEarned.ToString("n2"));
        }

        // Adjust amount earned based on Valid status
        if (customer.Valid)
        {
            customer.AmountEarned += customer.LoanAmount * Global.amountEarnedMultiplier;
            Debug.Log("Amount Earned: " + customer.AmountEarned.ToString("n2"));
        }
        else
        {
            customer.AmountEarned -= customer.LoanAmount * (Global.amountEarnedMultiplier / 2);
            Debug.Log("Amount Earned: " + customer.AmountEarned.ToString("n2"));
        }

        return customer.AmountEarned;
    }
}