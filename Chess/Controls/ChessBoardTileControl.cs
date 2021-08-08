using ChessCore;
using ChessCore.Modes.Standard.Pieces;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess.Controls
{
    class ChessBoardTileControl : Control
    {
        public BoardSquare Square
        {
            get => _square;
            set
            {
                _previousPiece = _square.GetPiece();
                _square = value;
                Invalidate();
            }
        }

        public bool IsHighlighted { get; private set; }
        private GamePiece _previousPiece;
        private BoardSquare _square;
        private Panel _tile;

        public ChessBoardTileControl(BoardSquare square)
        {
            _square = square;
            InitializeControls();
        }

        public void Highlight()
        {
            _tile.BackColor = Color.DarkRed;
            IsHighlighted = true;
        }

        public void RemoveHighlighting()
        {
            _tile.BackColor = GetColor();
            IsHighlighted = false;
        }

        private void InitializeControls()
        {
            _tile = new Panel
            {
                Size = new Size(Size.Width, Size.Height),
                BackColor = GetColor()
            };
            _tile.Click += Tile_Click;

            Controls.Add(_tile);
        }

        private void Tile_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _tile.Size = new Size(Size.Width, Size.Height);

            if (_square.GetPiece() != _previousPiece || _square.GetPiece() != null)
            {
                PictureBox piecePictureBox = null;
                piecePictureBox = new PictureBox
                {
                    Image = GetPieceBitmap(),
                    Size = new Size(Size.Width, Size.Height),
                    SizeMode = PictureBoxSizeMode.Zoom
                };
                piecePictureBox.Click += Tile_Click;

                if (_tile.Controls.Count > 0)
                {
                    var currentPictureBox = _tile.Controls[0];
                    _tile.Controls.Clear();
                    currentPictureBox.Dispose();
                }

                _tile.Controls.Add(piecePictureBox);
            }
        }

        private Color GetColor()
        {
            if (_square.GetFile() % 2 == 0)
            {
                return _square.GetRank() % 2 == 0 ? Color.DarkGreen : Color.White;
            }
            else
            {
                return _square.GetRank() % 2 == 0 ? Color.White : Color.DarkGreen;
            }
        }

        private Bitmap GetPieceBitmap()
        {
            var piece = _square.GetPiece();
            string imageResource = piece switch
            {
                Pawn => piece.GetPlayer() == 0 ? "pawn_light" : "pawn_dark",
                King => piece.GetPlayer() == 0 ? "king_light" : "king_dark",
                Knight => piece.GetPlayer() == 0 ? "knight_light" : "knight_dark",
                Queen => piece.GetPlayer() == 0 ? "queen_light" : "queen_dark",
                Bishop => piece.GetPlayer() == 0 ? "bishop_light" : "bishop_dark",
                Rook => piece.GetPlayer() == 0 ? "rook_light" : "rook_dark",
                _ => ""
            };

            var bmp = (Bitmap)Properties.Resources.ResourceManager.GetObject(imageResource);
            return bmp;
        }
    }
}
