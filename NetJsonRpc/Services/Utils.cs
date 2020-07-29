using System;
using System.Collections.Generic;

namespace NetJsonRpc.Services
{
    public static class Utils
    {
        public static string GetPlace(Person person)
        {
            if (person == null) return "";

            Address address = person.Address;

            if (address == null) return "";

            string result = address.Place;

            if (result == null || result.Length == 0) return "";

            return " (" + result + ")";
        }

        public static string GetPhones(Person person)
        {
            if (person == null) return "";

            IList<Phone> phones = person.Phones;

            if (phones == null || phones.Count == 0) return "";

            string result = "";

            foreach (Phone item in phones)
            {
                result += "," + item.Number;
            }

            if (result.Length == 0) return "";

            return " [" + result.Substring(1) + "]";
        }

        public static string GetEmails(Person person)
        {
            if (person == null) return "";

            IList<string> emails = person.Emails;

            if (emails == null || emails.Count == 0) return "";

            string result = "";

            foreach (string item in emails)
            {
                result += "," + item;
            }

            if (result.Length == 0) return "";

            return " [" + result.Substring(1) + "]";
        }
    }
}
