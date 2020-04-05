namespace Extensions.Http.Mvc
{
    public class PaginationInfo
    {
        public PaginationInfo(int total)
        {
            this.Total = total;
        }

        public int Total { get; }
    }
}
