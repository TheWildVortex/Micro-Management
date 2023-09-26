using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Stage4 : MonoBehaviour
{
    // Stage 4 Variables
    private float currentBudget;
    private float totalAmountSpent;
    private float totalAmountEarned;
    private int totalLoansCompleted;
    private int totalDemandLetters;
    private int totalLoansDropped;

    public TMP_Text currentBudgetText;
    public TMP_Text totalAmountSpentText;
    public TMP_Text totalAmountEarnedText;
    public TMP_Text totalLoansCompletedText;
    public TMP_Text totalDemandLettersText;
    public TMP_Text totalLoansDroppedText;

    public Button continueButton;
    public Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        // -------------------------------------------------STAGE 4 TESTING----------------------------------------

        /*// Set variables:
        string mfi = "Name";
        int testBudget = 50000;
        int number = 5;
        int minLoan = 5000;
        int maxLoan = 30000;
        float chanceForReal = 1f;
        float chanceForValid = 1f;

        // Generate new player and customers
        var player = Global.NewPlayer(mfi, testBudget);
        var customers = Global.GenerateCustomers(number, minLoan, maxLoan, chanceForReal, chanceForValid);

        // Clear Global data
        Global.ResetGlobalData();
        // Store data in Global
        Global.PlayerData = player;
        Global.CustomersData = customers;*/

        // -------------------------------------------------STAGE 4 TESTING----------------------------------------

        // Retrieve Global player and customers data
        var player = Global.PlayerData;
        var customers = Global.CustomersData;

        SetContent(player, customers);
    }

    private void SetContent(Global.Player player, List<Global.Customer> customers)
    {
        // Debug purposes
        foreach (Global.Customer customer in customers)
        {
            Debug.Log("Customer Dropped: " + customer.Dropped);
        }

        // Set status values
        totalAmountSpent = customers.Where(customer => customer.Approved).Sum(customer => customer.LoanAmount);
        totalAmountEarned = customers.Sum(customer => customer.AmountEarned);
        totalLoansCompleted = customers.Count(customer => customer.Paid);
        totalDemandLetters = customers.Count(customer => customer.Demanded);
        totalLoansDropped = customers.Count(customer => customer.Dropped);
        currentBudget = player.Budget + totalAmountEarned;

        // Set text
        currentBudgetText.text = "₱" + currentBudget.ToString("n2");
        totalAmountSpentText.text = "₱" + totalAmountSpent.ToString("n2");
        totalAmountEarnedText.text = "₱" + totalAmountEarned.ToString("n2");
        totalLoansCompletedText.text = totalLoansCompleted.ToString();
        totalDemandLettersText.text = totalDemandLetters.ToString();
        totalLoansDroppedText.text = totalLoansDropped.ToString();
    }
}
