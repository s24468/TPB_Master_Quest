using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Button actionButton;
    [SerializeField] private TextMeshProUGUI actionButtonText;

    private bool isBuying = true;
    public void SetMode(bool buyingMode)
    {
        isBuying = buyingMode;

        if (isBuying)
        {
            actionButtonText.text = "Buy Card";
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(() => CardPreviewManager.Instance.HandleCardTransaction(true));
        }
        else
        {
            actionButtonText.text = "Sell Card";
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(() => CardPreviewManager.Instance.HandleCardTransaction(false));
        }
    }
 
}