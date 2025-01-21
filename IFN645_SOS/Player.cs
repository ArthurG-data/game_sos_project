using System.Text;

namespace IFN645_SOS
{
    /*here the player is a class able to make a move, and the way to make a move can be customized with moveStrategy*/
    public interface IMoveStrategy 
    {
        string type { get;}
        IGameCommand MakeMove(GameState gameState,Board board, Game game);
    }

    public class HumanSosMoveStategy : IMoveStrategy 
    
    {
        public string type { get; } = "Human";
       
       //will be passe to human player
        public IGameCommand MakeMove(GameState gameState,Board board, Game game)
        {
            //take the gamestate, components and the number of cells in the grid
            int cell = 0;
            string pieceSelected;
            string[] avalaibleInput = new string[game.GetAvailableComponents().Length];
            //check input validity for cell
            Console.Write("Select an empty tile, -1 : undo previous move, -2 : save game,  -3 : exit program: ");
            while (!int.TryParse(Console.ReadLine(), out cell) && cell>-3 && cell<board.Row*board.Col) ;

            if (cell == -1)
            {
                //check here
                return new UndoMove(game);
            }
            else if (cell == -2)
            {
                return new SaveMove(game);
            }
            else if (cell == -3) 
            {
                Environment.Exit(0);
            }
            Console.Write("Select a piece by inputing the correpsonding character: ");
            for (int i = 0; i < game.GetAvailableComponents().Length; i++)
               
            {
             
                avalaibleInput[i] = game.GetAvailableComponents()[i].Draw();
                Console.Write(avalaibleInput[i] +" ");
            ;
            }
            do 
            {
                Console.WriteLine("Enter a valid option");
                pieceSelected = Console.ReadLine().ToUpper();
            }
            //check input validity
            while (Array.IndexOf(avalaibleInput, pieceSelected) == -1);

            int row = cell / board.Row;
            int col = cell % board.Col;

            if (game.GetAvailableComponents()[Array.IndexOf(avalaibleInput, pieceSelected)] is Piece piece)
            {
                return new MoveCommand(gameState.GetCurrentPlayer(), row, col, piece, board);
            }
            else 
            { 
                throw new Exception("Command is not a move, not added"); 
            }
            
        }
    }

    public class AiMoveStategyEasy : IMoveStrategy
    {
        public string type { get; } = "AI easy";
        public IGameCommand MakeMove(GameState gameState, Board board, Game game)
        {
            //here this AI movestrategy select a piece and cell at random, an other will check the validity
            //other levels of difficulty can be added
            int cell = 0;
            GameComponent gameComponent;
            Random random = new Random();
            cell = random.Next(0, board.Row * board.Col);
            gameComponent = game.GetAvailableComponents()[random.Next(game.GetAvailableComponents().Length)];
            int row = cell / board.Row;
            int col = cell % board.Col;
            if (gameComponent is Piece piece)
            {
                return new MoveCommand(gameState.GetCurrentPlayer(), row, col, piece, board);
            }
            else
            {
                throw new Exception("Command is not a move, not added");
            }

        }
    }
    public class Player
    {
        public string Name { get; set; }
        public int Id { get; set; }
        private IMoveStrategy moveStrategy = null;
        public Player( string name, int id)
        {
            
            this.Name = name;
            this.Id = id;
        }
        public string type;
        public void AddStrategy(IMoveStrategy moveStrategy) 
        {
            if (this.moveStrategy == null)
            {
                this.moveStrategy = moveStrategy;
                this.type = moveStrategy.type;
            }
            else {
                throw new Exception("player already has a strategy");
            }
        }
        public Player(IMoveStrategy moveStrategy, string name, int id)
        {
            this.moveStrategy = moveStrategy;
            this.Name = name;
            this.Id = id;
        }

        public IGameCommand MakeMove(GameState gameState, Board board, Game game)
        {
            return moveStrategy.MakeMove(gameState, board, game);
        }
        public override string ToString()
        {
            return Name;
        }
    }
    public class GameStrategyManager
        //add a strategy manger to inject into the match, it is then easier to modify the strat availables
    {
        private List<IMoveStrategy> strategies = new List<IMoveStrategy>();

        public void AddStategy(IMoveStrategy strategy)
        {
            strategies.Add(strategy);
        }

        public IEnumerable<IMoveStrategy> GetStrategies()
        {
            return strategies;
        }
    }
}
  

