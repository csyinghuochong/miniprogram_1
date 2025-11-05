using System;
using System.Collections.Generic;

namespace ET.Client
{
    [Code]
    public class BuffDispatcherComponentC : Singleton<BuffDispatcherComponentC>, ISingletonAwake
    {
        private readonly Dictionary<string, BuffCHandler> handlers = new();

        public void Awake()
        {
            var types = CodeTypes.Instance.GetTypes(typeof(BuffCHandlerAttribute));
            foreach (Type type in types)
            {
                BuffCHandler cHandler = Activator.CreateInstance(type) as BuffCHandler;
                if (cHandler == null)
                {
                    Log.Error($"not BuffHandler: {type.Name}");
                    continue;
                }

                this.handlers.Add(type.Name, cHandler);
            }
        }

        public BuffCHandler Get(string key)
        {
            this.handlers.TryGetValue(key, out var handler);
            return handler;
        }
    }
}