using UnityEngine;
using UnityEngine.Serialization;

// place first and last elements in children array manually
// others will be placed automatically with equal distances between first and last elements
namespace UI
{
    public class SameDistanceChildren : MonoBehaviour
    {
        public Transform[] children;

        // Use this for initialization
        void Awake()
        {
            Vector3 firstElementPos = children[0].transform.position;
            Vector3 lastElementPos = children[^1].transform.position;

            // dividing by Children.Length - 1 because for example: between 10 points that are 9 segments
            var xDist = (lastElementPos.x - firstElementPos.x) / (float)(children.Length - 1);
            var yDist = (lastElementPos.y - firstElementPos.y) / (float)(children.Length - 1);
            var zDist = (lastElementPos.z - firstElementPos.z) / (float)(children.Length - 1);

            var dist = new Vector3(xDist, yDist, zDist);

            for (int i = 1; i < children.Length; i++)
            {
                children[i].transform.position = children[i - 1].transform.position + dist;
            }
        }
    }
}