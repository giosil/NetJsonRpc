using System;

namespace NetJsonRpc.Services
{
    public class Person
    {
        private string firstname;
        private string lastname;
        private DateTime birthdate;
        private string sex;
        private bool registered;
        private Address address;
        private Phone[] phones;
        private string[] emails;

        public Person()
        {
        }

        public Person(string firstname, string lastname, DateTime birthdate, string sex)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.birthdate = birthdate;
            this.sex = sex;
            this.registered = false;
        }

        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public DateTime Birthdate { get => birthdate; set => birthdate = value; }
        public string Sex { get => sex; set => sex = value; }
        public bool Registered { get => registered; set => registered = value; }
        public Address Address { get => address; set => address = value; }
        public Phone[] Phones { get => phones; set => phones = value; }
        public string[] Emails { get => emails; set => emails = value; }

        public override string ToString()
        {
            return this.firstname + Utils.GetPlace(this) + Utils.GetPhones(this) + Utils.GetEmails(this);
        }
    }
}
