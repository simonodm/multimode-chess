using Chess.Game;
using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class ConfigurableChessBoardControl : ChessBoardControl
    {
        public delegate void MultipleOptionEventHandler(object sender, MultipleOptionEventArgs e);
        public event MultipleOptionEventHandler UserInputRequired
        {
            add
            {
                _onUserInputRequired += value;
            }
            remove
            {
                _onUserInputRequired -= value;
            }
        }
        private event MultipleOptionEventHandler _onUserInputRequired;
        private Board _board;

        public ConfigurableChessBoardControl(int width, int height) : base(width, height)
        {
            _board = new Board(width, height);
            TileClick += Tile_Click;
        }

        public override void UpdateBoard(Board board)
        {
            base.UpdateBoard(board);

            if(board != _board)
            {
                _board = board;
            }
        }

        public Board GetBoard()
        {
            return _board;
        }

        private void OnUserInputRequired(object sender, MultipleOptionEventArgs e)
        {
            _onUserInputRequired?.Invoke(sender, e);
        }

        private void Tile_Click(object sender, EventArgs e)
        {
            var tileSquare = ((ChessBoardTileControl)sender).Square;
            var argsPieces = new MultipleOptionEventArgs()
            {
                Options = GeneratePieceOptions()
            };
            var argsColor = new MultipleOptionEventArgs()
            {
                Options = GenerateColorOptions()
            };
            // TODO: Do not request color if the chosen piece type was None.
            OnUserInputRequired(sender, argsPieces);
            OnUserInputRequired(sender, argsColor);
            if(argsPieces.PickedOption != null && argsColor.PickedOption != null)
            {
                var piece = GetPieceFromIds(argsPieces.PickedOption.Id, argsColor.PickedOption.Id);
                if (tileSquare.GetPiece() != null)
                {
                    _board = _board.RemovePiece(tileSquare);
                }
                if (piece != null)
                {
                    _board = _board.AddPiece(tileSquare, piece);
                }
                UpdateBoard(_board);
            }
        }

        private List<Option> GenerateColorOptions()
        {
            var options = new List<Option>();
            options.Add(new Option(0, "White"));
            options.Add(new Option(1, "Black"));
            return options;
        }

        private List<Option> GeneratePieceOptions()
        {
            var options = new List<Option>();
            string[] pieces = {
                "King",
                "Queen",
                "Rook",
                "Knight",
                "Bishop",
                "Pawn",
                "None"
            };
            for(int i = 0; i < pieces.Length; i++)
            {
                options.Add(new Option(i, pieces[i]));
            }
            return options;
        }

        private GamePiece GetPieceFromIds(int pieceId, int colorId)
        {
            switch (pieceId)
            {
                case 0:
                    return new King(colorId);
                case 1:
                    return new Queen(colorId);
                case 2:
                    return new Rook(colorId);
                case 3:
                    return new Knight(colorId);
                case 4:
                    return new Bishop(colorId);
                case 5:
                    return new Pawn(colorId);
                case 6:
                    return null;
            }
            throw new Exception("Invalid piece id.");
        }
    }
}
