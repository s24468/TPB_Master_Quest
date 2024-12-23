using DataPersistance;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ObjectsSettingManager : MonoBehaviour
    {
        [SerializeField] public GameObject GameObject;
        [SerializeField] public bool startActiveness = false;

        
        public void setGameObjectActiveness()
        {
            if (startActiveness)
            {
                GameObject.SetActive(false);
                startActiveness = false;
            }
            else
            {
                GameObject.SetActive(true);
                startActiveness = true;
            }
        }

    }
}