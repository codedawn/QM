using Castle.DynamicProxy;

namespace Test1
{
    public class ProxyTest
    {
        public async static void Run()
        {
            ProxyGenerator generator = new ProxyGenerator();
            MyService myService = generator.CreateInterfaceProxyWithoutTarget<MyService>(new RpcIntercepter());
            Action action = async () => {

                try
                {
                    await myService.GetData<string>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            };
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //Console.WriteLine(result);  
        }
    }

    public class RpcIntercepter : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            Task task = tcs.Task;
            tcs.SetResult("test");
            invocation.ReturnValue = task;
            throw new NotImplementedException();
        }
    }

    public interface MyService
    {
        public Task<TResult> GetData<TResult>();
    }
}
