using Castle.DynamicProxy;

namespace Test1
{
    public class ProxyTest
    {
        public async static void Run()
        {
            ProxyGenerator generator = new ProxyGenerator();
            MyService myService = generator.CreateInterfaceProxyWithoutTarget<MyService>(new RpcIntercepter());
            string result = await myService.GetData<string>();
            Console.WriteLine(result);  
        }
    }

    public class RpcIntercepter : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            Task task = tcs.Task;
            tcs.SetResult("test");
            invocation.ReturnValue = task;
        }
    }

    public interface MyService
    {
        public Task<TResult> GetData<TResult>();
    }
}
