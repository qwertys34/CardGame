using UnityEngine;

namespace Game.scripts
{
    public struct Card
    {
        public string Name;
        public Sprite Logo;
        public int Damage, Defence;
        public bool isAlive;
        
        public Card(string Name, int Damage, int  Defence, string Path)
        {
            this.Name = Name;
            Logo = Resources.Load<Sprite>(Path);
            this.Damage = Damage;
            this.Defence = Defence;
            isAlive = true;
        }
    }
}