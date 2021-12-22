using System.Collections.Generic;

namespace Entry.Protocol
{
    public class Maintanence
    {
        public Maintanence()
        {

        }
    }

    public class Notice
    {
        // - start date
        // - end date
        // - title
        // - content

        public Notice()
        {

        }
    }

    public class AppStore
    {
        // - store link

        public AppStore()
        {

        }
    }

    public class Properties
    {
        // - guest login

        public Properties()
        {

        }
    }

    public class Server
    {
        public string Url { get; set; }
        public string ProtocolVersion { get; set; }
        public string ServiceVersion { get; set; }

        public Server()
        {

        }
    }

    public class RetrieveResult
    {
        public Maintanence Maintanence { get; set; }
        public List<Notice> NoticeList { get; set; }
        public AppStore AppStore { get; set; }
        public Properties Properties { get; set; }
        public Server Server { get; set; }

        public RetrieveResult()
        {

        }
    }
}
