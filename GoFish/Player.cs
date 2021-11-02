using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish
{
    public class Player
    {
        public static Random Random = new Random();

        private List<Card> hand = new List<Card>();
        private List<Values> books = new List<Values>();

        /// <summary> 
        /// The Cards in the player's hand
        /// </summary>
        public IEnumerable<Card> Hand => hand;

        /// <summary>
        ///  The books that the player has pulled out
        /// </summary>
        public IEnumerable<Values> Books => books;

        public readonly string Name;

        /// <summary>
        /// Pluralize a word, adding "s" if a value isn't equal to 1
        /// </summary>
        /// <param name="s">value count</param>
        /// <returns></returns>
        public static string S(int s) => s == 1 ? "" : "s";

        /// <summary>
        /// Returns the current status of the player: the number cards and books
        /// </summary>
        public string Status => $"{Name} has {hand.Count} card{S(hand.Count)} and {books.Count} book{S(books.Count)}";

        /// <summary>
        /// Constructor to create player
        /// </summary>
        /// <param name="name">Player's name</param>
        public Player(string name) => Name = name;

        /// <summary>
        /// Alternate contsructor (used for unit testing)
        /// </summary>
        /// <param name="name">Player's name</param>
        /// <param name="cards">Initial set of cards</param>
        public Player(string name, IEnumerable<Card> cards)
        {
            Name = name;
            hand.AddRange(cards);
        }

        /// <summary>
        /// Gets up to five cards from the stock
        /// </summary>
        /// <param name="stock">Stock to get the next hand from</param>
        public void GetNextHand(Deck stock)
        {
            while ((stock.Count() > 0) && (hand.Count < 5))
            {
                hand.Add(stock.Deal(0));
            }
        }

        /// <summary>
        /// If I have any cards that match the value, return them. 
        /// If I run out of card, get the next hand form the deck.
        /// </summary>
        /// <param name="value">Value I'm asked for</param>
        /// <param name="card">Deck to draw my next hand from</param>
        /// <returns>The cards that were pulled out of the other player's hand</returns>
        public IEnumerable<Card> DoYouHaveAny(Values value, Deck deck)
        {
            var matchingCards = hand.Where(card => card.Value == value)
                .OrderBy(card => card);

            hand = hand.Where(card => card.Value != value).ToList();

            if(hand.Count() == 0)
                GetNextHand(deck);

            return matchingCards;
        }

        public void AddCardsAndPullOutBooks(IEnumerable<Card> cards)
        {
            throw new NotImplementedException();
        }

        public void DrawCard(Deck stock)
        {
            throw new NotImplementedException();
        }

        public Values RandomValuesFromHand() => throw new NotImplementedException();

        public override string ToString() => Name;
    }
}
