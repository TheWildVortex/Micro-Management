using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Stage1 : MonoBehaviour
{
    // Stage 1 Variables
    public static float currentBudget;
    public static float totalLoanAmount;
    public int totalLoanCount;
    public TMP_Text currentBudgetText;
    public TMP_Text totalLoanAmountText;
    public TMP_Text totalLoanCountText;

    // Declare Prefab and Canvas
    public GameObject loanAppPrefab;
    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        // -------------------------------------------------STAGE 1 TESTING----------------------------------------

       /* // Set variables:
        string mfi = "Name";
        int testBudget = 50000;
        int number = 10;
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

        // -------------------------------------------------STAGE 1 TESTING----------------------------------------

        // Retrieve Global player and customers data
        var player = Global.PlayerData;
        var customers = Global.CustomersData;

        currentBudget = player.Budget;
        currentBudgetText.text = currentBudget.ToString("n2");

        foreach (Global.Customer customer in customers)
        {
            Debug.Log("----- Customer Info -----");
            Debug.Log("Name: " + customer.FirstName + " " + customer.LastName);
            Debug.Log("Loan Amount: " + customer.LoanAmount);
            Debug.Log("Business: " + customer.Business);
            Debug.Log("ID Used: " + customer.IDUsed.IDType);
            Debug.Log("Bus Address: " + customer.BusAddress);
            Debug.Log("Home Address: " + customer.HomeAddress);
            Debug.Log("Contact Info: " + customer.ContactInfo);
            Debug.Log("Real: " + customer.Real);
            Debug.Log("Valid: " + customer.Valid);
            Debug.Log("FSE: " + customer.Rate);
            Debug.Log("ELS: " + customer.Frequency);
            Debug.Log("Stop Chance: " + customer.StopChance);

            // Instantiate the LoanApp prefab with an adjusted position
            Vector3 spawnPosition = new Vector3(-700f, 0f, 0f);
            GameObject newLoanApp = Instantiate(loanAppPrefab, spawnPosition, Quaternion.identity);
            newLoanApp.transform.SetParent(canvas.transform, false); // Set the parent to the Canvas
            LoanAppPrefab loanAppScript = newLoanApp.GetComponent<LoanAppPrefab>();
            loanAppScript.SetContent(customer, canvas);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var customers = Global.CustomersData;

        // Calculate totalLoanAmount for approved customers using LINQ
        totalLoanAmount = customers.Where(customer => customer.Approved).Sum(customer => customer.LoanAmount);
        totalLoanAmountText.text = totalLoanAmount.ToString("n2");

        totalLoanCount = customers.Count(customer => customer.Approved);
        totalLoanCountText.text = totalLoanCount.ToString();

        // Ensure that the player knows they are overbudget
        if (totalLoanAmount > currentBudget)
        {
            totalLoanAmountText.faceColor = new Color32(255, 0, 0, 255);
        }
        else
        {
            totalLoanAmountText.faceColor = new Color32(0, 0, 0, 255);
        }
    }
}
