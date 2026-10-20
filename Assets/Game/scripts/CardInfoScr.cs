using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.scripts
{
    public class CardInfoScr : MonoBehaviour
    {
        public Card SelfCard;
        public Image Logo;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Damage;
        public TextMeshProUGUI Defence;
        
        public void ShowInfo(Card card)
        {
            SelfCard = card;
            
            Logo.preserveAspect = true;
            Logo.sprite = card.Logo;
            Name.text = card.Name;
            Damage.text = Convert.ToString(card.Damage);
            Defence.text = Convert.ToString(card.Defence);
        }

        public void UpdateInfo()
        {
            Damage.text = Convert.ToString(SelfCard.Damage);
            Defence.text = Convert.ToString(SelfCard.Defence);
        }

        public void HideInfo(Card card)
        {
            /*SelfCard = card;
            Logo.sprite = null;
            Name.text = "";
            Damage = null;
            Defence = null;*/
            ShowInfo(card);
        }
        
    }
}