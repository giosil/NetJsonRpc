using System;

namespace NetJsonRpc.Services
{
    public class Address
    {
        private string streetName;
        private string number;
        private string zipCode;
        private string place;

        public Address()
        {
        }

        public Address(string place)
        {
            this.place = place;
        }

        public Address(string place, string streetName, string number, string zipCode)
        {
            this.place = place;
            this.streetName = streetName;
            this.number = number;
            this.zipCode = zipCode;
        }

        public string StreetName { get => streetName; set => streetName = value; }
        public string Number { get => number; set => number = value; }
        public string ZipCode { get => zipCode; set => zipCode = value; }
        public string Place { get => place; set => place = value; }
    }
}
