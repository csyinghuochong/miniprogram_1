using System;
using System.Collections.Generic;

namespace ET.Server
{
    [Code]
    public class SkillDispatcherComponentS : Singleton<SkillDispatcherComponentS>, ISingletonAwake
    {
        private readonly Dictionary<string, SkillHandlerS> handlers = new();

        public void Awake()
        {
            var types = CodeTypes.Instance.GetTypes(typeof(SkillHandlerSAttribute));
            foreach (Type type in types)
            {
                SkillHandlerS handlerS = Activator.CreateInstance(type) as SkillHandlerS;
                if (handlerS == null)
                {
                    Log.Error($"not SkillHandler: {type.Name}");
                    continue;
                }

                this.handlers.Add(type.Name, handlerS);
            }
        }

        public SkillHandlerS Get(string key)
        {
            this.handlers.TryGetValue(key, out var handler);
            return handler;
        }
    }
}