using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using NetJsonRpc.Protocol;
using NetJsonRpc.Services;

namespace NetJsonRpc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RpcController : ControllerBase
    {
        public RpcController()
        {
            // Initialize RPC
            RPC.AddHanlder("TEST", new TestService());
        }

        // GET api/rpc
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "JSON-RPC 2.0 Controller";
        }

        // POST api/rpc
        [HttpPost]
        public ActionResult<IDictionary<string, object>> Post([FromBody] IDictionary<string, object> request)
        {
            RPCResponse response = RPC.Invoke(request);

            return response.GetDictionary();
        }
    }
}
