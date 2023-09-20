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
    private Global.Customer customer;

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

    // Class elements
    public Global.Customer loanCustomer;

    // Awake function
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Update sort order variable
        sortOrder = GetComponentInParent<Canvas>().sortingOrder;

        // Keep text and photo above sprite
        textCanvas.sortingOrder = sortOrder;

        // Constantly update bar
        FillBar();
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
        // Update progress text
        loanAmountNumber.text = "₱" + currentAmount.ToString("n2") + " / ₱" + targetAmount.ToString("n2");

        // Declare variables
        var player = Global.PlayerData;
        var customers = Global.CustomersData;
        var fillSpeed = 1000; // paymentRate;
        var slider = progressBar;

        if (currentAmount != targetAmount)
        {
            if (currentAmount < targetAmount)
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
            // Update paid status
            loanCustomer.Paid = true;

            // Green highlight upon completion
            loanAmountNumber.faceColor = new Color32(0, 250, 0, 255);

            // Ensure that the slider is uninteractable after completing
            slider.value = slider.maxValue;
        }
    }
}
