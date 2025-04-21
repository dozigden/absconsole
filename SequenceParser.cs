using System;
using System.Text.RegularExpressions;

namespace AbsConsole
{   
    public static class SequenceParser
    {
        public static int? TryParseSequenceNumber(this string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return null;

            // Regular expression to match a sequence of digits
            var sequenceRegex = new Regex(@"^(?<Sequence>\d+)");
            var match = sequenceRegex.Match(title);
            if (match.Success && int.TryParse(match.Value, out int sequenceNumber))
            {
                return sequenceNumber;
            }

            return null;
        }
    }
}