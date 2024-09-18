## [QM](https://github.com/codedawn/QM) 一个简单易用的、可拓展的游戏服务器框架
        完全使用c sharp(c#)开发，包括了游戏服务器所需的基本组件。
        QM的架构设计使得伸缩QM伸缩性非常好，很容易进行集群和分布式开发。
        特性：
        1.依托dotnetty线程模型处理消息，性能非常不错
        2.支持async/await编程，避免阻塞线程
        3.Connector服务器根据路由转发消息到Server服务器，非常容易支持集群（参考pomelo）
        4.支持自定义消息协议，自定义消息，自定义事件
        5.代码简单易懂，适合进行二次定制，同时有不错的性能
### 1. 涉及技术
  TCP通信使用dotnetty、消息序列化使用messagepack、rpc二次开发自DotNettyRpc、服务器发现Zookeeper
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
[Wiki文档](https://github.com/codedawn/QM/wiki)

[Demo项目](https://github.com/codedawn/QMDemo)
