using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Scene for SceneLoader
    public string sceneName;

    // Dialog box
    public GameObject dialogPrefab;

    // Parent button
    public Button MainButton;

    // Awake Function
    private void Awake()
    {
        // Get the Button component attached to the same GameObject
        var mainButton = GetComponent<Button>();

        // Set MainButton reference
        MainButton = mainButton;
    }

    // Update Function
    private void Update()
    {
        if (Global.IsGamePaused())
        {
            DisableButton();
        }
        else
        {
            EnableButton();
        }
    }

    // General Scene Loader Function
    public void LoadScene()
    {
        // Clear Global data
        Global.ResetGlobalData();
        SceneManager.LoadScene(sceneName);
    }

    // Start Game Function
    public void StartGame()
    {
        // Collect player input for Name
        string mfi = "Rojumicro Lending Corporation";

        // Generate New Player
        var player = Global.NewPlayer(mfi);

        // Set variables:
        int number = player.NumberOfCustomers;
        float minLoan = player.MinLoan;
        float maxLoan = player.MaxLoan;
        float chanceForReal = player.RealChance;
        float chanceForValid = player.ValidChance;

        // Generate New Customers
        var customers = Global.GenerateCustomers(number, minLoan, maxLoan, chanceForReal, chanceForValid);

        // Clear Global data
        Global.ResetGlobalData();
        // Store data in Global
        Global.PlayerData = player;
        Global.CustomersData = customers;

        // Load Stage1
        SceneManager.LoadScene(sceneName);
    }

    // Continue Game Function
    public void ContinueGame(Global.Player player, bool failed)
    {
        // Declare variables
        int number;
        float minLoan;
        float maxLoan;
        float chanceForReal;
        float chanceForValid;

        // If the player failed the level
        if (failed) 
        {
            // Set same variables as current level:
            number = player.NumberOfCustomers;
            minLoan = player.MinLoan;
            maxLoan = player.MaxLoan;
            chanceForReal = player.RealChance;
            chanceForValid = player.ValidChance;
        }

        // If the player completed the level
        else
        {
            // Set variables for next level:
            number = player.NumberOfCustomers + (Global.baseCustomerNumber / 2);
            minLoan = player.MinLoan *= 2;
            maxLoan = player.MaxLoan = player.Budget;
            chanceForReal = player.RealChance - 0.1f;
            chanceForValid = player.ValidChance - 0.1f;
        }

        // Generate New Customers
        var customers = Global.GenerateCustomers(number, minLoan, maxLoan, chanceForReal, chanceForValid);

        // Clear Global data
        Global.ResetGlobalData();
        // Store data in Global
        Global.PlayerData = player;
        Global.CustomersData = customers;

        // Load Stage1
        SceneManager.LoadScene(sceneName);
    }

    // Stage1 -> Stage2 Function
    public void Stage1_2()
    {
        // Retrieve the total loan amount and current budget
        var totalLoan = Stage1.totalLoanAmount;
        var budget = Stage1.currentBudget;

        // If no loans are selected...
        if (totalLoan == 0)
        {
            // Generate dialog and script
            var dialog = Instantiate(dialogPrefab);
            var dialogScript = dialog.GetComponent<ConfirmDialogPrefab>();

            // Disable buttons
            dialogScript.DisableButtons();
            dialogScript.SetText(dialogScript.dialogText, "No applications were selected. Please select at least one."); // Set dialog text
            dialogScript.CloseDialog(3.0f); // Give player 5 seconds before the dialog terminates

            return;
        }

        // If player is overbudget...
        if (totalLoan > budget)
        {
            // Generate dialog and script
            var dialog = Instantiate(dialogPrefab);
            var dialogScript = dialog.GetComponent<ConfirmDialogPrefab>();

            // Set dialog text
            dialogScript.SetText(dialogScript.dialogText, "You are currently overbudget. Are you sure you want to continue?");
            dialogScript.SetAction(OverwriteGlobalData, ReturnFunction);
        }

        else
        {
            OverwriteGlobalData();
        }
    }

    // Stage2 -> Stage3 Function
    public void Stage2_3()
    {
        OverwriteGlobalData();
    }

    // Stage3 -> Stage4 Function
    public void Stage3_4()
    {
        OverwriteGlobalData();
    }

    // Exit Game Function
    public void ExitGame()
    {
        Application.Quit();
    }

    // General Return Function
    public void ReturnFunction()
    {
        Debug.Log("RETURN FUNCTION!");
        return;
    }

    // Overwrite Global Data Function
    public void OverwriteGlobalData()
    {
        Debug.Log("OVERWRITE FUNCTION!");

        // Retrieve final player and customer data
        var player = Global.PlayerData;
        var customers = Global.CustomersData;

        // Remove loans with approved = false
        customers.RemoveAll(customer => !customer.Approved);

        // Clear Global data
        Global.ResetGlobalData();
        // Store current data in Global
        Global.PlayerData = player;
        Global.CustomersData = customers;

        // Load New Scene
        SceneManager.LoadScene(sceneName);
    }

    // Enable button function
    private void EnableButton()
    {
        MainButton.interactable = true;
    }

    // Disable button function
    private void DisableButton()
    {
        MainButton.interactable = false;
    }
}
