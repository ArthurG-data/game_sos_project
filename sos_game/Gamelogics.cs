using System.Text;

namespace IFN645_SOS
{
 
    public interface IGameLogic
    {
        bool isOver(Game game);
        bool isValid(IGameCommand move, Board board);

        bool isValid();
  

    }
    public class SosGameLogic : IGameLogic 
    {

       
        //get the board
        public bool isOver(Game game) 
        {
          
            return (game.GetBoard().IsFull()) ;
        }
        public bool isValid(IGameCommand move, Board board) 
        {
            if (move is MoveCommand commandMove) {
                if (board.GetGameComponent(commandMove.Row, commandMove.Col) is Piece piece)
                {
                    return false;
                }
                
            }
            return true;
            
        }
        public bool isValid() {
            return true;
        }
        







            
        };

}
