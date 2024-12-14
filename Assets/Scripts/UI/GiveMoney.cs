using DataPersistance;
using UnityEngine;

namespace UI
{
    public class GiveMoney : MonoBehaviour, IDataPersistence
    {
        int _money;
        // string nickname;

        public void giveMoney()
        {
            _money++;
            // nickname = "ahhaahahah";
            // Debug.Log(_money + nickname);
            Debug.Log(_money);
        }

        private void Awake()
        {
            _money = 0;
        }

        public void LoadData(GameData data)
        {
            _money = data.money;
            // nickname = data.nickname;
        }

        public void SaveData(ref GameData data)
        {
            data.money = _money;
            // data.nickname = nickname;
        }
    }
}