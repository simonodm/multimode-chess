using ChessCore.Game;
using ChessCore.Game.Modes.Standard.Pieces;
using System;
using System.Collections.Generic;

namespace Chess.Controls
{
    class ConfigurableChessBoardControl : ChessBoardControl
    {
        public event MultipleOptionEventHandler UserInputRequired;
        private Board _board;

        public ConfigurableChessBoardControl(int width, int height) : base(width, height)
        {
            _board = new Board(width, height);
            TileClick += Tile_Click;
        }

        public override void UpdateBoard(Board board)
        {
            base.UpdateBoard(board);

            if (board != _board)
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
            UserInputRequired?.Invoke(sender, e);
        }

        private void Tile_Click(object sender, EventArgs e)
        {
            var tileSquare = ((ChessBoardTileControl)sender).Square;

            var pieceArgs = new MultipleOptionEventArgs()
            {
                Options = GeneratePieceOptions()
            };
            var colorArgs = new MultipleOptionEventArgs()
            {
                Options = GenerateColorOptions()
            };

            // TODO: Do not request color if the chosen piece type was None.
            OnUserInputRequired(sender, pieceArgs);
            OnUserInputRequired(sender, colorArgs);

            ProcessSelectedOptions(tileSquare, pieceArgs, colorArgs);
        }

        private void ProcessSelectedOptions(BoardSquare targetSquare, MultipleOptionEventArgs pieceArgs, MultipleOptionEventArgs colorArgs)
        {
            if (pieceArgs.PickedOption != null && colorArgs.PickedOption != null)
            {
                var piece = GetPieceFromIds(pieceArgs.PickedOption.Id, colorArgs.PickedOption.Id);

                if (targetSquare.GetPiece() != null)
                {
                    _board = _board.RemovePiece(targetSquare);
                }
                if (piece != null)
                {
                    _board = _board.AddPiece(targetSquare, piece);
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
            for (int i = 0; i < pieces.Length; i++)
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
