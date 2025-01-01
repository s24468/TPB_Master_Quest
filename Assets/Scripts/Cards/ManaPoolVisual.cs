using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Cards
{
    [ExecuteInEditMode]
    public class ManaPoolVisual : MonoBehaviour {

        [FormerlySerializedAs("TestFullCrystals")] public int testFullCrystals;
        [FormerlySerializedAs("TestTotalCrystalsThisTurn")] public int testTotalCrystalsThisTurn;

        private int _totalCrystals;
        private int _availableCrystals;

        [FormerlySerializedAs("Crystals")] public Image[] crystals;
        [FormerlySerializedAs("ProgressText")] public TextMeshProUGUI progressText;

        public int TotalCrystals
        {
            get => _totalCrystals;

            set
            {
                //Debug.Log("Changed total mana to: " + value);

                if (value > crystals.Length)
                    _totalCrystals = crystals.Length;
                else if (value < 0)
                    _totalCrystals = 0;
                else
                    _totalCrystals = value;

                for (int i = 0; i < crystals.Length; i++)
                {
                    if (i < _totalCrystals)
                    {
                        if (crystals[i].color == Color.clear)
                            crystals[i].color = Color.gray;
                    }
                    else
                        crystals[i].color = Color.clear;
                }

                // update the text
                progressText.text = $"{_availableCrystals.ToString()}/{_totalCrystals.ToString()}";
            }
        }


        public int AvailableCrystals
        {
            get => _availableCrystals;

            set
            {
                //Debug.Log("Changed mana this turn to: " + value);

                if (value > _totalCrystals)
                    _availableCrystals = _totalCrystals;
                else if (value < 0)
                    _availableCrystals = 0;
                else
                    _availableCrystals = value;

                for (int i = 0; i < _totalCrystals; i++)
                {
                    crystals[i].color = i < _availableCrystals ? Color.white : Color.gray;
                }

                // update the text
                progressText.text = $"{_availableCrystals.ToString()}/{_totalCrystals.ToString()}";

            }
        }


        void Update()
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                TotalCrystals = testTotalCrystalsThisTurn;
                AvailableCrystals = testFullCrystals;
            }
        }
	
    }
}
