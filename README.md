## QM 一个简单易用的、可拓展的游戏服务器框架
        完全使用c sharp开发，包括了游戏服务器所需的基本组件。
        QM的架构设计使得伸缩QM伸缩性非常好，很容易进行集群和分布式开发。
        特性：
        1.使用线程池并发处理消息，性能非常不错
        2.支持async/await编程，避免阻塞线程
        3.Connector服务器根据路由转发消息到Server服务器（参考pomelo）
        4.支持自定义消息协议
        5.代码简单易懂，适合进行二次定制，同时有不错的性能
### 1. 涉及技术
     通信使用dotnetty、消息序列化使用messagepack、rpc二次开发自DotNettyRpc、服务器发现Zookeeper
### 2. 支持四种消息类型的消息
分别是request，response，notify和push，客户端发起request到服务器端，服务器端处理后会给其返回响应response;notify是客户端发给服务端的通知，也就是不需要服务端给予回复的请求;push是服务端主动给客户端推送消息的类型。
### 3. 开始使用
  主要解释Component、自定义消息、事件系统EventSystem，其中可以通过自定义Component实现不同业务，不同服务器装载不同Component，使得不同服务器的开发变得异常简单。
  #### 1. 启动服务器

```csharp 
Application application = Application.CreateApplication("Room01", Application.Server, 9999);
application.AddComponent(new RoomComp());
application.Start();
```
  ### 2. 如何实现自定义组件
  需要继承Compoment，Component有三个生命周期：start，afterStart，stop
```csharp 
public class RoomComp : Component
```
  ### 3. 如何自定义消息
  [MessageDispatch]会将消息转发到指定的服务器
  [MessageIndex]指定消息编号，需要唯一，编码时使用
  [MessagePackObject]是messagepack提供的
  继承自IRequest代表消息类型是request，同理IResponse，INotify和IPush

```csharp 
[MessageDispatch(ServerType.Server)]
[MessageIndex(1)]
[MessagePackObject]
public class UserRequest : IRequest
```
### 4. 如何自定义消息处理器
   使用 [MessageHandler]标记为消息处理器，继承MessageHandler实现Run方法实现业务逻辑
```csharp 
 [MessageHandler]
 public class UserMessageHandler : MessageHandler<UserRequest, UserResponse>
 {
     protected async override Task Run(UserRequest request, UserResponse response, ISession session)
     {
         response.Name = request.Name;
     }
 }
```
### 5. 如何使用EventSystem
  继承IEvent自定义Event
```csharp 
public class UserAddEvent : IEvent
{
    public RemoteSession Session { get; set; }
}
```
  使用[EventHandler]标记为事件处理器，继承EventHandler重写Run方法实现具体业务
```csharp 
 [EventHandler]
 public class UserAddEventHandler : EventHandler<UserAddEvent>
 {
     public override async Task Run(UserAddEvent e)
     {
         UserPush userPush = new UserPush() { Name = "push" };
         await Application.current.GetComponent<RpcComp>().PushToConnector(userPush, e.Session.serverId, e.Session.Sid);

         UserPush userPush1 = new UserPush() { Name = "broadcast" };
         await Application.current.GetComponent<RpcComp>().Broadcast(userPush1);
     }
 }
```
  发送一个事件
```csharp
await EventSystem.Instance.Publish(new UserAddEvent() { Session = remoteSession });
```