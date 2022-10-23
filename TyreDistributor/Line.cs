namespace TyreDistributor
{
    public class Line
    {
        public Tyre Tyre { get; }
        public int Quantity { get; }
        public Line(Tyre tyre, int quantity)
        {
            Tyre = tyre;
            Quantity = quantity;
        }

     
    }
}