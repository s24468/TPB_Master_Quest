using System.Collections.Generic;

namespace Cards.ExtenstionMethods
{
    public class ShufflingExtention
    {
        private static System.Random random = new System.Random();

        public static void Shuffle<T>(IList<T> list)
        {
            var n = list.Count;
            while (n>1)
            {
                n--;
                int k = random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}