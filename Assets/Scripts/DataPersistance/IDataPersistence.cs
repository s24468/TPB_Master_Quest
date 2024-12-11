using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DataPersistance
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);
        void SaveData(ref GameData data);
    }
}