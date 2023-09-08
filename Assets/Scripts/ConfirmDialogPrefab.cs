using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ConfirmDialogPrefab : MonoBehaviour
{
    // Confirmation Dialog Objects
    public TMP_Text dialogText;
    public TMP_Text yesText;
    public TMP_Text noText;
    public Button yesButton;
    public Button noButton;
    private UnityAction yesAction;
    private UnityAction noAction;
    private SceneLoader sceneLoaderScript;

    public void EnableButtons()
    {
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }

    public void DisableButtons()
    {
        // Ensure that game is not paused as this does not require player input to continue
        Global.UnpauseGame(true);

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }

    public void SetText(TMP_Text text, string newText)
    {
        text.text = newText;
    }

    public void SetAction(UnityAction newYesAction, UnityAction newNoAction)
    {
        yesAction = newYesAction;
        noAction = newNoAction;

        yesButton.onClick.AddListener(InvokeYes);
        noButton.onClick.AddListener(InvokeNo);

        // Ensure that game is paused as this requires player input to continue
        Global.PauseGame(true);
    }

    private void InvokeYes()
    {
        if (yesAction != null)
        {
            yesAction.Invoke(); // Invoke the action
        }

        // Unpause the game
        Global.UnpauseGame(true);

        // Delete dialog
        Destroy(gameObject);
    }

    private void InvokeNo()
    {
        if (noAction != null)
        {
            noAction.Invoke(); // Invoke the action
        }

        // Unpause the game
        Global.UnpauseGame(true);

        // Delete dialog
        Destroy(gameObject);
    }

    public void CloseDialog(float delay)
    {
        // Automatically close the dialog after the specified delay
        Destroy(gameObject, delay);
    }
}
