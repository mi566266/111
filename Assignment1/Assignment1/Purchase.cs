namespace Assignment1
{
    public class Purchase
    {
        private Recording recording;
        private DateTime time;
        private int amount;
        private double price;

        public Purchase(Recording recording, double price, int amount, DateTime time)
        {
            this.recording = recording;
            this.price = price;
            this.amount = amount;
            this.time = time;
        }

        public DateTime GetPurchaseDay()
        {
            return new DateTime(time.Year, time.Month, time.Day);
        }

        public double GetTotal()
        {
            return price * amount;
        }
    }
}
