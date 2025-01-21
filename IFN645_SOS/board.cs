
using System.Text;

namespace IFN645_SOS
{
    /* the board is a game component, it should know when full, be able to return the content of a tile and draw itself. 
     * multiple concrete board could be created: rectangular (squares are rectangle), triangle, etc. We only need the 
     * rectangular board for both our games
     */
    public abstract class Board : GameComponent
    {
        /*a dictionnary is use in case we need a non rectangular shape. 
         * Futur update: we could do with a simple list and use the strategy design pattern to introduce the shape creation algorithms
         */
        protected Dictionary<(int, int), Tile> tiles;
        public int Row { get; set; }
        public int Col { get; set; }

        public abstract void InitializeBoard();
        public abstract GameComponent GetGameComponent(int row, int column);
        public abstract void AddGameComponent(int x, int y, GameComponent gameComponent);

        public abstract bool IsOccupied(int row, int col);
        public abstract bool IsFull();
        public abstract string Draw();

    }


    public class RectangularBoard : Board
    {
        public RectangularBoard(int rows = 3, int columns = 3)
        {
            Row = rows;
            Col = columns;
        }

        //initialize an empty board
        public override void InitializeBoard()
        {
            //we initialize the dict with tile decorated with number. Is mo
            string tileNumber;
            tiles = new Dictionary<(int, int), Tile>();
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    tileNumber = $"{i * Col + j}";
                    Tile tile = new Tile(i, j);
                    NumberPiece piece = new NumberPiece(tileNumber);
                    tiles[(i, j)] = new TileWithNumberPiece(tile, piece);
                }
            }
        }
        public override GameComponent GetGameComponent(int row, int col)
        //return the content of a tileT
        {
            Tile currentCase = tiles[(row, col)];
            if (currentCase is TileWithPiece tileWithPiece)
            {
                return tileWithPiece.GetPiece();
            }
            return null;
        }
        public override bool IsFull()
        {
            foreach (KeyValuePair<(int, int), Tile> kvp in tiles)
            {
                (int row, int col) = kvp.Key;
                Tile tile = kvp.Value;
                if (tile is TileWithNumberPiece)
                {
                    return false;
                }
            }
            return true;
        }
        public override void AddGameComponent(int row, int col, GameComponent gameComponent)
        {

            //implement protection 
            tiles[(row, col)] = new TileWithPiece(tiles[(row, col)], (Piece)gameComponent);
        }
        public override string Draw()
        {


            // Return a string 
            StringBuilder boardDisplay = new StringBuilder();

            int maxNumber = Row * Col - 1;
            int maxSpacing = maxNumber.ToString().Length;

            for (int row = 0; row < this.Row; row++)
            {


                for (int col = 0; col < this.Col; col++)
                {
                    boardDisplay.Append("| " + this.tiles[(row, col)].Draw() + " ");
                    int number = row * this.Col + col;
                    int spacing = maxSpacing - (number).ToString().Length;
                    string spaceBetweenTiles = new string(' ', Math.Max(0, spacing));

                    boardDisplay.Append(spaceBetweenTiles);

                }
                ;
                boardDisplay.AppendLine("|");
                if (row < this.Row - 1)
                {
                    //needed to adjust the horizontal line, depends on the number of characters
                    int modifier = maxSpacing;

                    boardDisplay.AppendLine(new string('-', (this.Col * (3 + maxSpacing)) + 1));
                }
            }
            return boardDisplay.ToString();
        }
        public override bool IsOccupied(int row, int col)
        //check if tile is empty
        {
            return (tiles[(row, col)] is TileWithNumberPiece);
        }
    }//Board
}
