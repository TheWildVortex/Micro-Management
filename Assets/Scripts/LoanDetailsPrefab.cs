using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class LoanDetailsPrefab : MonoBehaviour
{
    // Loan Details variables
    private int sortOrder;
    private Stage3 stage3;
    private Global.Customer loanCustomer;

    public TMP_Text loanNameText;
    public TMP_Text loanStatusText;
    public TMP_Text loanDetailsText;
    public Button demandLetterButton;
    public Button dropLoanButton;
    public Button closeButton;
    public GameObject customerPrefab;
    public GameObject dialogPrefab;
    public Canvas customerCanvas;
    public Canvas detailsTextCanvas;

    void Awake()
    {
        // Assign the main camera to the canvas
        GetComponent<Canvas>().worldCamera = Camera.main;

        // Get sort order
        sortOrder = GetComponent<Canvas>().sortingOrder;

        // Find the script
        stage3 = FindObjectOfType<Stage3>();

        // Ensure that game is paused as this requires player input to continue
        stage3.PauseTime();
        stage3.DeactivateButtons();

        // Set listeners
        demandLetterButton.onClick.AddListener(() => GenerateDialog(SendLetter, "You may only send one demand letter to the customer. Do you want to continue?"));
        dropLoanButton.onClick.AddListener(() => GenerateDialog(DropLoan, "Dropping the loan will permanently close it before full payment has been made with the collateral being collected as additional payment. Do you want to continue?"));
        closeButton.onClick.AddListener(CloseDialog);
    }

    // Update is called once per frame
    void Update()
    {
        // Keep text and photo above sprite
        customerCanvas.sortingOrder = sortOrder + 5;

        // Keep the background paused
        stage3.PauseTime();

        // Update loan status
        SetStatus(loanCustomer);

        // Deactivate buttons if stage is over
        if (Stage3.stageFinish)
        {
            loanCustomer.Active = false;
            dropLoanButton.interactable = false;
            demandLetterButton.interactable = false;
        }
    }

    // Set customer details
    public void SetCustomer(Global.Customer customer)
    {
        // Customer
        loanCustomer = customer;

        // Text elements
        loanNameText.text = customer.FirstName + " " + customer.LastName;

        // Customer elements
        CustomerPrefab customerScript = customerPrefab.GetComponent<CustomerPrefab>();
        customerScript.GenerateSprite(customer, customerCanvas);

        // Check customer status to activate buttons
        CheckStatus(customer);
    }

    private void SetStatus(Global.Customer customer)
    {
        if (customer.Active && !customer.Paid)
        {
            loanStatusText.text = "Loan Active";
            loanStatusText.faceColor = new Color32(0, 250, 0, 255);
        }
        if (customer.Paid && !customer.Active)
        {
            loanStatusText.text = "Loan Fulfilled";
            loanStatusText.faceColor = new Color32(0, 250, 0, 255);

            // Adjust amount earned
            if (Stage3.currentTime >= 3)
            {
                customer.AmountEarned += customer.LoanAmount * Global.amountEarnedMultiplier;
                Debug.Log("Amount Earned: " + customer.AmountEarned.ToString("n2"));
            }
            else
            {
                customer.AmountEarned += customer.LoanAmount * (Global.amountEarnedMultiplier / 2);
                Debug.Log("Amount Earned: " + customer.AmountEarned.ToString("n2"));
            }
        }

        if (!customer.Paid && !customer.Active)
        {
            loanStatusText.text = "Loan Unfulfilled";
            Debug.Log("Amount Earned: " + customer.AmountEarned.ToString("n2"));
            loanStatusText.faceColor = new Color32(250, 0, 0, 255);
        }

        if (customer.Dropped)
        {
            loanStatusText.text = "Loan Dropped";
            Debug.Log("Amount Earned: " + customer.AmountEarned.ToString("n2"));
            loanStatusText.faceColor = new Color32(250, 0, 0, 255);
        }
    }

    // Send a demand letter
    private void SendLetter()
    {
        Debug.Log("Rate: " + loanCustomer.Rate);
        Debug.Log("Frequency: " + loanCustomer.Frequency);
        Debug.Log("StopChance: " + loanCustomer.StopChance);

        // Edit customer variables
        loanCustomer.Demanded = true;
        if (UnityEngine.Random.value < loanCustomer.ValidChance) { loanCustomer.Rate *= 2; }
        if (UnityEngine.Random.value < loanCustomer.ValidChance) { loanCustomer.Frequency *= 2; }
        if (UnityEngine.Random.value < loanCustomer.RealChance) { loanCustomer.StopChance *= 2; }

        Debug.Log("New Rate: " + loanCustomer.Rate);
        Debug.Log("New Frequency: " + loanCustomer.Frequency);
        Debug.Log("New StopChance: " + loanCustomer.StopChance);

        // Add to count
        stage3.LetterCount(1);

        // Deactivate button
        demandLetterButton.interactable = false;
        dropLoanButton.interactable = true;
    }

    // Drop the loan
    private void DropLoan()
    {
        // Edit customer variables
        loanCustomer.Paid = false;
        loanCustomer.Active = false;
        loanCustomer.Demanded = true;
        loanCustomer.Dropped = true;

        // Add to count
        stage3.DropCount(1);

        // Deactivate buttons
        demandLetterButton.interactable = false;
        dropLoanButton.interactable = false;
    }

    // Close the details window
    private void CloseDialog()
    {
        // Unpause the game
        stage3.PlayTime();
        stage3.ReactivateButtons();
        Destroy(gameObject);
    }

    // Check the status and hide the necessary buttons
    private void CheckStatus(Global.Customer customer)
    {
        if (customer.Active)
        {
            dropLoanButton.interactable = true;
        }

        if (customer.Active && !customer.Demanded)
        {
            demandLetterButton.interactable = true;
        }
    }

    private void GenerateDialog(UnityEngine.Events.UnityAction function, string text)
    {
        demandLetterButton.interactable = false;
        dropLoanButton.interactable = false;

        // Generate dialog and script
        var dialog = Instantiate(dialogPrefab);
        var dialogScript = dialog.GetComponent<ConfirmDialogPrefab>();

        // Set dialog text
        dialogScript.SetText(dialogScript.dialogText, text);
        dialogScript.SetAction(function, ReturnFunction);
    }

    // General Return Function
    private void ReturnFunction()
    {
        demandLetterButton.interactable = true;
        dropLoanButton.interactable = true;

        Debug.Log("RETURN FUNCTION!");
        return;
    }
}
