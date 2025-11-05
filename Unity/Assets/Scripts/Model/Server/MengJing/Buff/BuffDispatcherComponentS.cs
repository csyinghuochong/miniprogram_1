using System;
using System.Collections.Generic;

namespace ET.Server
{
    [Code]
    public class BuffDispatcherComponentS : Singleton<BuffDispatcherComponentS>, ISingletonAwake
    {
        private readonly Dictionary<string, BuffSHandler> handlers = new();

        public void Awake()
        {
            var types = CodeTypes.Instance.GetTypes(typeof(BuffSHandlerAttribute));
            foreach (Type type in types)
            {
                BuffSHandler handler = Activator.CreateInstance(type) as BuffSHandler;
                if (handler == null)
                {
                    Log.Error($"not BuffHandler: {type.Name}");
                    continue;
                }

                this.handlers.Add(type.Name, handler);
            }
        }

        public BuffSHandler Get(string key)
        {
            this.handlers.TryGetValue(key, out var handler);
            return handler;
        }
    }
}