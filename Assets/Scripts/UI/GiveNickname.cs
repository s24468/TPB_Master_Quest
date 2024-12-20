using DataPersistance;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GiveNickname : MonoBehaviour, IDataPersistence
    {
        string _nickname;

        [SerializeField] private TextMeshProUGUI nicknameInput;

        public void SetNickname()
        {
            _nickname = nicknameInput.text; // Bez GetComponent
            Debug.Log("Saved: " + _nickname);
        }

        private void Awake()
        {
            // Debug.Log("awakeNickName");
        }

        public void LoadData(GameData data)
        {
            _nickname = data.nickname;
        }

        public void SaveData(ref GameData data)
        {
            data.nickname = _nickname;
        }
    }
}