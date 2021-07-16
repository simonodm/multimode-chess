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
//            if(_from.Piece is Knight)
//            {
//                return false;
//            }

//            bool pieceFound = false;
//            int startFile = _from.File;
//            int endFile = _to.File;
//            int startRank = _from.Rank;
//            int endRank = _to.Rank;
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

//            if (_to.Piece != null && _to.Piece.Player == _from.Piece.Player)
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
//                if (square.Piece.Player == player && IsSquareUnderThreat(square, player))
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        private bool IsPreventedByCheck()
//        {
//            return IsCheck(_state.RemoveAt(_from), _from.Piece.Player);
//        }

//        private bool IsCapture()
//        {
//            if (IsEnPassant())
//            {
//                return true;
//            }

//            if (_from.Piece is Pawn)
//            {
//                if (_from.File == _to.File ||
//                   (_from.Piece.Player == 0 && _to.Rank < _from.Rank) ||
//                   (_from.Piece.Player == 1 && _to.Rank > _from.Rank))
//                {
//                    return false;
//                }
//            }

//            if (_to.Piece != null && _to.Piece.Player != _from.Piece.Player)
//            {
//                return true;
//            }

//            return false;
//        }

//        private bool IsEnPassant()
//        {
//            if(_previousMove != null && _previousMove.Piece is Pawn && Math.Abs(_previousMove.To.Rank - _previousMove.From.Rank) == 2 && GetEnPassantSquare(_previousMove) == _to)
//            {
//                return true;
//            }
//            return false;
//        }

//        private BoardSquare GetEnPassantSquare(Move move)
//        {
//            var file = move.From.File;
//            var rank = move.From.Rank + (move.To.Rank - move.From.Rank) / 2;
//            return move.BoardBefore.GetSquare(file, rank);
//        }

//        private bool IsPromotion()
//        {
//            return _from.Piece is Pawn && (_to.Rank == 0 || _to.Rank == 7);
//        }

//        private bool IsCastle()
//        {
//            if (CheckBaseCastleConditions())
//            {
//                for (int i = Math.Min(_from.File, _to.File); i <= Math.Max(_from.File, _to.File); i++)
//                {
//                    if (IsSquareUnderThreat(_state.GetSquare(i, _from.Rank), _from.Piece.Player))
//                    {
//                        return false;
//                    }
//                }
//                var expectedRook = GetRookSquare().Piece;
//                if (expectedRook is Rook && expectedRook.Player == _from.Piece.Player && expectedRook.MoveCount == 0)
//                {
//                    return true;
//                }
//            }
//            return false;
//        }

//        private bool CheckBaseCastleConditions()
//        {
//            return _from.Piece is King &&
//                Math.Abs(_to.File - _from.File) == 2 &&
//                _from.File == 4 &&
//                _from.Piece.MoveCount == 0 &&
//                (_from.Rank == 0 || _from.Rank == 7);
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
//            if (_to.File < _from.File)
//            {
//                return _state.GetSquare(0, _from.Rank);
//            }
//            else
//            {
//                return _state.GetSquare(7, _from.Rank);
//            }
//        }

//        private bool IsNormal()
//        {
//            if (_to.Piece == null)
//            {
//                if (_from.Piece is King)
//                {
//                    return Math.Abs(_from.Rank - _to.Rank) <= 1 && Math.Abs(_from.File - _to.File) <= 1;
//                }
//                if (_from.Piece is Pawn)
//                {
//                    if (_to.File != _from.File)
//                    {
//                        return false;
//                    }
//                    if (_from.Piece.Player == 0 && _to.Rank - _from.Rank < 0)
//                    {
//                        return false;
//                    }
//                    if (_from.Piece.Player == 1 && _from.Rank - _to.Rank < 0)
//                    {
//                        return false;
//                    }
//                    if (_to.Rank - _from.Rank == 2)
//                    {
//                        return _from.Rank == 1;
//                    }
//                    if (_from.Rank - _to.Rank == 2)
//                    {
//                        return _from.Rank == 6;
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
