using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoanPanelPrefab : MonoBehaviour
{
    // Private variables
    private Canvas parentCanvas;
    private CanvasGroup canvasGroup;
    private int sortOrder;

    // TMP_Text elements
    public Canvas customerCanvas;
    public Canvas textCanvas;

    // Prefab elements
    public Global.Customer customer;
    public GameObject customerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update sort order variable
        sortOrder = GetComponentInParent<Canvas>().sortingOrder;

        // Keep text and photo above sprite
        customerCanvas.sortingOrder = sortOrder + 1;
    }

    public void SetContent(Global.Customer customer, Canvas canvas)
    {
        // Text elements

        // Customer elements
        CustomerPrefab customerScript = customerPrefab.GetComponent<CustomerPrefab>();
        customerScript.GenerateSprite(customer, customerCanvas);
    }
}
