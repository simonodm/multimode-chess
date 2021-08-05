namespace ChessCore
{
    static class IntExt
    {
        public static char ConvertToChessFile(this int file)
        {
            return (char)(97 + file);
        }
    }
}
