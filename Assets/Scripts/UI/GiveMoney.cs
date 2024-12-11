using DataPersistance;
using UnityEngine;

namespace UI
{
    public class GiveMoney: MonoBehaviour, IDataPersistence
    {
        int _money;
        public void giveMoney()
        {
            _money++;
            Debug.Log(_money);
        }

        private void Awake()
        {
            _money = 0;
        }

        public void LoadData(GameData data)
        {
            _money = data._money;
        }

        public void SaveData(ref GameData data)
        {
            data._money = _money;
        }
    }
}