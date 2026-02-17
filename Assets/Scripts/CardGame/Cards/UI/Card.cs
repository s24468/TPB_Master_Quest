// using UnityEngine;
//
// namespace UI
// {
//     public class Card
//     {
//         public int ID;
//         public string Name;
//         public string Type;
//         public int Mana;
//         public int TPower;
//         public int PPower;
//         public int BPower;
//         public int CasualPower;
//         public string Description;
//         public int Cost; 
//         public string Abilities; 
//         public Sprite CardSprite;
//     }
// }
using UnityEngine;

namespace UI
{
    public class Card
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Mana { get; set; }
        public int TPower { get; set; }
        public int PPower { get; set; }
        public int BPower { get; set; }
        public int CasualPower { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public string Abilities { get; set; }

        // to zostaw jako pole (CsvHelper i tak tego nie wczyta z CSV)
        public Sprite CardSprite;
    }
}
