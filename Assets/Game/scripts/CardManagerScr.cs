using UnityEngine;

namespace Game.scripts
{
    public class CardManagerScr : MonoBehaviour
    {
        private void Awake() 
        {
            if (CardManager.AllCards == null)
                CardManager.AllCards = new();
            
            CardManager.AllCards.Add(new Card("zero", 1, 2, 
                "new/card_sprites/without_stats/Monster 0 card_1"));
            CardManager.AllCards.Add(new Card("one", 2, 2, 
                "new/card_sprites/without_stats/Monster 1 card_1"));
            CardManager.AllCards.Add(new Card("two", 3, 1, 
                "new/card_sprites/without_stats/Monster 2 card_1"));
            CardManager.AllCards.Add(new Card("three", 4, 6, 
                "new/card_sprites/without_stats/Monster 3 card_1"));
            CardManager.AllCards.Add(new Card("four", 3, 3, 
                "new/card_sprites/without_stats/Monster 4 card_1"));
            CardManager.AllCards.Add(new Card("five", 1, 1, 
                "new/card_sprites/without_stats/Monster 5 card_1"));
            CardManager.AllCards.Add(new Card("six", 6, 2, 
                "new/card_sprites/without_stats/Monster 6 card_1"));
            CardManager.AllCards.Add(new Card("seven", 5, 2, 
                "new/card_sprites/without_stats/Monster 7 card_1"));
            CardManager.AllCards.Add(new Card("eight", 6, 6, 
                "new/card_sprites/without_stats/Monster 8 card_1"));
            
        }
    }
}