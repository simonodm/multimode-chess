namespace ChessCore
{
    internal static class IntExt
    {
        /// <summary>
        /// Converts the given file index to the standard notation used in chess (0 is A, 1 is B, ...)_
        /// </summary>
        /// <param name="file">File to convert</param>
        /// <returns>A character representing the file</returns>
        public static char ConvertToChessFile(this int file)
        {
            return (char)(97 + file);
        }
    }
}
