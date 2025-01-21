
using System.Text;


namespace IFN645_SOS
{
    
    public class LoadManager
    {
        const char DELIM = ',';
        const string FILENAME = "saves.txt";
        const string MOVEDELIM = "Moves";
        const string CURRENTPLAYER = "currentPlayer";
        const string PLAYERTAG = "Players";
        Game game=null;
        const int MOVEINFOLENGHT = 4;
        const int PLAYERINFOLENGHT = 3;
        public void addGame(Game game) 
        {
            if (this.game == null)
            {
                this.game = game;
            }
            else 
            {
                throw new Exception("LoadManager already has a game");
            }
        }
      
        public  void SaveGame()
        {
            //we need the gameName, get an name for the save, the players ID and Name, the move coordinates and piece.
            game.PassTurn = false;
            StringBuilder builder = new StringBuilder();
            FileStream outFile = new FileStream(FILENAME, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(outFile);
            Console.Write("Enter a name for the saved game: ");
            string saveId = Console.ReadLine();
            //0
            builder.Append(saveId);
            builder.Append(DELIM);
            string outPut;
            //1
            builder.Append(game.Name + DELIM);
            //2,3
            builder.Append(game.gameBoard.Row.ToString() + DELIM + game.gameBoard.Col.ToString() + DELIM);
            builder.Append(PLAYERTAG + DELIM);
            for (int i = 0; i < game.GetRequiredPlayers(); i++)
            {
                //new
                builder.Append(game.Players[i].Name + DELIM + game.Players[i].Id + DELIM + game.Players[i].type + DELIM);
            }
            builder.Append(PLAYERTAG + DELIM);
            builder.Append(CURRENTPLAYER + DELIM + game.gameState.GetCurrentPlayer() + DELIM);
            builder.Append(MOVEDELIM + DELIM);
            foreach (MoveCommand move in game.commandManager.GetCommands())
            {
                builder.Append(move.Player.ToString() + DELIM + move.Row.ToString() + DELIM + move.Col.ToString() + DELIM + move.GetGameComponent().Draw() + DELIM);
            }
            writer.WriteLine(builder.ToString());
            writer.Close();
            outFile.Close();

        }
        public void LoadGame(string savedInfo)

        //introduce the command from the main interface, start a new game with same player and size
        //The method will look for specific tags and update the game eith the information provided
        {
            Console.WriteLine(savedInfo);
            string[] fields = savedInfo.Trim().Split(DELIM);
            //players will be stored in an array
            Player[] players=null;
            List<IMoveCommand> commands = new List<IMoveCommand>();
            //find where the players are stored, between two tags and with 3 parameters for each
            int startPLayerArray = 0;
            int endPLayerArray = 0;
            while (fields[startPLayerArray] != PLAYERTAG)
            { startPLayerArray++; }
            endPLayerArray = startPLayerArray + 1;
            while (fields[endPLayerArray] != PLAYERTAG)
            { endPLayerArray++; }
            //create new players
            try
            {
                players = new Player[(endPLayerArray - startPLayerArray) / PLAYERINFOLENGHT];
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"unavble to create player in the load game {ex.Message}");
            }
            
            int playerIndex = 0;
            //add the correct moveStartegy to the player. We have only two, but can imagine we could have a dict with all the available start if multiple games
            for (int w = startPLayerArray + 1; w < endPLayerArray; w += PLAYERINFOLENGHT)
            {
                try
                {
                    players[playerIndex] = new Player(fields[w], int.Parse(fields[w + 1]));
                    if (fields[w + 2] == "Human") { players[playerIndex].AddStrategy(new HumanSosMoveStategy()); } else { players[playerIndex].AddStrategy(new AiMoveStategyEasy()); }
                    playerIndex++;
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"unavble to assign strat in the load game {ex.Message}");
                }
               

            }

            try
            {
                game.AddBoard(int.Parse(fields[0]), int.Parse(fields[1]));
            }
            catch (MissingMethodException)
            {
                Console.WriteLine("The 'addBoard' method is not available in the current game object.");
            }

            int startMove = 0;
            while (fields[startMove] != MOVEDELIM)
            { startMove++; }
            for (int j = startMove + 1; j <= fields.Length - MOVEINFOLENGHT; j += MOVEINFOLENGHT)
            {
                foreach (Player player in players)
                {
                    if (fields[j] == player.ToString())
                    {
                        MoveCommand newMove = new MoveCommand(player, int.Parse(fields[j + 1]), int.Parse(fields[j + 2]), new SosPiece(fields[j + 3]), game.GetBoard());
                        commands.Add(newMove);
                    }
                }
            }

            //maybe move
            game.InitialyzeGame();

            for (int z = 0; z < players.Length; z++)
            {
                game.AddPlayer(players[z], z);

            }
            foreach (MoveCommand command in commands) 
            {
                game.commandManager.AddCommand(command);
            }
            int i = 0;
            while (fields[i] != CURRENTPLAYER)
            { i++; }

            foreach (Player player in players)
            {
                if (fields[i + 1] == players.ToString())
                {
                    game.SelectFirstPLayer(player);
                }
            }
        }
    }
}
