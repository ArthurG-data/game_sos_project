using System.Text;

namespace IFN645_SOS 
{ 

    public abstract class Game
    {
        
        public string Name { get; set; }
        //use for undo loooooooooop
        public bool IsBackWard { get; set; } =false;
        public int RequiredPlayers { get; set; }
        //mayber not needed
        public Player[] Players { get; set; }
        //maybe implement elsewhere, to be able to create games without gameboard
        public Board gameBoard { get; set; }
        public GameComponent[] GameComponents { get; set;}
        public GameState gameState { get; set; }
        public LoadManager loadManager { get; set; }
        public GameScorer GameScorer { get;set; }
        //rnew implementation
        public string GameRules { get; set; }
        public IGameLogic gameLogic = null;

        public MoveCommandManager commandManager;
        public abstract Board GetBoard();
        public bool PassTurn { get; set; } = true;
        public abstract void AddBoard(int row, int col);
        public abstract void InitialyzeGame();
        public abstract void StartGame();
        public abstract void NewGameState();
        public void LoadGameState() { }  
        public abstract Player GetWinner();
        public abstract bool IsOver();
        //player related
        public abstract void SelectFirstPLayer(Player player = null);
        public abstract int GetRequiredPlayers();
        public abstract void AddPlayer(Player player, int index);

        public abstract void AddCommands(List<IMoveCommand> commands);
        //game related
        public abstract void  UndoMove();
        public abstract bool IsReady();
        public abstract void SaveGame();
        
        public abstract void DisplaycenteredBoard();
        public abstract GameComponent[] GetAvailableComponents();
        //new implementation
        public abstract string GetRules();
        public abstract void addGameLogic(IGameLogic gameLogic);
        public abstract void addLoadManger(LoadManager loadManager);

        public int AskInteger(string output, int min=3, int max=15) {
            int result;

            Console.Write($"Enter a  value for {output} between {min} and {max}: ");
            //arbitr
            while (!int.TryParse(Console.ReadLine(), out result)|| (result <min || result >max));
            return result;
        }

    }


    //implement the SOsgame
    public class ConcreteSOS : Game {
        public ConcreteSOS() 
        {
            //maybe have a game logic that you can pass here
            this.RequiredPlayers = 2;
            this.GameComponents = new Piece[2];
            GameComponents[0] = new SosPiece("S");
            GameComponents[1] = new SosPiece("O");
            this.Name = "Sos";
            this.GameScorer = new SosGameScorer();
            this.GameRules = "You are about to play a game of SOS. You will be able to select the size of the board and choose to play against a friend or the AI. During your turn, you will be able to add a new piece, either an S or an O. You then gain points if your piece creates an SOS word. You will be able to undo any number of moves and save the game at any point to continue. If you score a point, you can keep playing. The game stops when no more moves can be played. The winner is the player with the most points.When players are tie, the winner is the last player to play.";
        }
        public override string GetRules()
        {
           
            return this.GameRules;   
        }
        public override void addLoadManger(LoadManager loadManager)
        {
            this.loadManager = loadManager;
        }
        public override void addGameLogic(IGameLogic gameLogic)
        {
            if (this.gameLogic == null)
            {
                this.gameLogic = gameLogic;
            }
            else 
            {
                throw new Exception("Game already has a gameLogic");
            }
        }
        public override void AddCommands(List<IMoveCommand> commands) {
        //when the game is loaded, adds the move to the commandManager
            foreach (IMoveCommand command in commands) {
                this.commandManager.AddCommand(command);
            }
        }
        public override GameComponent[] GetAvailableComponents()
        {
            return this.GameComponents;
        }
        public override void UndoMove()
        {
        //used by the gameLoop, trigger the call of previous player in the loop
            this.IsBackWard = true;
            try { commandManager.RemoveCommand();
                this.PassTurn = true;
            } 
            catch ( CustomRemoveCommandException ex)
            { 
            //if the list of moves is empty, do not pass the turn and keep asking for a move
                this.PassTurn = false;
            }    
        }

        public override bool IsReady()
        {
            foreach (Player player in Players) 
            {
                if (player == null) 
                {
                    return false;
                }
            }
           // if (Scores == null || Scores.Length == 0)
             //   return false;

            if (gameBoard == null)
                return false;

            if (GameComponents == null || GameComponents.Length == 0)
                return false;

            if (gameState == null)
                return false;

            if (GameScorer == null)
                return false;

            if (commandManager == null)
                return false;
            return true;
        }


