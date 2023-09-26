using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Stage3 : MonoBehaviour
{
    // Stage 3 Variables
    private float currentBudget;
    public static bool stageFinish = false;

    // HEADER ELEMENTS
    // CountDown Variables
    public static float currentTime;
    public float countdownTime = 100;
    public TMP_Text countdownText;
    // Data Variables
    private int loansRemain = 0;
    private int loansClosed = 0;
    private int loansDropped = 0;
    private int demandLetters = 0;
    public TMP_Text loansRemainText;
    public TMP_Text loansClosedText;
    public TMP_Text loansDroppedText;
    public TMP_Text demandLettersText;

    // SCROLLVIEW ELEMENTS
    public Canvas dashboardCanvas;
    public GameObject loanPanelPrefab;
    public Transform contentContainer;

    // FOOTER ELEMENTS
    // Pause Button
    public Button pauseButton;
    public TMP_Text pauseButtonText;
    // Speed Button
    private float speedScale = 2f;
    public Button speedButton;
    public static bool speedOn = false;
    // Continue Button
    public Button continueButton;

    // Start is called before the first frame update
    void Start()
    {
        // -------------------------------------------------STAGE 3 TESTING----------------------------------------

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

        // -------------------------------------------------STAGE 3 TESTING----------------------------------------

        // Retrieve Global player and customers data
        var player = Global.PlayerData;
        var customers = Global.CustomersData;

        // Set status values
        currentBudget = player.Budget;

        // Set timer
        currentTime = countdownTime;

        // Set button
        pauseButton.onClick.AddListener(PauseTime);
        pauseButtonText.text = "II";

        // Process customers
        ProcessCustomers(customers);
    }

    // Update is called once per frame
    void Update()
    {
        // Update countdown timer
        CountDown();

        // Update status information
        UpdateText();

        // Speed up functionality
        if (speedOn)
        {
            SpeedUp();  
        }
        else
        {
            SpeedDown();
        }

        // Once the stage is complete
        if (stageFinish)
        {
            CompleteStage();
        }
    }

    // Process all customers
    private void ProcessCustomers(List<Global.Customer> customers)
    {
        foreach (Global.Customer customer in customers)
        {
            // Declare the desk canvas
            var canvas = dashboardCanvas;
            var content = contentContainer;

            Debug.Log("----- Customer Info -----");
            Debug.Log("Name: " + customer.FirstName + " " + customer.LastName);
            Debug.Log("Age: " + customer.Age);
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

            // Customer Appearance Details
            Debug.Log("----- Customer Appearance Details -----");
            Debug.Log("Clothes: " + customer.Clothes);
            Debug.Log("Clothes Color: " + customer.ClothesColor);
            Debug.Log("Skin Type: " + customer.SkinType);
            Debug.Log("Body Color: " + customer.BodyColor);
            Debug.Log("Eye Color: " + customer.EyeColor);
            Debug.Log("Eyes: " + customer.Eyes);
            Debug.Log("Nose: " + customer.Nose);
            Debug.Log("Mouth: " + customer.Mouth);
            Debug.Log("Sex: " + customer.Sex);
            Debug.Log("Hair Color: " + customer.HairColor);
            Debug.Log("Brows: " + customer.Brows);
            Debug.Log("Bangs: " + customer.Bangs);
            Debug.Log("Hair: " + customer.Hair);
            Debug.Log("Hair Extension: " + customer.HairExtension);

            // Instantiate the Loan panel inside the Content of the ScrollView
            GameObject newLoanPanel = Instantiate(loanPanelPrefab, content);
            LoanPanelPrefab loanPanelScript = newLoanPanel.GetComponent<LoanPanelPrefab>();
            loanPanelScript.SetContent(customer, canvas);
        }
    }

    // Countdown function
    private void CountDown()
    {
        // Check if all customers have customer.Active = false
        var customers = Global.CustomersData;
        bool customersComplete = customers.All(customer => !customer.Active);

        // If the timer reaches 0
        if (Time.timeScale == 0f)
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0 || customersComplete)
        {
            currentTime = 0;
            Time.timeScale = 0f;
            stageFinish = true;
        }

        countdownText.text = Mathf.FloorToInt(currentTime).ToString() + " Days Left";
    }

    // Update text function
    private void UpdateText()
    {
        var customers = Global.CustomersData;

        // Data update
        loansRemain = customers.Count(customer => !customer.Paid);
        loansClosed = customers.Count(customer => customer.Paid);

        // Text update
        loansRemainText.text = "Loans Remaining: " + loansRemain.ToString();
        loansClosedText.text = "Loans Closed: " + loansClosed.ToString();
        loansDroppedText.text = "Loans Dropped: " + loansDropped.ToString();
        demandLettersText.text = "Demand Letters Used: " + demandLetters.ToString();
    }

    // Close the stage
    private void CompleteStage()
    {
        Debug.Log("Stage Complete!");

        // Activate/deactivate buttons
        pauseButton.interactable = false;
        speedButton.interactable = false;
        continueButton.gameObject.SetActive(true);
    }

    // Speed Up Function
    public void SpeedUp()
    {
        Global.SpeedGame(speedScale);
    }

    // Speed Down Function
    public void SpeedDown()
    {
        Global.ResetSpeedGame();
    }

    // Pause Time Function
    public void PauseTime()
    {
        Time.timeScale = 0f;
        pauseButton.onClick.AddListener(PlayTime);
        pauseButtonText.text = ">";
    }

    //Play Time Function
    public void PlayTime()
    {
        Time.timeScale = 1f;
        pauseButton.onClick.AddListener(PauseTime);
        pauseButtonText.text = "II";
    }

    // Deactivate all buttons
    public void DeactivateButtons()
    {
        // Find all buttons in the Canvas
        Button[] buttons = dashboardCanvas.GetComponentsInChildren<Button>();

        // Find all ScrollRects in the Canvas
        ScrollRect[] scrollRects = dashboardCanvas.GetComponentsInChildren<ScrollRect>();

        // Deactivate each button
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }

        // Deactivate each ScrollRect
        foreach (ScrollRect scrollRect in scrollRects)
        {
            // Disable scrolling
            scrollRect.vertical = false;
        }
    }

    // Reactivate all buttons
    public void ReactivateButtons()
    {
        // Find all buttons in the Canvas
        Button[] buttons = dashboardCanvas.GetComponentsInChildren<Button>();

        // Find all ScrollRects in the Canvas
        ScrollRect[] scrollRects = dashboardCanvas.GetComponentsInChildren<ScrollRect>();

        // Activate each button
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        // Activate each ScrollRect
        foreach (ScrollRect scrollRect in scrollRects)
        {
            // Enable scrolling
            scrollRect.vertical = true;
        }
    }

    // Adjust Dropped loans
    public void DropCount(int number)
    {
        loansDropped += number;
    }

    // Adjust number of demand letters sent
    public void LetterCount(int number)
    {
        demandLetters += number;
    }
}
