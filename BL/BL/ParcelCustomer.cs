namespace IBL.BO
{
    public class ParcelCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses Status { get; set; }
        public CustomerParcel SecondSideCustomer { get; set; }
        public override string ToString()
        {
            return string.Format($"{Id}, {Weight}, {Priority}, {Status}, {SecondSideCustomer}");
        }
    }
}