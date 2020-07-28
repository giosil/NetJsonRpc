using System;

namespace NetJsonRpc.Services
{
    public class TestService
    {
        public string Hello(Person person)
        {
            if(person.Firstname != null && person.Firstname.Equals("error"))
            {
                throw new Exception("Test error");
            }

            return "Hello " + person + ".";
        }

        public string Hello(Person person, string locale)
        {
            if (locale != null && locale.ToLower().Equals("it"))
            {
                return "Ciao " + person + ".";
            }

            return "Hello " + person + ".";
        }
    }
}
