//using Chess.Game.Pieces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Chess.Game.Modes
//{
//    class ClassicModeMoveFactory : IMoveFactory
//    {
//        private BoardState _state;
//        private BoardSquare _from;
//        private BoardSquare _to;
//        private Move _previousMove;


//        public Move GetMove(BoardState state, BoardSquare from, BoardSquare to, Move previousMove = null)
//        {
//            _state = state;
//            _from = from;
//            _to = to;
//            _previousMove = previousMove;


//            Move move;

//            move = GenerateMove<MoveCastle>(from, to, previousMove);
//            if(move.IsLegal())
//            {
//                return move;
//            }

//            move = GenerateMove<MovePromotion>(from, to, previousMove);
//            if(move.IsLegal()) {
//                return move;
//            }

//            move = GenerateMove<MoveEnPassant>(from, to, previousMove);
//            if(move.IsLegal())
//            {
//                return move;
//            }

//            move = GenerateMove<MoveCapture>(from, to, previousMove);
//            if(move.IsLegal())
//            {
//                return move;
//            }

//            move = GenerateMove<MoveNormal>(from, to, previousMove);
//            if(move.IsLegal())
//            {
//                return move;
//            }
//        }

//        private Move GenerateMove<TMove>(BoardSquare from, BoardSquare to, Move previousMove) where TMove : Move, new()
//        {
//            return new TMove
//            {
//                From = from,
//                To = to,
//                Piece = from.Piece,
//                Previous = previousMove
//            };
//        }

//        private bool IsBlocked()
//        {
//            if(_from.GetPiece() is Knight)
//            {
//                return false;
//            }

//            bool pieceFound = false;
//            int startFile = _from.GetFile();
//            int endFile = _to.GetFile();
//            int startRank = _from.GetRank();
//            int endRank = _to.GetRank();
//            int i = startFile;
//            int j = startRank;

//            while (i != endFile || j != endRank)
//            {
//                var piece = _state.GetSquare(i, j).Piece;
//                if (piece != null && piece != _from.Piece)
//                {
//                    pieceFound = true;
//                }
//                if (pieceFound)
//                {
//                    return true;
//                }
//                if (startFile < endFile && i < endFile) i++;
//                if (startFile > endFile && i > endFile) i--;
//                if (startRank < endRank && j < endRank) j++;
//                if (startRank > endRank && j > endRank) j--;
//            }

//            if (_to.GetPiece() != null && _to.GetPiece().Player == _from.GetPiece().Player)
//            {
//                return true;
//            }

//            return false;
//        }
        
//        private bool IsCheck(BoardState state, int player)
//        {
//            var kingSquares = _state.FindPieces<King>();

//            foreach (var square in kingSquares)
//            {
//                if (square.GetPiece().Player == player && IsSquareUnderThreat(square, player))
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        private bool IsPreventedByCheck()
//        {
//            return IsCheck(_state.RemoveAt(_from), _from.GetPiece().Player);
//        }

//        private bool IsCapture()
//        {
//            if (IsEnPassant())
//            {
//                return true;
//            }

//            if (_from.GetPiece() is Pawn)
//            {
//                if (_from.GetFile() == _to.GetFile() ||
//                   (_from.GetPiece().Player == 0 && _to.GetRank() < _from.GetRank()) ||
//                   (_from.GetPiece().Player == 1 && _to.GetRank() > _from.GetRank()))
//                {
//                    return false;
//                }
//            }

//            if (_to.GetPiece() != null && _to.GetPiece().Player != _from.GetPiece().Player)
//            {
//                return true;
//            }

//            return false;
//        }

//        private bool IsEnPassant()
//        {
//            if(_previousMove != null && _previousmove.Piece is Pawn && Math.Abs(_previousMove.To.GetRank() - _previousMove.From.GetRank()) == 2 && GetEnPassantSquare(_previousMove) == _to)
//            {
//                return true;
//            }
//            return false;
//        }

//        private BoardSquare GetEnPassantSquare(Move move)
//        {
//            var file = move.From.GetFile();
//            var rank = move.From.GetRank() + (move.To.GetRank() - move.From.GetRank()) / 2;
//            return move.BoardBefore.GetSquare(file, rank);
//        }

//        private bool IsPromotion()
//        {
//            return _from.GetPiece() is Pawn && (_to.GetRank() == 0 || _to.GetRank() == 7);
//        }

//        private bool IsCastle()
//        {
//            if (CheckBaseCastleConditions())
//            {
//                for (int i = Math.Min(_from.GetFile(), _to.GetFile()); i <= Math.Max(_from.GetFile(), _to.GetFile()); i++)
//                {
//                    if (IsSquareUnderThreat(_state.GetSquare(i, _from.GetRank()), _from.GetPiece().Player))
//                    {
//                        return false;
//                    }
//                }
//                var expectedRook = GetRookSquare().Piece;
//                if (expectedRook is Rook && expectedRook.Player == _from.GetPiece().Player && expectedRook.MoveCount == 0)
//                {
//                    return true;
//                }
//            }
//            return false;
//        }

//        private bool CheckBaseCastleConditions()
//        {
//            return _from.GetPiece() is King &&
//                Math.Abs(_to.GetFile() - _from.GetFile()) == 2 &&
//                _from.GetFile() == 4 &&
//                _from.GetPiece().MoveCount == 0 &&
//                (_from.GetRank() == 0 || _from.GetRank() == 7);
//        }

//        private bool IsSquareUnderThreat(BoardSquare square, int forPlayer)
//        {
//            foreach(var squareFrom in _state.GetAllSquares())
//            {
//                if(squareFrom != square)
//                {
//                    var move = GetMove(_state, squareFrom, square);
//                    if(move != null && move.Piece.Player != forPlayer)
//                    {
//                        return true;
//                    }
//                }
//            }
//            return false;
//        }

//        private BoardSquare GetRookSquare()
//        {
//            if (_to.GetFile() < _from.GetFile())
//            {
//                return _state.GetSquare(0, _from.GetRank());
//            }
//            else
//            {
//                return _state.GetSquare(7, _from.GetRank());
//            }
//        }

//        private bool IsNormal()
//        {
//            if (_to.GetPiece() == null)
//            {
//                if (_from.GetPiece() is King)
//                {
//                    return Math.Abs(_from.GetRank() - _to.GetRank()) <= 1 && Math.Abs(_from.GetFile() - _to.GetFile()) <= 1;
//                }
//                if (_from.GetPiece() is Pawn)
//                {
//                    if (_to.GetFile() != _from.GetFile())
//                    {
//                        return false;
//                    }
//                    if (_from.GetPiece().Player == 0 && _to.GetRank() - _from.GetRank() < 0)
//                    {
//                        return false;
//                    }
//                    if (_from.GetPiece().Player == 1 && _from.GetRank() - _to.GetRank() < 0)
//                    {
//                        return false;
//                    }
//                    if (_to.GetRank() - _from.GetRank() == 2)
//                    {
//                        return _from.GetRank() == 1;
//                    }
//                    if (_from.GetRank() - _to.GetRank() == 2)
//                    {
//                        return _from.GetRank() == 6;
//                    }
//                    return true;
//                }
//                else
//                {
//                    return true;
//                }
//            }
//            return false;
//        }

//    }
//}
