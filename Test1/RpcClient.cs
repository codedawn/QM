using System;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class RpcClient : DynamicObject
{
    private readonly string _serviceUrl;

    public RpcClient(string serviceUrl)
    {
        _serviceUrl = serviceUrl;
    }

    // Override to intercept dynamic method calls
    public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
    {
        result = CallRpcMethodAsync(binder.Name, args);
        return true;
    }

    // Simulate async RPC call
    private async Task<object> CallRpcMethodAsync(string methodName, object[] args)
    {
        using (HttpClient client = new HttpClient())
        {
            // Simulate creating a request to call the remote method
            var requestJson = new
            {
                Method = methodName,
                Params = args,
            };

            // Here we can perform an actual HTTP request to the service
            HttpResponseMessage response = await client.PostAsJsonAsync($"{_serviceUrl}/{methodName}", requestJson);

            // Ensure the response is successful
            //response.EnsureSuccessStatusCode();

            // Read the response as a dynamic object
            var responseJson = "json";

            return responseJson;
        }
    }
}
