using DataPersistance;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GiveNickname : MonoBehaviour, IDataPersistence
    {
        string nickname;

        [SerializeField] private TextMeshProUGUI nicknameInput;
        [SerializeField] public GameObject buttonPrefab;

        public void SetNickname()
        {
            nickname = nicknameInput.text; // Bez GetComponent
            // nickname = "ssss"; // Bez GetComponent
            Debug.Log("Saved: "+nickname);
            buttonPrefab.SetActive(true);

        }

        private void Awake()
        {
            buttonPrefab.SetActive(false);
            Debug.Log("awakeNickName");
        }

        public void LoadData(GameData data)
        {
            nickname = data.nickname;
        }

        public void SaveData(ref GameData data)
        {
            data.nickname = nickname;
        }
    }
}