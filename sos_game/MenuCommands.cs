using System.Text;
namespace IFN645_SOS
{
    /*Here are the commands for the interfaceMenu, they enable the launch of the game t*/
    public interface MenuCommands
    {
        void Execute();
    };

    public class StartMatchCommand : MenuCommands
        {
       
        //here, pass the game to the match, initialisation will be done by new game or load game command
        public Game Game { get; }
        public Match Match { get; }
        GameStrategyManager GameStategyManager { get; set; }

        IGameLogic GameLogic { get; set; }

    
        public StartMatchCommand(Game game, Match match, GameStrategyManager gameStategyManager, IGameLogic gameLogic)
        {
            Game = game;
            Match = match;
            GameStategyManager = gameStategyManager;

            GameLogic = gameLogic;
        }
        public void Execute() {
            Match.SetGame(Game);
            Match.SetStrategies(GameStategyManager);
            Match.DisplayRules();
            Match.game.addGameLogic(GameLogic);
            //Match.initializeMatch();

        } 
    };
    public class LoadSavesCommand : MenuCommands 
    {
        UserInterface UserInterface { get; }
        public LoadSavesCommand(UserInterface userInterface) 
        {
            this.UserInterface = userInterface;
        }
        public void Execute() 
        {
            UserInterface.UpdateSaveMenu();
        }
    }
    public class LoadGameCommand : MenuCommands 
    {
        //launch a specific saved game
        Match Match { get; }

        string data;
        public LoadGameCommand(Match match, string data)
        {    
            this.data = data;
            this.Match = match;
        }
        public void Execute()
        {
         
            Match.LoadGame(data);
        }
      
    }
    public class NewGameCommand : MenuCommands {
        //take a game and
        Match Match { get; }
        public NewGameCommand(Match match) 
        {
            Match = match;
           
        }
        public void Execute() {
            Match.InitializeMatch();
            Match.NewGame();
            Match.StartGame();
          
        }
    }
    public class ExiteGameCommand : MenuCommands {
        public void Execute() {
            Console.WriteLine("Exiting the game, thank you for playing");
            Environment.Exit(0);
        }
    }
    public class SetPlayerGameStrategy : MenuCommands
    {
        Player Player { set; get; }
        IMoveStrategy MoveStrategy { get; set; }

        public SetPlayerGameStrategy(Player player, IMoveStrategy moveStrategy) 
        {
            this.Player = player;
            this.MoveStrategy = moveStrategy;
        }
        public void Execute()
        {
            this.Player.AddStrategy(MoveStrategy);
        }
    }


}
