namespace Assignment1
{
    public class Composer
    {
        private string firstName;
        private string lastName;

        public Composer(string firstName, string lastName)
        {   
            this.firstName = firstName;
            this.lastName = lastName;  

        }

        public string GetName()
        {
            return $"{firstName} {lastName}";
        }

        public void SetName(string firstName, string lastName)
        {
            if (checkName(firstName) && checkName(lastName)) {

                this.firstName = firstName;
                this.lastName = lastName;

            }
        }

        private bool checkName(string name) { 
            if (name != null && name.All(char.IsLetter) && name.Length != 0 && !char.IsUpper([0]) && name.Skip(1).All(char.IsLower)) return true;
            return false;
        }
    }
}
