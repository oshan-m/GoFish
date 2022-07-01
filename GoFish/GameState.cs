using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GoFish
{
    public class GameState
    {
        public readonly IEnumerable<Player> Players;
        public readonly IEnumerable<Player> Opponents;
        public readonly Player HumanPlayer;
        public bool GameOver { get; private set; } = false;
        public readonly Deck Stock;
        /// <summary>
        /// Constructor creates the players and deals their first hands
        /// </summary>
        /// <param name="humanPlayerName">Name of the human player</param>
        /// <param name="opponentNames">Names of the computer players</param>
        /// <param name="stock">Shuffled stock of cards to deal from</param>
        public GameState(string humanPlayerName, IEnumerable<string> opponentNames, Deck stock)
        {
            this.Stock = stock;

            HumanPlayer = new Player(humanPlayerName);
            HumanPlayer.GetNextHand(stock);
            var opponents = new List<Player>();
            foreach (var name in opponentNames)
            {
                var player = new Player(name);
                player.GetNextHand(stock);
                opponents.Add(player);
            }
            Opponents = opponents;
            Players = new List<Player>() { HumanPlayer }.Concat(Opponents) ;
        }
        /// <summary> 
        /// Gets a random player that doesn't match the current player
        /// </summary>
        /// <param name="currentPlayer">The current player</param>
        /// <returns>A random player that the current player can ask for a card</returns>
        public Player RandomPlayer(Player currentPlayer) => Players.Where(player => player != currentPlayer)
            .Skip(Player.Random.Next( Players.Count() -1))
            .First();
        /// <summary>
        /// Makes one player play a round
        /// </summary>
        /// <param name="player">The player asking for a card</param>
        /// <param name="playerToAsk">The player being asked for a card</param>
        /// <param name="valueToAskFor">The value to ask the player for</param>
        /// <param name="stock">The stock to draw cards from</param>
        /// <returns>A message that describes what just happened</returns>
        public string PlayRound(Player player, Player playerToAsk,
        Values valueToAskFor, Deck stock)
        {

            var msg = $"{player.Name} asked {playerToAsk.Name} for {valueToAskFor}{(valueToAskFor == Values.Six ? "es" : "s")}{Environment.NewLine}";
            var cards = playerToAsk.DoYouHaveAny(valueToAskFor, stock);
            if (cards.Count() > 0)
            {
                msg += $"{playerToAsk.Name} has { cards.Count()} { valueToAskFor} card{ Player.S(cards.Count())}";
                player.AddCardsAndPullOutBooks(cards);
            }
            else if(stock.Count() == 0)
            {
                msg += "The stock is out of cards";
            } 
            else
            {
                player.DrawCard(stock);
                msg += $"{player.Name} drew a card";
            }
            if (player.Hand.Count() == 0)
            {
                player.GetNextHand(stock);
                msg += $"{Environment.NewLine}{player.Name} ran out of cards, drew {player.Hand.Count()} from the stock";
            }
            return msg;
        }
        /// <summary>
        /// Checks for a winner by seeing if any players have any cards left, sets GameOver
        /// if the game is over and there's a winner
        /// </summary>
        /// <returns>A string with the winners, an empty string if there are no winners</returns>
        public string CheckForWinner()
        {

            var playerCards = Players.Select(player => player.Hand.Count()).Sum();
            if (playerCards > 0) return "";
            GameOver = true;

            var winningNumber = Players.Select(player => player.Books.Count()).Max();
            var winners = Players.Where(player => player.Books.Count() == winningNumber);
            if (winners.Count() == 1)
                return $"The winner is {winners.First().Name}";
            return $"The winners are {String.Join(" and ",winners)}";
        }
    }
}
