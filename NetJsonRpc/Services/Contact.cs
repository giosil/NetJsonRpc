using System;

namespace NetJsonRpc.Services
{
    public class Phone
    {
        private string type;
        private string number;

        public Phone()
        {
        }

        public Phone(string number)
        {
            this.number = number;
        }

        public Phone(string number, string type)
        {
            this.number = number;
            this.type = type;
        }

        public string Type { get => type; set => type = value; }
        public string Number { get => number; set => number = value; }
    }
}
