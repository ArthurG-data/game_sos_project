using System.Text;

namespace IFN645_SOS
{
   /*Here we have the match class responsible to create the players, set up the game, decide what to do when the game is over and load a game
    */
    public class Match
    {
       
        public Game game=null;
        GameStrategyManager GameStategyManager { get; set; } = null;
        //in our case the strategies for both game would be the same
        Menu strategiesMenu = new Menu("PlayerStrategies");
        LoadManager loadManager = new LoadManager();
        public void DisplayRules()
        //here the textFormatter should be injected. In our case it was a proof of concept more than an important feature
        {
            TextFormatter centerText =  new TextFormatter();
            centerText.DisplayCenteredText(this.game.GetRules());
          }
        public void SetStrategies(GameStrategyManager gameStategyManager) 
            //use to add the desire gamestrategy manager
        {
            if (this.GameStategyManager == null)
            {
                this.GameStategyManager = gameStategyManager;
                
            }
            else
            {
                throw new Exception("Match already has strategies");
            }

        }
        public void SetGame(Game game) 
        {
            //use to add the game selected, could any game supported.also add the loadManger to the game, so the game could save a futur game
            if (this.game == null)
            {
                this.game = game;
                this.game.addLoadManger(loadManager);
            }
            else 
            {
                throw new Exception("Match already has a game");
            }
        }
        public Player CreatePlayer(string name, int ID)
        {
            //here only create the player, need to know if AI or human
            Player newPlayer = new Player(name, ID);
            strategiesMenu = new Menu("PlayerStrategies");
            foreach (IMoveStrategy moveStrategy in GameStategyManager.GetStrategies()) 
            {
                SetPlayerGameStrategy setPlayerGameStrategy = new SetPlayerGameStrategy(newPlayer, moveStrategy);
                strategiesMenu.Add(moveStrategy.type, setPlayerGameStrategy);
            }
            //the menu is used for the selection of the player type: here is AI easy or human
            strategiesMenu.Display();
            return newPlayer;
        }

        public void StartGame() 
        {
         
            this.game.StartGame();
            EndGame();
        }

        public void EndGame()
        {
            //display the winner. The class is responsible to know the win conditions.could also be a separate module
            Console.Clear();
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;
            string displayText = ($"Congratulation {GetWinner()}, you won!!");
            int centerX = (consoleWidth-displayText.Length) / 2;
            int centerY = consoleHeight / 2;
            
            Console.SetCursorPosition(centerX, centerY-5);
            Console.WriteLine(displayText);
            Console.SetCursorPosition(centerX, centerY);
            this.game.DisplaycenteredBoard();
            //the application terminates here
            Environment.Exit(0);


        }
        public void LoadGame(string savedInfo)
        //introduce the command from the main interface, start a new game with same player and size
        {   
            loadManager.addGame(game);
            loadManager.LoadGame(savedInfo);
            //start the game
            game.StartGame();
            this.EndGame() ;
            //propose new game or back to main menu
        }
        public void SaveGame() 
        {
            try 
            {
                loadManager.addGame(game);
                loadManager.SaveGame();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
       
        
            
        //change to startGame in the futur.The game is suppose to be ready by this point
        public void NewGame()
        {
            Player newPlayer;
            string name;
            Console.WriteLine();
            //check everything is tip top
            if (!game.IsReady())
            {
                for (int i = 0; i < game.GetRequiredPlayers(); i++)
                {
                    int consoleWidth = Console.WindowWidth;
                    int consoleHeight = Console.WindowHeight;
                    int centerX = consoleWidth / 2;
                    int centerY = consoleHeight / 2;

                    Console.Clear();

                    Console.SetCursorPosition(centerX - ($"Enter a player name for player {i + 1}: ".Length / 2), centerY - game.GetRequiredPlayers() / 2 + i);

                    do
                    {
                        Console.Clear();
                        Console.SetCursorPosition(centerX - ($"Enter a player name for player {i + 1}: ".Length / 2), centerY - game.GetRequiredPlayers() / 2 + i);
                        Console.Write($"Enter a player name for player {i + 1}: ");
                        name = Console.ReadLine();
                    } while (name.Length == 0 || name.Length > 10);


                    newPlayer = CreatePlayer(name, i);

                    game.AddPlayer(newPlayer, i);
                }
            }
            else 
            {
                throw new Exception("Game is not ready mate!");
            }
            
        }
        public Player GetWinner() {

            return this.game.GetWinner();
        }
        public void InitializeMatch()
        {
            this.game.InitialyzeGame();

        }
    }
    
}
