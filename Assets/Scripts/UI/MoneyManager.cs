using DataPersistance;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MoneyManager : MonoBehaviour, IDataPersistence
    {
        int _money = 0;
        [SerializeField] TextMeshProUGUI _moneyText; // UI Text reference

        public void giveMoney()
        {
            _money++;
            Debug.Log(_money);
        }


        private void Awake()
        {
            if (_moneyText == null)
            {
                Debug.Log("MoneyManager: TextMeshProUGUI reference is not set!");
            }
            else
            {
                UpdateMoneyText();
            }
        }

        // Updates the UI Text with the current money value
        public void UpdateMoneyText()
        {
            _moneyText.text = "Money: " + _money;
            Debug.Log(_money);
        }

        public void LoadData(GameData data)
        {
            _money = data.money;
            if (_moneyText == null)
            {
                Debug.Log("MoneyManager: TextMeshProUGUI reference is not set!");
            }
            else
            {
                UpdateMoneyText();
            }
        }

        public void SaveData(ref GameData data)
        {
            data.money = _money;
        }
    }
}