using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage2 : MonoBehaviour
{
    // Stage 2 Variables
    private bool buttonClicked = false;
    private bool customersComplete = false;
    private Global.Customer currentCustomer;
    private int currentBudget;
    private int totalLoanAmount;
    private int totalLoanCount;
    public TMP_Text currentBudgetText;
    public TMP_Text totalLoanAmountText;
    public TMP_Text totalLoanCountText;
    public Button ApproveButton;
    public Button RejectButton;
    public Button ContinueButton;

    // Declare Prefabs and Canvas
    public GameObject dialogPrefab;
    public GameObject customerPrefab;
    public GameObject loanAppPrefab;
    public GameObject driversLicensePrefab;
    public GameObject seniorIDPrefab;
    public GameObject nationalIDPrefab;
    public GameObject barangayIDPrefab;
    public GameObject passportPrefab;
    public GameObject schoolIDPrefab;
    public Canvas deskCanvas;
    public Canvas backgroundCanvas;

    // Start is called before the first frame update
    void Start()
    {
        // -------------------------------------------------STAGE 2 TESTING----------------------------------------

        /*// Set variables:
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

        // -------------------------------------------------STAGE 2 TESTING----------------------------------------

        // Retrieve Global player and customers data
        var player = Global.PlayerData;
        var customers = Global.CustomersData;

        // Set status values
        currentBudget = player.Budget;
        totalLoanAmount = 0;
        totalLoanCount = customers.Count;

        // Add event listeners to the buttons
        ApproveButton.onClick.AddListener(() =>
        {
            OnApproved();
        });
        RejectButton.onClick.AddListener(() =>
        {
            OnRejected();
        });

        // Process customers
        StartCoroutine(ProcessCustomers(customers));
    }

    // Process all customers
    IEnumerator ProcessCustomers(List<Global.Customer> customers)
    {
        foreach (Global.Customer customer in customers)
        {
            // Declare the desk canvas
            var canvas = deskCanvas;
            currentCustomer = customer;

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

            // Instantiate the LoanApp prefab as a child of the LoanAppContainer
            GameObject newLoanApp = Instantiate(loanAppPrefab, Vector3.zero, Quaternion.identity);
            newLoanApp.transform.SetParent(canvas.transform, false); // Set the parent to the Canvas
            LoanAppPrefab loanAppScript = newLoanApp.GetComponent<LoanAppPrefab>();
            loanAppScript.SetContent(customer, canvas);

            // Common ID details
            Debug.Log("----- Common ID Details -----");
            Debug.Log("First Name: " + customer.IDUsed.IDFirstName);
            Debug.Log("Last Name: " + customer.IDUsed.IDLastName);
            Debug.Log("Nationality: " + customer.IDUsed.IDNationality);
            Debug.Log("Sex: " + customer.IDUsed.IDSex);
            Debug.Log("Civil Status: " + customer.IDUsed.IDCivilStatus);
            Debug.Log("Address: " + customer.IDUsed.IDAddress);
            Debug.Log("Birth Place: " + customer.IDUsed.IDBirthPlace);
            Debug.Log("Birthday: " + customer.IDUsed.IDBday.ToShortDateString());
            Debug.Log("Age: " + customer.IDUsed.IDAge);
            Debug.Log("Issue Date: " + customer.IDUsed.IDIssueDate.ToShortDateString());
            Debug.Log("Expire Date: " + customer.IDUsed.IDExpireDate.ToShortDateString());
            Debug.Log("Contact Number: " + customer.IDUsed.IDContactNumber);

            // Declare newID variable
            GameObject newID = null;

            // Additional fields based on ID type
            switch (customer.IDUsed.IDTypeNumber)
            {
                // Driver's License
                case 0:
                    Debug.Log("Weight: " + customer.IDUsed.Weight);
                    Debug.Log("Height: " + customer.IDUsed.Height);
                    Debug.Log("Blood Type: " + customer.IDUsed.BloodType);
                    Debug.Log("Restriction: " + customer.IDUsed.Restriction);
                    Debug.Log("Conditions: " + customer.IDUsed.Conditions);

                    // Instantiate the ID
                    newID = Instantiate(driversLicensePrefab, Vector3.zero, Quaternion.identity);
                    newID.transform.SetParent(canvas.transform, false); // Set the parent to the Canvas
                    DriversLicensePrefab driversLicenseScript = newID.GetComponent<DriversLicensePrefab>();
                    driversLicenseScript.SetContent(customer, canvas);
                    break;

                // Senior Citizen ID
                case 1:
                    Debug.Log("Emergency Contact Name: " + customer.IDUsed.EmergencyContactName);
                    Debug.Log("Emergency Contact Number: " + customer.IDUsed.EmergencyContactNumber);

                    // Instantiate the ID
                    newID = Instantiate(seniorIDPrefab, Vector3.zero, Quaternion.identity);
                    newID.transform.SetParent(canvas.transform, false); // Set the parent to the Canvas
                    SeniorIDPrefab seniorIDScript = newID.GetComponent<SeniorIDPrefab>();
                    seniorIDScript.SetContent(customer, canvas);
                    break;

                // National ID
                case 2:
                    Debug.Log("National ID Number: " + customer.IDUsed.IDNumber);

                    // Instantiate the ID
                    newID = Instantiate(nationalIDPrefab, Vector3.zero, Quaternion.identity);
                    newID.transform.SetParent(canvas.transform, false); // Set the parent to the Canvas
                    NationalIDPrefab nationalIDScript = newID.GetComponent<NationalIDPrefab>();
                    nationalIDScript.SetContent(customer, canvas);
                    break;

                // Barangay ID
                case 3:
                    Debug.Log("Barangay: " + customer.IDUsed.Barangay);
                    Debug.Log("Precinct Number: " + customer.IDUsed.PrecinctNumber);

                    // Instantiate the ID
                    newID = Instantiate(barangayIDPrefab, Vector3.zero, Quaternion.identity);
                    newID.transform.SetParent(canvas.transform, false); // Set the parent to the Canvas
                    BarangayIDPrefab barangayIDScript = newID.GetComponent<BarangayIDPrefab>();
                    barangayIDScript.SetContent(customer, canvas);
                    break;

                // Passport
                case 4:
                    Debug.Log("Passport Type: " + customer.IDUsed.PassportType);
                    Debug.Log("Country Code: " + customer.IDUsed.CountryCode);

                    // Instantiate the ID
                    newID = Instantiate(passportPrefab, Vector3.zero, Quaternion.identity);
                    newID.transform.SetParent(canvas.transform, false); // Set the parent to the Canvas
                    PassportPrefab passportScript = newID.GetComponent<PassportPrefab>();
                    passportScript.SetContent(customer, canvas);
                    break;

                // School ID
                case 5:
                case 6:
                    Debug.Log("School Name: " + customer.IDUsed.SchoolName);
                    Debug.Log("School City: " + customer.IDUsed.SchoolCity);
                    Debug.Log("School Grade: " + customer.IDUsed.SchoolGrade);
                    Debug.Log("School Section: " + customer.IDUsed.SchoolSection);
                    Debug.Log("School Year: " + customer.IDUsed.SchoolYear);

                    // Instantiate the ID
                    newID = Instantiate(schoolIDPrefab, Vector3.zero, Quaternion.identity);
                    newID.transform.SetParent(canvas.transform, false); // Set the parent to the Canvas
                    SchoolIDPrefab schoolIDScript = newID.GetComponent<SchoolIDPrefab>();
                    schoolIDScript.SetContent(customer, canvas);
                    break;

                // Bank Slip
                case 7:
                    Debug.Log("Bank Branch: " + customer.IDUsed.BankBranch);
                    Debug.Log("Deposit Number: " + customer.IDUsed.DepositNumber);
                    Debug.Log("Deposit Amount: " + customer.IDUsed.DepositAmount);
                    break;
            }

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

            // Instantiate the Customer prefab as a child of the LoanAppContainer
            canvas = backgroundCanvas;
            GameObject newCustomer = Instantiate(customerPrefab, Vector3.zero, Quaternion.identity);
            newCustomer.transform.SetParent(canvas.transform, false); // Set the parent to the Canvas
            CustomerPrefab customerScript = newCustomer.GetComponent<CustomerPrefab>();
            customerScript.GenerateSprite(customer, canvas);

            // Wait for player input before proceeding
            yield return new WaitUntil(() => buttonClicked);
            Debug.Log("Approval Status: " + customer.Approved);

            // Update approval status
            customer.Approved = currentCustomer.Approved;

            // Adjust amounts accordingly
            if (customer.Approved)
            {
                totalLoanAmount += customer.LoanAmount;
                currentBudget -= customer.LoanAmount;
            }
            totalLoanCount -= 1;

            // Destroy all physical traces of previous customer
            Destroy(newCustomer);
            Destroy(newLoanApp);
            Destroy(newID);

            // Reset buttonClick
            buttonClicked = false;
        }

        // All customers were processed
        customersComplete = true;

        // Generate dialog and script
        var dialog = Instantiate(dialogPrefab);
        var dialogScript = dialog.GetComponent<ConfirmDialogPrefab>();

        // Disable buttons
        dialogScript.DisableButtons();
        dialogScript.SetText(dialogScript.dialogText, "All customers were processed!"); // Set dialog text
        dialogScript.CloseDialog(3.0f); // Give player time before the dialog terminates
    }

    // Approve Loan
    void OnApproved()
    {
        // Check if budget is sufficient
        if (currentBudget < currentCustomer.LoanAmount)
        {
            // Generate dialog and script
            var dialog = Instantiate(dialogPrefab);
            var dialogScript = dialog.GetComponent<ConfirmDialogPrefab>();

            // Disable buttons
            dialogScript.DisableButtons();
            dialogScript.SetText(dialogScript.dialogText, "You do not have enough money for the loan!"); // Set dialog text
            dialogScript.CloseDialog(3.0f); // Give player time before the dialog terminates
        }
        else
        {
            buttonClicked = true;
            currentCustomer.Approved = true;
        }
    }

    // Reject Loan
    void OnRejected()
    {
        buttonClicked = true;
        currentCustomer.Approved = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Update status texts
        currentBudgetText.text = currentBudget.ToString("n2");
        totalLoanAmountText.text = totalLoanAmount.ToString("n2");
        totalLoanCountText.text = totalLoanCount.ToString();

        // Edit buttons if all customers have been processed
        if (customersComplete)
        {
            ContinueButton.gameObject.SetActive(true);
            ApproveButton.gameObject.SetActive(false);
            RejectButton.gameObject.SetActive(false);
        }
    }
}
