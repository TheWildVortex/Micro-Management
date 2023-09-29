using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class BannerPrefab : MonoBehaviour
{
    // Confirmation Dialog Objects
    public TMP_Text bannerMainText;
    public TMP_Text bannerSubText;

    public void SetText(string newMainText, string newSubText)
    {
        bannerMainText.text = newMainText;
        bannerSubText.text = newSubText;
    }

    public void CloseBanner(float delay)
    {
        // Automatically close the dialog after the specified delay
        Destroy(gameObject, delay);
        Debug.Log("BANNER DESTROYED!");
    }
}
