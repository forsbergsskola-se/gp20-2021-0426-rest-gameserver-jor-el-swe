using System;
namespace TinyBrowser {
    public static class ParserHelper {
     
        public static HostAndPath TrimPathName(string hostname, string hyperLink)
        {
            var pathName = hyperLink.TrimStart('/');
            pathName = "/" + pathName;
            
            return new HostAndPath(hostname, pathName);
        }
        

        public static string FindStringBetweenTwoStrings(string sourceString, string startString, string endString, int startAtPosition, out int foundPosition)
        {
            foundPosition = sourceString.IndexOf(startString, startAtPosition, StringComparison.OrdinalIgnoreCase);
            if (foundPosition == -1)
            {
                return "";
            }
            
            foundPosition += startString.Length;
            var pTo = sourceString.IndexOf(endString, foundPosition, StringComparison.OrdinalIgnoreCase);
            var result = sourceString[foundPosition..pTo];
            return result;
        }
    }
}