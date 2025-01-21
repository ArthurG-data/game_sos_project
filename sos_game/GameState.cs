using System.Text;

namespace IFN645_SOS
{
    public abstract class GameState
    //holds the informations for a given game and can be save or loaded
    {
        public string gameName { get; set; }
        public Player CurrentPlayer { get; set; }
        public List<Player> players;
        public Dictionary<Player, int> gameScores { get; set; }
        public abstract void AddPlayer(Player player);
        public abstract void SetCurrentPlayer(Player player);
        public abstract void InitializeScore();
        public abstract void NextPlayer();
        public abstract int GetScorePlayer(Player player);
        public abstract void AddPoint(Player player, int point = 1);
        public abstract Player GetCurrentPlayer();
        public abstract Player GetPlayer(int i);
        public abstract void PreviousPlayer();
        
           
    }

    public class SosGameState : GameState
    {
        public SosGameState()
        {
            gameName = "Sos";
            //maybe make an array if faster
            players = new List<Player>();
            gameScores = new Dictionary<Player, int>();
            CurrentPlayer = null;
        }
        public override void AddPoint(Player player, int point = 1)
        {
            if (gameScores.ContainsKey(player))
            {
                gameScores[player] += point;
            }
            else
            {
                throw new KeyNotFoundException($"Player '{player.Name}' not found in gameScores.");
            }
        }
        public override Player GetCurrentPlayer()
        {
            return CurrentPlayer;
        }
        public override Player GetPlayer(int i)
        {
            return players[i];
        }
        public override int GetScorePlayer(Player player)
        {
            if (gameScores.ContainsKey(player))
                return gameScores[player];
            else
            {
                throw new InvalidOperationException($"Player '{player.Name}' does not have a score.");
            }
        }

        public override void PreviousPlayer() 
        {
            int index = players.IndexOf(CurrentPlayer);
            index--;
            if (index > -1)
            {

                SetCurrentPlayer(players[index]);
            }
            else
            {
                SetCurrentPlayer(players[players.Count - 1]);
            }
        }
        public override void NextPlayer()
        {
            
            int index = players.IndexOf(CurrentPlayer);
            index++;
            if (index < players.Count)
            {

                SetCurrentPlayer(players[index]);
            }
            else
            {
                SetCurrentPlayer(players[0]);
            }
        }
        public override void AddPlayer(Player player)
        {
            this.players.Add(player);
        }
        public override void SetCurrentPlayer(Player player)
        {
            CurrentPlayer = player;
        }
        public override void InitializeScore()
        {
            foreach (Player player in players)
            {
                gameScores[player] = 0;
            }
        }
    }
}
