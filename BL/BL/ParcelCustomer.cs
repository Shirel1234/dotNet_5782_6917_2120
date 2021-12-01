namespace IBL.BO
{
    public class ParcelCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses Status { get; set; }
        public int CustomerId { get; set; }
    }
}