using System.Runtime.InteropServices;

namespace JobOfferService.Messaging
{
    public static class SubjectBuilder
    {
        private static readonly string SEPARATOR = ".";
        private static readonly string WILDCARD_ANY = "*";
        private static readonly string WILDCARD_ANY_TAIL = ">";

        public static string Build(string Topic, [Optional] string Event) => Topic + SEPARATOR + (Event ?? WILDCARD_ANY);
    }
}
