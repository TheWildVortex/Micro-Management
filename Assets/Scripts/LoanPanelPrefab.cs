using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoanPanelPrefab : MonoBehaviour
{
    // Private variables
    private Canvas parentCanvas;
    private CanvasGroup canvasGroup;
    private int sortOrder;
    private Global.Customer loanCustomer;
    private bool stageEnd = false;

    // Button elements
    public Button loanDetailsButton;
    public GameObject loanDetailsPrefab;

    // TMP_Text elements
    public Canvas textCanvas;
    public TMP_Text loanName;
    public TMP_Text loanAmountNumber;

    // Progress Bar elements
    private float targetAmount;
    private float currentAmount;
    private float paymentRate;
    private float paymentFrequency;
    private float paymentStopChance;
    public Slider progressBar;

    // Awake function
    void Awake()
    {
        // Add listener for loan details button
        loanDetailsButton.onClick.AddListener(OpenDetails);
    }

    // Update is called once per frame
    void Update()
    {
        // Update sort order variable
        sortOrder = GetComponentInParent<Canvas>().sortingOrder;

        // Keep text and photo above sprite
        textCanvas.sortingOrder = sortOrder;

        // Check if the stage is not finished yet
        if (!Stage3.stageFinish)
        {
            FillBar(); // Call FillBar continuously until stageFinish becomes true
        }
        else if (!stageEnd)
        {
            FillBar();
            stageEnd = true; 
        }
    }

    // Set text and progress bar elements
    public void SetContent(Global.Customer customer, Canvas canvas)
    {
        // Customer
        loanCustomer = customer;

        // Text elements
        loanName.text = customer.FirstName + " " + customer.LastName;

        // Probability elements
        targetAmount = customer.LoanAmount;
        currentAmount = 0;
        paymentRate = customer.Rate;
        paymentFrequency = customer.Frequency;
        paymentStopChance = customer.StopChance;

        // Adjust progress bar
        progressBar.value = currentAmount;
        progressBar.maxValue = targetAmount;
    }

    // Bar fills according to the customer's probabilities
    private void FillBar()
    {
        // Check if Loan is still active
        if (loanCustomer.Dropped || loanCustomer.Active && Stage3.stageFinish)
        {
            // Red highlight means incomplete
            loanCustomer.Active = false;
            loanAmountNumber.faceColor = new Color32(250, 0, 0, 255);

            // Adjust amount earned
            loanCustomer.AmountEarned = ApplyMultipliers(loanCustomer);
            Debug.Log("Amount Earned: " + loanCustomer.AmountEarned.ToString("n2"));

            return;
        }

        // Update progress text
        loanAmountNumber.text = "₱" + currentAmount.ToString("n2") + " / ₱" + targetAmount.ToString("n2");

        // Declare variables
        var fillSpeed = loanCustomer.LoanAmount * paymentRate;
        var slider = progressBar;

        if (currentAmount != targetAmount)
        {
            if (currentAmount < targetAmount && UnityEngine.Random.value < paymentFrequency)
            {
                currentAmount += fillSpeed * Time.deltaTime;
                if (currentAmount > targetAmount) currentAmount = targetAmount;
            }
            else
            {
                currentAmount -= fillSpeed * Time.deltaTime;
                if (currentAmount < targetAmount) currentAmount = targetAmount;
            }
            slider.value = currentAmount;
        }

        else
        {
            // Update status
            loanCustomer.Paid = true;
            loanCustomer.Active = false;
            loanCustomer.AmountEarned = ApplyMultipliers(loanCustomer);
            Debug.Log("Amount Earned: " + loanCustomer.AmountEarned.ToString("n2"));

            // Green highlight upon completion
            loanAmountNumber.faceColor = new Color32(0, 250, 0, 255);

            // Ensure that the slider is uninteractable after completing
            slider.value = slider.maxValue;
        }
    }

    private void OpenDetails()
    {
        // Generate dialog and script
        var details = Instantiate(loanDetailsPrefab);
        var detailsScript = details.GetComponent<LoanDetailsPrefab>();

        // Set loan details
        detailsScript.SetCustomer(loanCustomer);
    }

    private float ApplyMultipliers(Global.Customer customer)
    {
        Debug.Log("Original Amount Earned: " + customer.AmountEarned.ToString("n2"));
        customer.AmountEarned = (customer.AmountEarned - customer.LoanAmount) + currentAmount;
        Debug.Log("Applied CurrentAmount: " + currentAmount.ToString("n2"));
        Debug.Log("Applied Loan Amount: " + customer.LoanAmount.ToString("n2"));
        Debug.Log("Applied Amount Earned: " + customer.AmountEarned.ToString("n2"));

        if (customer.Demanded)
        {
            customer.AmountEarned -= customer.LoanAmount * Global.demandLetterMultiplier;
            Debug.Log("Letter Amount Earned: " + customer.AmountEarned.ToString("n2"));
        }

        if (customer.Dropped)
        {
            customer.AmountEarned += customer.LoanAmount * Global.droppedMultiplier;
            Debug.Log("Dropped Amount Earned: " + customer.AmountEarned.ToString("n2"));
        }

        return customer.AmountEarned;
    }
}
