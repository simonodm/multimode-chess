using Chess.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Chess.Game.Pieces;

namespace Chess
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
                _square = value;
                Invalidate();
                Update();
            }
        }

        public Panel Tile;
        public bool IsHighlighted
        {
            get
            {
                return _isHighlighted;
            }
        }
        private BoardSquare _square;
        private bool _isHighlighted = false;

        public ChessBoardTileControl(BoardSquare square)
        {
            _square = square;
        }

        public void Highlight()
        {
            Tile.BackColor = Color.DarkRed;
            _isHighlighted = true;
            Invalidate();
        }

        public void RemoveHighlighting()
        {
            Tile.BackColor = GetColor();
            _isHighlighted = false;
            Invalidate();
        }

        private void Tile_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            PictureBox piecePictureBox = null;
            
            if (_square.GetPiece() != null)
            {
                piecePictureBox = new PictureBox();
                piecePictureBox.Image = GetPieceBitmap();
                piecePictureBox.Size = new Size(Size.Width, Size.Height);
                piecePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                piecePictureBox.Click += Tile_Click;
            }

            if (Tile == null)
            {
                Tile = new Panel
                {
                    Size = new Size(Size.Width, Size.Height)
                };

                Tile.BackColor = GetColor();
                Tile.Click += Tile_Click;
                if(piecePictureBox != null)
                {
                    Tile.Controls.Add(piecePictureBox);
                }

                Controls.Add(Tile);
            }
            else
            {
                Tile.Controls.Clear();
                if(piecePictureBox != null)
                {
                    Tile.Controls.Add(piecePictureBox);
                }
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
