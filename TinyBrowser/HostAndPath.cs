namespace TinyBrowser {
    public struct HostAndPath
    {
        public HostAndPath(string host, string path)
        {
            HostName = host;
            PathName = path;
        }
        public string HostName {get; set; }
        public string PathName {get; set; }
    }
}