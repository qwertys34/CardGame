using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.scripts
{
    public class Game
    {
        public List<Card> PlayerDeck,
            EnemyDeck;

        public Game()
        {
            PlayerDeck = GiveDeckCard();
            EnemyDeck = GiveDeckCard();
        }

        private List<Card> GiveDeckCard()
        {
            var deck = new List<Card>();
            for (int i = 0; i < 10; i++) 
                deck.Add(CardManager.AllCards[Random.Range(0, CardManager.AllCards.Count)]);

            return deck;
        }

    }
    
    public class GameManagerScr : MonoBehaviour
    {
        public static GameManagerScr G;
        
        [SerializeField] private Button finishTurnBtn;
        [SerializeField] private TextMeshProUGUI timeRemainingText;
        
        public Game CurrentGame;
        public Transform PlayerHandTr, EnemyHandTr,
                         PlayerFieldTr, EnemyFieldTr;
        public GameObject CardPref;
        public List<CardInfoScr> PlayerHandCards,
                                 EnemyHandCards,
                                 PlayerFieldCards,
                                 EnemyFieldCards;
        public bool IsPlayerTurn => Turn % 2 == 0;
        
        private int Turn = 0, TurnTime = 30;
        private WaitForSeconds _pause = new WaitForSeconds(1);
        
        public bool CanHaveMoreCardInEitherPlace(PlaceType placeType)
        {
            switch (placeType)
            {
                case PlaceType.SelfHand:
                    return PlayerHandCards.Count < 5;
                case PlaceType.EnemyHand:
                    return EnemyHandCards.Count < 5;
                case PlaceType.SelfField:
                    return PlayerFieldCards.Count < 6;
                case PlaceType.EnemyField:
                    return EnemyFieldCards.Count < 6;
                default: Debug.LogException(new Exception("no such"));
                    return false;
            }
        }
        
        private void Awake()
        {
            if (G != null && G != this)
            {
                Destroy(gameObject);
                return;
            }

            G = this;
            DontDestroyOnLoad(gameObject);
            
            PlayerHandCards = new();
            EnemyHandCards = new();
            PlayerFieldCards = new();
            EnemyFieldCards = new();
        }

        private void OnDestroy()
        {
            if (G == this) G = null;
        }

        private void Start()
        {
            CurrentGame = new Game();
            
            GiveHandCards(CurrentGame.PlayerDeck, PlayerHandTr);
            //GiveHandCards(CurrentGame.EnemyDeck, EnemyHandTr);
            GiveCardToHand(CurrentGame.EnemyDeck, EnemyHandTr);

            StartCoroutine(TurnFunc());
        }
        
        public void ChangeTurn()
        {
            FightCards();
            StopAllCoroutines();
            Turn++;
            Debug.Log("Turn number: " + Turn);
            finishTurnBtn.interactable = IsPlayerTurn;

            if (IsPlayerTurn) 
                GiveNewCards();
            
            StartCoroutine(TurnFunc());
        }

        private void FightCards()
        {
            if (PlayerFieldCards.Count == 0 || EnemyFieldCards.Count == 0)
                return;

            for (int i = 0; i < PlayerFieldCards.Count; i++)
            {
                var pCard = PlayerFieldCards[i];
                var eCard = EnemyFieldCards[i];
                
                if (!pCard || !eCard) // пока так, в дальнейшем наносить урон противнику, если его не дефает карта
                    return;
                
                eCard.SelfCard.Defence -= pCard.SelfCard.Damage;
                pCard.SelfCard.Defence -= eCard.SelfCard.Damage;
                Debug.Log($"Player {i} card hp: {pCard.SelfCard.Defence}");
                Debug.Log($"Enemy {i} card hp: {eCard.SelfCard.Defence}");
                
                if (eCard.SelfCard.Defence <= 0)
                {
                    EnemyFieldCards.Remove(eCard);
                    Destroy(eCard.gameObject);
                }
                else    
                    eCard.UpdateInfo();

                if (pCard.SelfCard.Defence <= 0)
                {
                    PlayerFieldCards.Remove(pCard);
                    Destroy(pCard.gameObject);
                }
                else 
                    pCard.UpdateInfo();
            }
        }
        
        private IEnumerator TurnFunc()
        {
            TurnTime = 30;
            timeRemainingText.text = TurnTime.ToString();

            if (IsPlayerTurn)
            {
                while (TurnTime-- > 0)
                {
                    yield return _pause;
                    timeRemainingText.text = TurnTime.ToString();
                }
            }
            else
            {
                while (TurnTime-- > 27)
                {
                    yield return _pause;
                    timeRemainingText.text = TurnTime.ToString();
                }

                if (EnemyHandCards.Count > 0)
                    EnemyTurn(EnemyHandCards);
            }
            
            ChangeTurn();
        }

        private void EnemyTurn(List<CardInfoScr> cards)
        {
            var count = Random.Range(1, cards.Count);
            for (int i = 0; i < count; i++)
            {
                if (!CanHaveMoreCardInEitherPlace(PlaceType.EnemyField))
                    return;
                
                cards[0].ShowInfo(cards[0].SelfCard);
                cards[0].transform.SetParent(EnemyFieldTr);

                EnemyFieldCards.Add(cards[0]);
                EnemyHandCards.Remove(cards[0]);
            }
        }

        private void GiveNewCards()
        {
            if (CanHaveMoreCardInEitherPlace(PlaceType.SelfHand))
                GiveCardToHand(CurrentGame.PlayerDeck ,PlayerHandTr);
            if (CanHaveMoreCardInEitherPlace(PlaceType.EnemyHand))
                GiveCardToHand(CurrentGame.EnemyDeck ,EnemyHandTr);
        }
        
        private void GiveHandCards(List<Card> deck, Transform hand)
        {
            var i = 0;
            while (i++ < 4) 
                GiveCardToHand(deck, hand);
        }

        private void GiveCardToHand(List<Card> deck, Transform hand)
        {
            if (deck.Count == 0)
                return;

            Card card = deck[0];

            GameObject cardGO = Instantiate(CardPref, hand, false);
            
            if (hand == EnemyHandTr)
            {
                var cardInfo = cardGO.GetComponent<CardInfoScr>();
                cardInfo.HideInfo(card);
                EnemyHandCards.Add(cardInfo);
            }
            else
            {
                var cardInfo = cardGO.GetComponent<CardInfoScr>();
                cardInfo.ShowInfo(card);
                PlayerHandCards.Add(cardInfo);
            }
            
            deck.RemoveAt(0);
        }
    }
}