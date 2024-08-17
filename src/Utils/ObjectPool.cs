using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    /// <summary>
    /// 线程安全
    /// </summary>
    public class ObjectPool
    {
        public static readonly ObjectPool Instance = new ObjectPool();

        private static int _defaultCapacity = 1000;
        private ConcurrentDictionary<Type, Pool> objPool = new ConcurrentDictionary<Type, Pool>();
        private Func<Type, Pool> poolFunc = type => new Pool(type, _defaultCapacity);

        private ObjectPool() { }

        public T Fetch<T>() where T : IPool
        {
            return (T)Fetch(typeof(T));
        }

        public object Fetch(Type type)
        {
            if(!typeof(IPool).IsAssignableFrom(type))
            {
                throw new ArgumentException("必须继承IPool接口");
            }
            Pool pool = GetPool(type);
            return pool.Get();
        }

        public void Recycle(IPool obj)
        {
            if (obj is null)
            {
                return;
            }

            Type type = obj.GetType();
            Pool pool = GetPool(type);
            pool.Return(obj);
        }

        private Pool GetPool(Type type)
        {
            return objPool.GetOrAdd(type, poolFunc);
        }
    }

    /// <summary>
    /// 线程安全无锁对象池
    /// </summary>
    class Pool
    {
        private readonly Type _type;
        private readonly int _capacity;
        private int _num;
        private readonly ConcurrentQueue<object> _objectQueue = new ConcurrentQueue<object>();
        private object _fastObject;

        public Pool(Type type, int capacity)
        {
            _type = type;
            _capacity = capacity;
        }

        public object Get()
        {
            object obj = _fastObject;
            if (obj == null || Interlocked.CompareExchange(ref _fastObject, null, obj) != obj)
            {
                if (_objectQueue.TryDequeue(out obj))
                {
                    Interlocked.Decrement(ref _num);
                    return obj;
                }
                return Activator.CreateInstance(_type);
            }
            return obj;
        }

        public void Return(object obj)
        {
            if(_fastObject != null || Interlocked.CompareExchange(ref _fastObject, obj, null) != null)
            {
                if (Interlocked.Increment(ref _num) <= _capacity)
                {
                    _objectQueue.Enqueue(obj);
                    return;
                }
                Interlocked.Decrement(ref _num);
            }
        }
    }
}
