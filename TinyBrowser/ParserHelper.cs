using System;
namespace TinyBrowser {
    public static class ParserHelper {
     
        public static HostAndPath FindHostAndPath(string hostname, string currentPath, string hyperLink) {
            const string httpProtocol = "http://";
            const string httpsProtocol = "https://";
            var pathName = "/";
            //1. detect if hyperlink contains a new host
            if (hyperLink.Contains(httpProtocol)) {
                //2. assign new host
                hostname = FindStringBetweenTwoStrings(hyperLink, httpProtocol, "/", 0, out var pos, out var endPosition);
                //3. extract pathname
                pathName = "/" + hyperLink[endPosition..hyperLink.Length];
            }
            else if (hyperLink.Contains(httpsProtocol)) {
                //2. assign new host
                hostname = FindStringBetweenTwoStrings(hyperLink, httpsProtocol, "/", 0, out var pos, out var endPosition);
                //3. extract pathname
                pathName = "/" + hyperLink[endPosition..hyperLink.Length];
            }
            
            //else, trim path
            else
            {
                //check if link is build on the current path, or a "root" link
                if (hyperLink[0] == '/') {
                    pathName = hyperLink.TrimStart('/');
                    pathName = "/" + pathName;  
                }
                else {
                    pathName = currentPath + hyperLink;
                }
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