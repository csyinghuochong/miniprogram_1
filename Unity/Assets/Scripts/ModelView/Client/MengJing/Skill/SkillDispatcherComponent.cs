using System;
using System.Collections.Generic;

namespace ET.Client
{
    [Code]
    public class SkillDispatcherComponent : Singleton<SkillDispatcherComponent>, ISingletonAwake
    {
        private readonly Dictionary<string, SkillHandler> handlers = new();

        public void Awake()
        {
            var types = CodeTypes.Instance.GetTypes(typeof(SkillHandlerCAttribute));
            foreach (Type type in types)
            {
                SkillHandler handler = Activator.CreateInstance(type) as SkillHandler;
                if (handler == null)
                {
                    Log.Error($"not SkillHandlerC: {type.Name}");
                    continue;
                }

                this.handlers.Add(type.Name, handler);
            }
        }

        public SkillHandler Get(string key)
        {
            this.handlers.TryGetValue(key, out var handler);
            return handler;
        }
    }
}