        public override void AddBoard(int row, int col) 
        {
           
           this.gameBoard = new RectangularBoard(row, col);
            
           
        }
        public override bool IsOver()
        {
            return gameBoard.IsFull();
        }
        public override int GetRequiredPlayers()
        {
            return this.RequiredPlayers;
        }
        //should be in gameLogic
        public override Player GetWinner()
        {
            return findHighScore(); ;
        }
        public override void InitialyzeGame()
        {
         // if the board has not been created(load game), ask for the dimensions, for new game
            if (this.gameBoard == null) 
            {
                int rows = AskInteger("rows");
                int cols = AskInteger("colums");
                AddBoard(rows, cols);
            }
            gameBoard.InitializeBoard();
            NewGameState();
            Players = new Player[this.RequiredPlayers];
        //this could be inject instead
            this.commandManager = new MoveCommandManager(this.GameScorer, this.gameState, this.gameBoard);
        }
        public override void SaveGame() 
        {
        //if the game has not been added to the loadmanger, add it. It coudl have been added by loading the game
            try { loadManager.addGame(this); }catch(Exception e) {  }
            loadManager.SaveGame(); 
        }

        public Player findHighScore()
            //will have to deal with ties
        {
            int score = 0;
            Player best = null;
            foreach (Player player in gameState.players) 
            {
                if (best == null) 
                {
                    best = player;
                    score = gameState.GetScorePlayer(player);
                }
                else if (gameState.GetScorePlayer(player) > score) {
                    score = gameState.GetScorePlayer(player);
                    best = player;
                }
            }
            return best;
        }

        public override void NewGameState()
        {
            this.gameState = new SosGameState(); 
    
        }
        public override void SelectFirstPLayer(Player player=null) 
        { 
            Random random = new Random();
            int randomIndex = random.Next(0, this.RequiredPlayers);
            //here thighly couple
            if (player == null) {
               
                gameState.SetCurrentPlayer(Players[randomIndex]);
            }
            else { gameState.SetCurrentPlayer(player); }
            
        }
        public override void AddPlayer(Player player, int index)
        {
            Players[index]=player;
        }
       
        public override Board GetBoard()
        {
            return gameBoard;
        }

        
        public override void DisplaycenteredBoard()
        {
        //could be a format class. bidplay the board
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            string[] lines = this.gameBoard.Draw().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            int totalLines = lines.Length;
            int startRow = (consoleHeight - totalLines) / 2; // Center vertically

            for (int i = 0; i < totalLines; i++)
            {
                string line = lines[i];
                int startCol = (consoleWidth - line.Length) / 2; // Center horizontally

                Console.SetCursorPosition(startCol, startRow + i);
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }

        public void GameLoop() 
        {
        //while the game is not over, ask for move. the player holds the logic for the move
            while (!gameLogic.isOver(this))
            {
                string flow = null;
                Console.Clear();
                int score = 0;
                IGameCommand move;
                this.IsBackWard = false;
                this.PassTurn = true;
                DisplaycenteredBoard();
                for (int i = 0; i < RequiredPlayers; i++)
                {
                //display the players with the score, the active player is highlighted
                    if (this.gameState.GetPlayer(i) == this.gameState.GetCurrentPlayer())
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    }

                    Console.WriteLine("Player {0}: {1} points", this.gameState.GetPlayer(i), this.gameState.GetScorePlayer(this.gameState.GetPlayer(i)));
                    Console.ResetColor();
                }
                //we need to check the score of the player before the move and after
                //her score before
                score= gameState.GetScorePlayer(gameState.GetCurrentPlayer());
                do
                //ask for a move from the currentplayer, gameLogic check if the move is valid
                { move = gameState.GetCurrentPlayer().MakeMove(gameState, this.GetBoard(), this); }
                while
                (!gameLogic.isValid(move, this.gameBoard));
                if (move is MoveCommand sosMove)
                {
                    commandManager.AddCommand(sosMove);

                }
                else
                {
                //wa have to allow the player to save and quit the game. If not  movecommand, it will be treated differently
                    move.Execute();

                }
                commandManager.ExecuteCommands();
                //case A: undo and no point => stay
                //case B : undo and less => next
                //case C : move and point => stay
                //case D: move and no point => next
                if ((gameState.GetScorePlayer(gameState.GetCurrentPlayer()) == score && IsBackWard) && PassTurn)
                {
                    gameState.PreviousPlayer();
                }
                else if ((gameState.GetScorePlayer(gameState.GetCurrentPlayer()) == score && !IsBackWard) && PassTurn)
                {
                    gameState.NextPlayer();
                }

            }
        }
        public override void StartGame()
        {

            for (int i = 0; i < this.RequiredPlayers; i++)
            {
                gameState.AddPlayer(Players[i]);
             
            }

            if (gameState.GetCurrentPlayer() == null)
            {
                SelectFirstPLayer();
            }
       
            gameState.InitializeScore();
            commandManager.ExecuteCommands();

            GameLoop();
        }
    }
}
