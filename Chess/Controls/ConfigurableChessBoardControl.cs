using ChessCore;
using ChessCore.Modes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGUI.Controls
{
    /// <summary>
    /// A control which allows the user to configure the chessboard.
    /// </summary>
    internal class ConfigurableChessBoardControl : ChessBoardControl
    {
        /// <summary>
        /// Occurs when the control requires user input.
        /// </summary>
        public event MultipleOptionEventHandler UserInputRequired;

        private Board _board;
        private IGameRules _rules;

        public ConfigurableChessBoardControl(int width, int height, IGameRules rules) : base(width, height)
        {
            _board = new Board(width, height);
            TileClick += Tile_Click;
            _rules = rules;
        }

        /// <inheritdoc cref="ChessBoardControl.UpdateBoard"/>
        public override void UpdateBoard(Board board)
        {
            base.UpdateBoard(board);

            _board = board;
        }

        /// <summary>
        /// Retrieves the currently displayed board.
        /// </summary>
        /// <returns>A Board instance for the current board configuration</returns>
        public Board GetBoard()
        {
            return _board;
        }

        /// <summary>
        /// Updates the rules to retrieve possible configurations from.
        /// </summary>
        /// <param name="rules">A concrete IGameRules implementation</param>
        public void SetRules(IGameRules rules)
        {
            _rules = rules;
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

            ProcessPieceSelection(tileSquare, pieceArgs, colorArgs);
        }

        private void ProcessPieceSelection(BoardSquare targetSquare, MultipleOptionEventArgs pieceArgs, MultipleOptionEventArgs colorArgs)
        {
            OnUserInputRequired(this, pieceArgs);
            var pickedPieceOption = pieceArgs.PickedOption;
            if (pickedPieceOption != null)
            {
                if (pickedPieceOption.Id == pieceArgs.Options[^1].Id) // None selected
                {
                    if (targetSquare.GetPiece() != null)
                    {
                        _board = _board.RemovePiece(targetSquare);
                    }
                }
                else
                {
                    OnUserInputRequired(this, colorArgs);
                    if (colorArgs.PickedOption != null)
                    {
                        var piece = _rules.GetPieceFactory().GetPiece(pieceArgs.PickedOption.Id, colorArgs.PickedOption.Id);
                        if (targetSquare.GetPiece() != null)
                        {
                            _board = _board.RemovePiece(targetSquare);
                        }
                        _board = _board.AddPiece(targetSquare, piece);
                    }
                }
            }

            UpdateBoard(_board);
        }

        private List<Option> GenerateColorOptions()
        {
            var options = new List<Option>
            {
                new Option(0, "White"),
                new Option(1, "Black")
            };
            return options;
        }

        private List<Option> GeneratePieceOptions()
        {
            var options = _rules.GetPieceFactory().GetPieceOptions().ToList();
            options.Add(new Option(options.Count, "None"));
            return options;
        }
    }
}
