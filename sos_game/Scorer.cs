using System.Text;

namespace IFN645_SOS
{
    public interface GameScorer
    {
        //here again, could be change for a move general command but not necessary
        int CalculateScore(IMoveCommand move);
    };

    public class SosGameScorer : GameScorer
    {
     
        GameComponent piece;
        Board board;
        int row;
        int col;
       
        public int CalculateScore(IMoveCommand move)
        {

       
            if (move is MoveCommand moveCommand)
            {
              
                this.piece = moveCommand.GetGameComponent();
                this.row = moveCommand.Row;
                this.col = moveCommand.Col;
                this.board = moveCommand.Board;
            }
            else {
                throw new Exception("Unsuported move type");
            }
         
            int score = 0;
            //check horizontale
            //in the futur might be interesting to create methods to check each case \
    
            if (piece.Draw() == "O")
            {
                if (col > 0) 
                {
                    Piece before =(Piece) board.GetGameComponent(row, col - 1);
                    if (before != null && before.Draw() == "S")
                    {
                        if (col + 1 < board.Col)
                        {
                            Piece after = (Piece) board.GetGameComponent(row, col + 1);
                            if (after != null && before.Draw() == "S")
                            {
                                score++;
                            }
                        }
                    }
                }//hori1

                //check vertical
                if (row > 0)
                {
                    Piece before = (Piece)board.GetGameComponent(row - 1, col);
                    if (before != null && before.Draw() == "S")
                    {
                        if (row + 1 < board.Row)
                        {
                            Piece after = (Piece)board.GetGameComponent(row + 1, col);
                            if (after != null && before.Draw() == "S")
                            {
                                score++;
                            }
                        }
                    }
                }//verti1
                    //check horizontal
                if (row > 0 && col > 0)
                {
                    Piece before = (Piece)board.GetGameComponent(row - 1, col - 1);
                    if (before != null && before.Draw() == "S")
                    {
                        if (row + 1 < board.Row && col + 1 < board.Col)
                        {
                            Piece after = (Piece)board.GetGameComponent(row + 1, col + 1);
                            if (after != null && after.Draw() == "S")
                            {
                                score++;
                            }
                        }
                    }
                }//horiz
                if (row > 0 && col + 1 < board.Col)
                {
                    Piece before = (Piece)board.GetGameComponent(row - 1, col + 1);
                    if (before != null && before.Draw() == "S")
                    {
                        if (row + 1 < board.Row && col > 0)
                        {
                            Piece after = (Piece)board.GetGameComponent(row + 1, col - 1);
                            if (after != null && after.Draw() == "S")
                            {
                                score++;
                            }
                        }
                    }
                }//horiz
            }
            //what if is S
            else
            {
                if (col - 2 >= 0)
                {

                    Piece before = (Piece)board.GetGameComponent(row, col - 2);
                    if (before != null && before.Draw() == "S")
                    {
                        before = (Piece)board.GetGameComponent(row, col - 1);
                        if (before != null && before.Draw() == "O")
                        {
                            score++;
                        }
                    }
                }//look hor left
                if (col + 2 < board.Col)
                {

                    Piece after = (Piece)board.GetGameComponent(row, col + 2);
                    if (after != null && after.Draw() == "S")
                    {
                        after = (Piece)board.GetGameComponent(row, col + 1);
                        if (after != null && after.Draw() == "O")
                        {
                            score++;
                        }
                    }
                }//look hor right
                if (row - 2 >= 0)
                {

                    Piece down = (Piece)board.GetGameComponent(row - 2, col);
                    if (down != null && down.Draw() == "S")
                    {
                        down = (Piece)board.GetGameComponent(row - 1, col);
                        if (down != null && down.Draw() == "O")
                        {
                            score++;
                        }
                    }
                }//look up
                if (row + 2 < board.Row)
                {

                    Piece up = (Piece)board.GetGameComponent(row + 2, col);
                    if (up != null && up.Draw() == "S")
                    {
                        up = (Piece)board.GetGameComponent(row + 1, col);
                        if (up != null && up.Draw() == "O")
                        {
                            score++;
                        }
                    }
                }//look down
                if (col - 2 >= 0 && row - 2 >= 0)
                {

                    Piece upLeft = (Piece)board.GetGameComponent(row - 2, col - 2);
                    if (upLeft != null && upLeft.Draw() == "S")
                    {
                        upLeft = (Piece)board.GetGameComponent(row - 1, col - 1);
                        if (upLeft != null && upLeft.Draw() == "O")
                        {
                            score++;
                        }
                    }
                }//upleft
                if (col + 2 < board.Col && row + 2 < board.Row)
                {

                    Piece upRight = (Piece)board.GetGameComponent(row + 2, col + 2);
                    if (upRight != null && upRight.Draw() == "S")
                    {
                        upRight = (Piece)board.GetGameComponent(row + 1, col + 1);
                        if (upRight != null && upRight.Draw() == "O")
                        {
                            score++;
                        }
                    }
                }//upright
                if (col + 2 < board.Col && row - 2 >= 0)
                {

                    Piece DownRight = (Piece)board.GetGameComponent(row - 2, col + 2);
                    if (DownRight != null && DownRight.Draw() == "S")
                    {
                        DownRight = (Piece)board.GetGameComponent(row - 1, col + 1);
                        if (DownRight != null && DownRight.Draw() == "O")
                        {
                            score++;
                        }
                    }
                }//downRight
                
             
            }
            return score;
        }
        
    }

}
