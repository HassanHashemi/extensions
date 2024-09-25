namespace Extensions.Http.Mvc
{
    public class PaginationInfo
    {
        public PaginationInfo()
        {
        }

        public PaginationInfo(int total)
        {
            Total = total;
        }

        // allow json serialization
        public int Total { get; set; }
    }
}
