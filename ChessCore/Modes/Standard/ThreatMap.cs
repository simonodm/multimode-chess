namespace ChessCore.Modes.Standard
{
    class ThreatMap
    {
        int[,] _whiteThreatMap;
        int[,] _blackThreatMap;

        public ThreatMap(StandardBoardState state)
        {
            int width = state.GetBoard().GetWidth();
            int height = state.GetBoard().GetHeight();

            _whiteThreatMap = new int[width, height];
            _blackThreatMap = new int[width, height];

            GenerateThreatMaps(state);
        }

        public int GetThreatCount(int file, int rank, int byPlayer)
        {
            if (byPlayer == 0)
            {
                return _whiteThreatMap[file, rank];
            }
            else
            {
                return _blackThreatMap[file, rank];
            }
        }

        public int GetThreatCount(BoardSquare square, int byPlayer)
        {
            return GetThreatCount(square.GetFile(), square.GetRank(), byPlayer);
        }

        private void GenerateThreatMaps(StandardBoardState state)
        {
            foreach (var square in state.GetBoard().GetAllSquares())
            {
                ProcessSquareThreats(state, square);
            }
        }

        private void ProcessSquareThreats(StandardBoardState state, BoardSquare square)
        {
            var piece = square.GetPiece();
            if (piece != null && piece is StandardPiece)
            {
                var standardPiece = (StandardPiece)piece;

                var threatenedSquares = standardPiece.GetThreatenedSquares(state, square);
                foreach (var threatenedSquare in threatenedSquares)
                {
                    IncrementSquareThreat(standardPiece, threatenedSquare);
                }
            }
        }

        private void IncrementSquareThreat(StandardPiece piece, BoardSquare threatenedSquare)
        {
            if (piece.GetPlayer() == 0)
            {
                _whiteThreatMap[threatenedSquare.GetFile(), threatenedSquare.GetRank()]++;
            }
            else
            {
                _blackThreatMap[threatenedSquare.GetFile(), threatenedSquare.GetRank()]++;
            }
        }
    }
}
