namespace Assignment1
{
    public class Piece
    {
        private string title;
        private Composer composer;
        private string catalogue;

        public Piece(string title, Composer composer, string catalogue)
        {
            
            this.title = title;
            this.composer = composer;
            this.catalogue = catalogue;
        }

        public Piece(string title, string composerName, string catalogue)
        {
            this.title = title;
            this.catalogue = catalogue;
            string[] composerParts = composerName.Split(' ');
            this.composer = new Composer(composerParts[0], composerParts[1]);

        }

        public string GetTitle()
        {
            return title; 
        }
    }
}
