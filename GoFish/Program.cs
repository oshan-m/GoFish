using System;
using System.Linq;

namespace GoFish
{
    class Program
    {
        /// <summary>
        /// Play a game of Go Fish!
        /// </summary>
        static void Main(string[] args)
        {
            //throw new NotImplementedException();

            Console.Write("Enter you name: ");
            string playerName = Console.ReadLine(); 

            Console.Write("Enter the  number of computer opponents: ");
            int numberOfOpponents;
            while(! int.TryParse(Console.ReadKey().KeyChar.ToString(), out numberOfOpponents) || numberOfOpponents > 4 || numberOfOpponents < 1)
                Console.WriteLine($"{Environment.NewLine}Please enter a number from 1 to 4");

            Console.WriteLine($"{Environment.NewLine}Welcome to the game, {playerName}");

            gameController = new GameController(playerName, Enumerable.Range(1,numberOfOpponents).Select( x => $"Computer #{x}"));
            Console.WriteLine(gameController.Status);

            while (!gameController.GameOver)
            {
                while (!gameController.GameOver)
                {
                    Console.WriteLine("Your hand:");
                    Console.WriteLine(string.Join($"{Environment.NewLine}", gameController.HumanPlayer.Hand
                        .OrderBy(card => card.Suit)
                        .OrderBy(card => card.Value)));

                    var value = PromptForAValue();

                    var player = PromptForAnOpponent();

                    gameController.NextRound(player, value);
                    Console.WriteLine(Environment.NewLine + gameController.Status);
                }

                Console.WriteLine("Press N for a new game, any other key to quit.");
                if (Console.ReadKey(true).KeyChar.ToString().ToUpper() == "N")
                    gameController.NewGame();
                else
                    break;
            }

        }
        /// <summary>
        /// The GameController to manage the game
        /// </summary>
        static GameController gameController;
        /// <summary>
        /// Prompt the human player for a card value
        /// in their hand
        /// </summary>
        /// <returns>The value to ask for</returns>
        static Values PromptForAValue()
        {
            var handValues = gameController.HumanPlayer.Hand.Select(card => card.Value).ToList();
            Console.Write("What card value to you want to ask for? ");
            while (true)
            {
                if(Enum.TryParse(typeof(Values),Console.ReadLine(),out var value) && handValues.Contains((Values)value))
                    return (Values)value;
                else
                    Console.WriteLine("Please enter a value in your hand.");
            }

        }
        /// <summary>
        /// Prompt the human player for an opponent
        /// to ask for a card
        /// </summary>
        /// <returns>The opponent to ask</returns>
        static Player PromptForAnOpponent()
        {
            var opponents = gameController.Opponents.ToList();
            for (int i = 1; i <= opponents.Count(); i++)
                Console.WriteLine($"{i}. {opponents[i-1]}");
            Console.Write("Who do you want to ask for a card? ");
            int selection;
            while (!int.TryParse(Console.ReadKey().KeyChar.ToString(),out selection) || selection > opponents.Count() || selection <= 0)
            {
                Console.WriteLine($"Please enter a number from 1 to {opponents.Count()}:");
            }
            return opponents[selection-1];
        }
    }
}
