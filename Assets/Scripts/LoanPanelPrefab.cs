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

        // Ensure that the variable is set
        stageEnd = false;
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
        paymentRate = customer.Rate * customer.LoanAmount;
        paymentFrequency = customer.Frequency;
        paymentStopChance = customer.StopChance;

        // Adjust progress bar
        progressBar.value = currentAmount;
        progressBar.maxValue = targetAmount;

        // Initial StopChance check
        CheckStopChance(customer);

        Debug.Log("Amount Earned: " + customer.AmountEarned.ToString("n2"));
    }

    // Bar fills according to the customer's probabilities
    private void FillBar()
    {
        // Ensure variables are updated
        paymentRate = loanCustomer.Rate * loanCustomer.LoanAmount;
        paymentFrequency = loanCustomer.Frequency;
        paymentStopChance = loanCustomer.StopChance;

        // Declare current time
        var time = Stage3.currentTime;

        if (time / 25 == 0)
        {
            // StopChance check
            CheckStopChance(loanCustomer);
        }

        // Check if Loan is still active
        if (loanCustomer.Dropped || (Stage3.stageFinish && !loanCustomer.Paid))
        {
            // Adjust amount earned
            loanCustomer.AmountEarned = currentAmount;
            loanCustomer.AmountEarned = ApplyMultipliers(loanCustomer);
            Debug.Log("Amount Earned: " + loanCustomer.AmountEarned.ToString("n2"));

            // Red highlight means incomplete
            loanCustomer.Active = false;
            loanAmountNumber.faceColor = new Color32(250, 0, 0, 255);
            return;
        }

        // Update progress text
        loanAmountNumber.text = "₱" + currentAmount.ToString("n2") + " / ₱" + targetAmount.ToString("n2");

        // Declare variables
        var fillSpeed = paymentRate;
        var slider = progressBar;

        Debug.Log("Pay Rate: " + paymentRate.ToString());

        if (currentAmount != targetAmount)
        {
            if (currentAmount < targetAmount && UnityEngine.Random.value < paymentFrequency)
            {
                currentAmount += fillSpeed * Time.deltaTime;
                if (currentAmount > targetAmount) currentAmount = targetAmount;
            }
            else if (currentAmount > targetAmount)
            {
                currentAmount -= fillSpeed * Time.deltaTime;
                if (currentAmount < targetAmount) currentAmount = targetAmount;
            }
            slider.value = currentAmount;
        }
        else if (currentAmount == targetAmount)
        {
            if (!loanCustomer.Paid) // Check if not already paid
            {
                // Update status
                loanCustomer.Paid = true;
                loanCustomer.Active = false;
                loanAmountNumber.faceColor = new Color32(0, 250, 0, 255);

                // Ensure that the slider is uninteractable after completing
                slider.value = slider.maxValue;
            }
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

    private void CheckStopChance(Global.Customer customer)
    {
        if (UnityEngine.Random.value < customer.StopChance)
        {
            customer.Frequency = 0f;
            Debug.Log("WHERE'D THEY GO????");
        }
    }

    private float ApplyMultipliers(Global.Customer customer)
    {
        Debug.Log("Original Amount Earned: " + customer.AmountEarned.ToString("n2"));

        // Check if demanded and apply multiplier
        if (customer.Demanded)
        {
            customer.AmountEarned -= customer.LoanAmount * Global.demandLetterMultiplier;
            Debug.Log("Applied Demanded Amount: " + customer.AmountEarned.ToString("n2"));
        }

        // Check if dropped and apply multiplier
        if (customer.Dropped)
        {
            customer.AmountEarned += customer.LoanAmount * Global.droppedMultiplier;
            Debug.Log("Applied Dropped Amount: " + customer.AmountEarned.ToString("n2"));
        }

        return customer.AmountEarned;
    }
}
