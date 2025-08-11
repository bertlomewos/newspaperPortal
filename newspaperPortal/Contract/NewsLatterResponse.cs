namespace newspaperPortal.Contract
{
    public class NewsLatterResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ReadTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
