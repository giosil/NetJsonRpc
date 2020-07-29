using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;

using NetJsonRpc.Protocol;
using NetJsonRpc.Services;

namespace NetJsonRpc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RpcController : ControllerBase
    {
        private readonly ILogger<RpcController> _logger;

        public RpcController(ILogger<RpcController> logger)
        {
            // Initialize logger
            this._logger = logger;

            // Initialize RPC
            RPC.AddHanlder("TEST", new TestService());
            RPC.AddHanlder("DBMS", new DBService());
        }

        // GET api/rpc
        [HttpGet]
        public ActionResult<string> Get()
        {
            _logger.LogInformation("RpcController.Get...");

            return "JSON-RPC 2.0 Controller";
        }

        // POST api/rpc
        [HttpPost]
        public ActionResult<IDictionary<string, object>> Post([FromBody] IDictionary<string, object> request)
        {
            _logger.LogInformation("RpcController.Post...");

            RPCResponse response = RPC.Invoke(request);

            return response.GetDictionary();
        }
    }
}
