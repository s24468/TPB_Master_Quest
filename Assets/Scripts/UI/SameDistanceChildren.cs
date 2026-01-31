using UnityEngine;
using UnityEngine.Serialization;

// place first and last elements in children array manually
// others will be placed automatically with equal distances between first and last elements
namespace UI
{
    public class SameDistanceChildren : MonoBehaviour
    {
        public Transform[] children;

        void Awake()
        {
            Vector3 first = children[0].localPosition;
            Vector3 last  = children[^1].localPosition;

            Vector3 step = (last - first) / (children.Length - 1);

            for (int i = 1; i < children.Length; i++)
            {
                children[i].localPosition = children[i - 1].localPosition + step;
            }
        }
    }

}