using DataPersistance;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MoneyManager : MonoBehaviour, IDataPersistence
    {
        int _money = 0;
        [SerializeField] TextMeshProUGUI _moneyText; // UI Text reference

        public void UpdateMoneyText()
        {
            _moneyText.text = "Money: " + _money;
        }

        public void LoadData(GameData data)
        {
            _money = data.money;
            UpdateMoneyText();
        }

        public void SaveData(ref GameData data)
        {
            // data.money = _money;
        }
    }
}