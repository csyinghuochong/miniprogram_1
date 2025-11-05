using System;
using System.Collections.Generic;

namespace ET.Client
{
    [Code]
    public class BuffDispatcherComponentC : Singleton<BuffDispatcherComponentC>, ISingletonAwake
    {
        private readonly Dictionary<string, BuffHandler> handlers = new();

        public void Awake()
        {
            var types = CodeTypes.Instance.GetTypes(typeof(BuffCHandlerAttribute));
            foreach (Type type in types)
            {
                BuffHandler handler = Activator.CreateInstance(type) as BuffHandler;
                if (handler == null)
                {
                    Log.Error($"not BuffHandler: {type.Name}");
                    continue;
                }

                this.handlers.Add(type.Name, handler);
            }
        }

        public BuffHandler Get(string key)
        {
            this.handlers.TryGetValue(key, out var handler);
            return handler;
        }
    }
}