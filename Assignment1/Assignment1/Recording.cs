namespace Assignment1
{
    public class Recording
    {
        private Piece piece;
        private string code;

        public Recording(Piece piece, string code)
        {
            this.piece = piece;
            this.code = code;
        }

        public string GetTitle()
        {
            return piece.GetTitle();
        }
    }
}
