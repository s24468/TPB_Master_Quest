using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Button actionButton;
    [SerializeField] private TextMeshProUGUI actionButtonText;
    [SerializeField] private CardSpawner cardSpawner; // Reference to the CardSpawner in the scene.

    private bool isBuying = true;

    public void SetMode(bool buyingMode)
    {
        isBuying = buyingMode;

        if (isBuying)
        {
            actionButtonText.text = "Buy Card";
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(() =>
            {
                CardPreviewManager.Instance.HandleCardTransaction(true);
                UpdateShopCardState(); // Refresh the shop UI after a transaction.
            });
        }
        else
        {
            actionButtonText.text = "Sell Card";
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(() =>
            {
                CardPreviewManager.Instance.HandleCardTransaction(false);
                UpdateShopCardState(); // Refresh the shop UI after a transaction.
            });
        }
    }

    public void UpdateShopCardState()
    {
        // Update creature cards
        var creatureDisplays = cardSpawner.creatureCardParent.GetComponentsInChildren<CardDisplay>(true);
        foreach (var display in creatureDisplays)
        {
            display.UpdateNotUnlocked();
        }
        // Update spell cards
        var spellDisplays = cardSpawner.spellCardParent.GetComponentsInChildren<CardDisplay>(true);
        foreach (var display in spellDisplays)
        {
            display.UpdateNotUnlocked();
        }
    }
}