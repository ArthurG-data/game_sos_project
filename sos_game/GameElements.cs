using System.Text;

namespace IFN645_SOS
{
/// <summary>
/// This section is for the game elements
/// </summary>
    public interface GameComponent {
      
        public abstract string Draw();
    }
    public abstract class Piece : GameComponent
    {
      
        public string Shape { get; set; }
        public abstract string Draw();
    }
    public class SosPiece : Piece
    {
      
        
        public SosPiece(string shape)
        {
            this.Shape = shape;
          
        }
        public override string Draw() {
            return Shape;
                }
    }
    public class NumberPiece : Piece 
    {
        public NumberPiece(string shape)
        {
            this.Shape = shape;
        
        }
        public override string Draw()
        {
            return Shape;
        }
    }
    //define the pieces as an abstract to be able to include whatever type of piece we want
    public class Tile : GameComponent
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Tile(int x, int y) 
        {
            Row= x;
            Col= y;
        }
        public Tile() { }
        public virtual string Draw()
        {
            return "";
        }
    }
    public abstract class TileDecorator : Tile 
    {
        GameComponent component { get; set; }

        public TileDecorator(Tile decoratedTile, GameComponent gameComponent) 
        {
            Row = decoratedTile.Row; Col = decoratedTile.Col;
            component = gameComponent;
        }
        public override string Draw() 
        {
            return component.Draw();
        }
    }
    
    public class TileWithPiece : TileDecorator
    { 
        Piece Piece { get; set; }
        public TileWithPiece(Tile tile,GameComponent gameComponent):base(tile,gameComponent)
        {
            if (gameComponent is Piece piece) 
            {
                Piece = piece;
            }
        }
        public Piece GetPiece() { return Piece; }
        public override string Draw()
        {
            return Piece.Draw();
        }
    }
    public class TileWithNumberPiece : TileDecorator
    {
        Piece Piece { get; set; }
        public TileWithNumberPiece(Tile tile, GameComponent gameComponent) : base(tile, gameComponent)
        {
            if (gameComponent is Piece piece)
            {
                Piece = piece;
            }
        }
        public Piece GetPiece() { return Piece; }
        public override string Draw()
        {
            return Piece.Draw();
        }
    }
}
