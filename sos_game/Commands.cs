using System.Text;
namespace IFN645_SOS
{
    /*Here we implement the command design pattern. Those are commands created by player to interact with a game.
     * A commandManger class holds a list of commands and executes all commands. each turn, the board is wiped, score set to 0, and all move are player.
     * Doing this, we don't have to store the previous state of the board to check if the score has changed. The score is calculated in the movecommandManager
     * to check 
     */
    public interface IGameCommand {
        void Execute();
        
    }

    public interface IMoveCommand : IGameCommand

    {
        Player Player { get; }
        Player GetPlayer();
        int Row { get; }
        int Col { get; }

    }
//fin
    public class UndoMove : IGameCommand {
        //remove the last move in the list
        private Game Game { get; set; }

        public UndoMove(Game game) {
            Game = game;
        }

        public void Execute() {

            Game.UndoMove();
        }
    }
    public class SaveMove : IGameCommand
    {
        private Game Game { get; set; }

        public SaveMove(Game game)
        {
            Game = game;
        }
        public void Execute()
        {
            Game.SaveGame();
        }
    }

    public class MoveCommand : IMoveCommand
    {
    //adds a component to the board.The validity of the input is checked before creation, but the validity of the move is checked before execution
        public Player Player { get; }
        public int Row { get; }
        public int Col { get; }
        public Piece Piece { get; }
        public Board Board { get; }
        public void Execute()
        {
           
            Board.AddGameComponent(this.Row, this.Col, this.Piece);
            
        }
        public MoveCommand(Player player, int row, int col, Piece piece, Board board)
        {
            this.Player = player;
            this.Row = row;
            this.Col = col;
            this.Piece = piece;
            this.Board = board;
        }
        public Player GetPlayer()
        {
            return Player;
        }
        public GameComponent GetGameComponent()
        {
            return this.Piece;
        }
        public int GetRow()
        {
            return this.Row;
        }
        public int GetCol()
        {
            return this.Col;
        }
    }

  
    public interface ICommandManager {
        public void ExecuteCommands();
        public void AddCommand(IGameCommand command);
        public void RemoveCommand();
    }
    public class CustomRemoveCommandException : Exception
    {
        public CustomRemoveCommandException(string message) : base(message)
        {
        }
    }
   
    public class MoveCommandManager : ICommandManager
    {
        //the task of this module is to deal with the moves of the game. It calculates if a move creates points and add the points ti the gamestate
        public GameState gameState;
        GameScorer scorer;
        Board board;
        //a list make it easy to store an unknown number of moves. Scales with the size of the board
        private List<IMoveCommand> commands= new List<IMoveCommand>();
        public MoveCommandManager(GameScorer scorer, GameState gameState, Board gameBoard) {
            this.scorer = scorer;
            this.gameState = gameState;
            this.board=gameBoard;
           
        }
        public void AddCommand(IGameCommand command) {
            if (command is IMoveCommand moveCommand) 
            {
                commands.Add(moveCommand);
            }
            else { throw new Exception("Command is not a move command"); }
       
        }
        
        public void RemoveCommand() {  
            //here check if there is a move to remove. a exception is thrown if empty, then catched in the game to prevent the changing of player in the loop
            if (commands.Count > 0) { 
                commands.RemoveAt(commands.Count - 1); 
            } else 
            { throw new CustomRemoveCommandException("No more move to remove"); }

          
        
        }
        public List<IMoveCommand> GetCommands()
        {
            //use to save the game by the gameLoader
            return commands;
        }
        public void ExecuteCommands() {
            this.board.InitializeBoard();
            this.gameState.InitializeScore();
            //here could be change to command to process different commands, here we implemented only move
            foreach(IMoveCommand command in commands)
                {
                    command.Execute();
                    int score = scorer.CalculateScore(command);    
                    Player player = command.GetPlayer();
                    gameState.AddPoint(player, score);
                    
    
               ;
            }    
        }
        
    }
}
