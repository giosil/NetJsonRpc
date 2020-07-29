using NUnit.Framework;

using System.Collections.Generic;

using NetJsonRpc.Protocol;
using NetJsonRpc.Services;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            RPC.AddHanlder("TEST", new TestService());
        }

        [Test]
        public void TestRPC()
        {
            object[] parameters = new object[1];
            parameters[0] = BuildPerson();

            IDictionary<string, object> request = new Dictionary<string, object>();
            request["id"] = 1;
            request["method"] = "TEST.hello";
            request["params"] = parameters;

            RPCResponse response = RPC.Invoke(request);

            Log("response.Success     : " + response.Success);
            Log("response.Result      : " + response.Result);
            Log("response.ErrorCode   : " + response.ErrorCode);
            Log("response.ErrorMessage: " + response.ErrorMessage);

            Assert.IsTrue(response.Success);

            Assert.Pass();
        }

        protected IDictionary<string, object> BuildPerson()
        {
            IDictionary<string, object> phone0 = new Dictionary<string, object>();
            phone0["type"] = "work";
            phone0["number"] = "331-11111111";

            IDictionary<string, object> phone1 = new Dictionary<string, object>();
            phone1["type"] = "work";
            phone1["number"] = "331-11111111";

            IList<IDictionary<string, object>> phones = new List<IDictionary<string, object>>();
            phones.Add(phone0);
            phones.Add(phone1);

            string[] emails = new string[2];
            emails[0] = "test@email.com";
            emails[1] = "dev@emai.com";

            IDictionary<string, object> person = new Dictionary<string, object>();
            person["firstname"] = "World";
            person["phones"] = phones;
            person["emails"] = emails;

            return person;
        }

        protected void Log(string message)
        {
            TestContext.Progress.WriteLine(message);
        }
    }
}
