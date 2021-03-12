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

        public int Total { get; }
    }
}
