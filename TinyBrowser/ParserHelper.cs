using System;
namespace TinyBrowser {
    public static class ParserHelper {
     
        public static HostAndPath FindHostAndPath(string hostname, string hyperLink) {
            const string httpProtocol = "http://";
            var pathName = "/";
            //1. detect if hyperlink contains a new host
            if (hyperLink.Contains(httpProtocol)) {
                //2. assign new host
                hostname = FindStringBetweenTwoStrings(hyperLink, httpProtocol, "/", 0, out var pos, out var endPosition);
                //3. extract pathname
                pathName = "/" + hyperLink[endPosition..hyperLink.Length];
            }
            
            //else, trim path
            else
            {
                pathName = hyperLink.TrimStart('/');
                pathName = "/" + pathName;   
            }

            
            return new HostAndPath(hostname, pathName);
        }
        

        public static string FindStringBetweenTwoStrings(string sourceString, string startString, string endString, int startAtPosition, out int foundPosition, out int endPosition)
        {
            foundPosition = sourceString.IndexOf(startString, startAtPosition, StringComparison.OrdinalIgnoreCase);
            if (foundPosition == -1) {
                endPosition = -1;
                return "";
            }
            
            foundPosition += startString.Length;
            var pTo = sourceString.IndexOf(endString, foundPosition, StringComparison.OrdinalIgnoreCase);
            endPosition = pTo + endString.Length;
            var result = sourceString[foundPosition..pTo];
            return result;
        }
    }
}