using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class BannerPrefab : MonoBehaviour
{
    // Confirmation Dialog Objects
    public TMP_Text bannerText;

    public void SetText(string newText)
    {
        bannerText.text = newText;
    }

    public void CloseBanner(float delay)
    {
        // Automatically close the dialog after the specified delay
        Destroy(gameObject, delay);
        Debug.Log("BANNER DESTROYED!");
    }
}
