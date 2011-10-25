namespace RHAL9000.Monitors.Builds
{
    public class Summary : ISummary
    {
        public Summary(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public Summary()
        {
        }

        #region ISummary Members

        public string Name { get; set; }
        public string Id { get; set; }

        #endregion
    }
}