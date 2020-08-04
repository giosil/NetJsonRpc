# Net JSON-RPC

Simple JSON-RPC protocol implementation based on ASP.NET Core project.

## Build

- `git clone https://github.com/giosil/NetJsonRpc.git`
- `dotnet publish NetJsonRpc/NetJsonRpc.csproj -c Release -o published`

## Run

- `cd NetJsonRpc` - Starting dotnet in the root folder you get http 404 on launch index.html
- `dotnet published/NetJsonRpc.dll`

## Run locally on Docker

- `docker build -t netjsonrpc .`
- `docker run --rm -it -p 5000:80 --name=netjsonrpc_test netjsonrpc`

## Test

- `dotnet build` 
- `dotnet test` 

## Example

```csharp
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
    public RpcController(ILoggerFactory loggerFactory)
    {
      // Initialize RPC
      RPC.AddHanlder("TEST", new TestService());
      RPC.AddHanlder("DBMS", new DBService());
    }
    
    // GET api/rpc
    [HttpGet]
    public ActionResult<string> Get()
    {
      return "JSON-RPC";
    }
    
    // POST api/rpc
    [HttpPost]
    public ActionResult<IDictionary<string, object>> Post([FromBody] IDictionary<string, object> request)
    {
      RPCRequest req = new RPCRequest(request);
      
      RPCResponse res = RPC.Invoke(req);
      
      return res.GetDictionary();
    }
  }
}
```

## Contributors

* [Giorgio Silvestris](https://github.com/giosil)
