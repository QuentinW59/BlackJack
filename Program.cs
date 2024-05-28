using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creation du projet 
            var game = new BlackJackGame();
            game.Play();
        }
    }

    class BlackJackGame // ICI on créer deux list , une pour le joueur et l'autre pour le dealer et on fait un nouveau tableau
    {
        private Deck deck;
        private List<Card> playerHand;
        private List<Card> dealerHand;

        public BlackJackGame()
        {
            deck = new Deck();
            playerHand = new List<Card>();
            dealerHand = new List<Card>();
        }

        public void Play() /* Ici on initialise les règles  et on fait deux méthodes , une pour le mélange et une pour la distribution du jeu et les instructions une fois ls premières cartes reçu*/
        {
            Console.WriteLine("Bienvenue au Black Jack!");

            deck.Shuffle();
            InitialDeal();

            PlayerTurn();
            if (CalculateHandValue(playerHand) <= 21)
            {
                DealerTurn();
            }

            DetermineWinner();
        }

        private void InitialDeal()
        {
            playerHand.Add(deck.DrawCard());
            playerHand.Add(deck.DrawCard());
            dealerHand.Add(deck.DrawCard());
            dealerHand.Add(deck.DrawCard());

            Console.WriteLine("Votre main: " + string.Join(", ", playerHand));
            Console.WriteLine("Main du croupier: " + dealerHand[0] + ", [Carte cachée]");
        }

        private void PlayerTurn() /*Initialiser ce qu'on veut faire en écrivant la mainn qu'on à reçu et savoir 
        si on veut tirer avec input T ou rester avec input R*/        {
            while (true)
            {
                Console.WriteLine("Votre main: " + string.Join(", ", playerHand));
                Console.WriteLine("Valeur de votre main: " + CalculateHandValue(playerHand));

                Console.WriteLine("Voulez-vous (T)irer une carte ou (R)ester?");
                string input = Console.ReadLine().ToUpper();
                if (input == "T")
                {
                    playerHand.Add(deck.DrawCard());
                    if (CalculateHandValue(playerHand) > 21)
                    {
                        Console.WriteLine("Vous avez dépassé 21! Vous avez perdu.");
                        return;
                    }
                }
                else if (input == "R")
                {
                    break;
                }
            }
        }

        private void DealerTurn() // initialisation de la main du croupier et faire en sorte que à 17 il ne tire plus de carte 
        {
            Console.WriteLine("Main du croupier: " + string.Join(", ", dealerHand));
            while (CalculateHandValue(dealerHand) < 17)
            {
                dealerHand.Add(deck.DrawCard());
                Console.WriteLine("Main du croupier: " + string.Join(", ", dealerHand));
            }
        }

        private void DetermineWinner() /*On va calculer les deux mains des joueurs et faire une boucle pour déterminer les différents scénarios possible */
        {
            int playerValue = CalculateHandValue(playerHand);
            int dealerValue = CalculateHandValue(dealerHand);

            Console.WriteLine("Valeur de votre main: " + playerValue);
            Console.WriteLine("Valeur de la main du croupier: " + dealerValue);

            if (playerValue > 21)
            {
                Console.WriteLine("Vous avez perdu!");
            }
            else if (dealerValue > 21 || playerValue > dealerValue)
            {
                Console.WriteLine("Vous avez gagné!");
            }
            else if (playerValue < dealerValue)
            {
                Console.WriteLine("Vous avez perdu!");
            }
            else
            {
                Console.WriteLine("Égalité!");
            }
        }

        private int CalculateHandValue(List<Card> hand)
        {
            int value = hand.Sum(card => card.Value);
            int aceCount = hand.Count(card => card.Rank == Rank.As);

            while (value > 21 && aceCount > 0)
            {
                value -= 10;
                aceCount--;
            }

            return value;
        }
    }

    class Deck /* Ici on créer une list pour les cartes et avoir des cartes au hasard avec un nouveau tableau */
    {
        private List<Card> cards;
        private Random random = new Random();

        public Deck()
        {
            cards = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card(rank, suit));
                }
            }
        }

        public void Shuffle()
        {
            cards = cards.OrderBy(c => random.Next()).ToList();
        }

        public Card DrawCard()
        {
            if (cards.Count == 0)
            {
                throw new InvalidOperationException("Le deck est vide.");
            }

            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }

    class Card
    {
        public Rank Rank { get; private set; }
        public Suit Suit { get; private set; }

        public Card(Rank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public int Value
        {
            get
            {
                if (Rank >= Rank.Deux && Rank <= Rank.Dix)
                {
                    return (int)Rank;
                }
                else if (Rank >= Rank.Valet && Rank <= Rank.Roi)
                {
                    return 10;
                }
                else // Ace
                {
                    return 11;
                }
            }
        }

        public override string ToString()
        {
            return $"{Rank} de {Suit}";
        }
    }

    enum Rank // Valeurs des cartes
    {
        As = 1 ,
        Deux,
        Trois,
        Quatre,
        Cinq,
        Six,
        Sept,
        Huit,
        Neuf,
        Dix,
        Valet,
        Dame,
        Roi
    }
// Enseigne de la carte 
    enum Suit
    {
        Coeurs,
        Carreaux,
        Trèfles,
        Piques
    }
}