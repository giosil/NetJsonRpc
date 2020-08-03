# Net JSON-RPC

Simple JSON-RPC protocol implementation based on ASP.NET Core project.

## Example

```csharp
using System;
using System.Threading;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;

using NetJsonRpc.Protocol;
using NetJsonRpc.Services;

using NetJsonRpc.Auth;
using Microsoft.AspNetCore.Http;

namespace NetJsonRpc.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RpcController : ControllerBase
  {
    private readonly ILogger<RpcController> _logger;

    public RpcController(ILoggerFactory loggerFactory)
    {
      // Initialize logger
      this._logger = loggerFactory.CreateLogger<RpcController>();

      // Initialize RPC
      RPC.AddHanlder("TEST", new TestService(loggerFactory));
      RPC.AddHanlder("DBMS", new DBService());
    }

    // GET api/rpc
    [HttpGet]
    public ActionResult<string> Get()
    {
      _logger.LogInformation("RpcController.Get...");

      User user = HttpContext.Session.GetUser();

      string checkUser = user != null ? " (" + user.Username + " logged)" : "";

      return "JSON-RPC 2.0 Controller" + checkUser;
    }

    // POST api/rpc
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IDictionary<string, object>> Post([FromBody] IDictionary<string, object> request)
    {
      _logger.LogInformation("RpcController.Post...");

      RPCRequest rpcRequest = new RPCRequest(request);

      User user = HttpContext.Session.GetUser();

      if(rpcRequest.GetHandlerName().Equals("DBMS"))
      {
        if (user == null) return Unauthorized();
      }

      Thread.SetData(Thread.GetNamedDataSlot("user"), user);

      RPCResponse rpcResponse = RPC.Invoke(rpcRequest);

      return rpcResponse.GetDictionary();
    }
  }
}
```

## Contributors

* [Giorgio Silvestris](https://github.com/giosil)
