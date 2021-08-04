using ChessCore.Game;
using ChessCore.Game.Modes.Standard.Pieces;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess.Controls
{
    class ChessBoardTileControl : Control
    {
        public BoardSquare Square
        {
            get
            {
                return _square;
            }
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
                Size = new Size(Size.Width, Size.Height)
            };

            _tile.BackColor = GetColor();
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
                piecePictureBox = new PictureBox();
                piecePictureBox.Image = GetPieceBitmap();
                piecePictureBox.Size = new Size(Size.Width, Size.Height);
                piecePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                piecePictureBox.Click += Tile_Click;

                if(_tile.Controls.Count > 0)
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
                if (_square.GetRank() % 2 == 0)
                {
                    return Color.DarkGreen;
                }
                else
                {
                    return Color.White;
                }
            }
            else
            {
                if (_square.GetRank() % 2 == 0)
                {
                    return Color.White;
                }
                else
                {
                    return Color.DarkGreen;
                }
            }
        }

        private Bitmap GetPieceBitmap()
        {
            var piece = _square.GetPiece();
            string imageResource = "";
            if (piece is Pawn)
            {
                imageResource = piece.GetPlayer() == 0 ? "pawn_light" : "pawn_dark";
            }
            if (piece is King)
            {
                imageResource = piece.GetPlayer() == 0 ? "king_light" : "king_dark";
            }
            if (piece is Knight)
            {
                imageResource = piece.GetPlayer() == 0 ? "knight_light" : "knight_dark";
            }
            if (piece is Queen)
            {
                imageResource = piece.GetPlayer() == 0 ? "queen_light" : "queen_dark";
            }
            if (piece is Bishop)
            {
                imageResource = piece.GetPlayer() == 0 ? "bishop_light" : "bishop_dark";
            }
            if (piece is Rook)
            {
                imageResource = piece.GetPlayer() == 0 ? "rook_light" : "rook_dark";
            }

            Bitmap bmp = (Bitmap)Properties.Resources.ResourceManager.GetObject(imageResource);
            return bmp;
        }
    }
}
