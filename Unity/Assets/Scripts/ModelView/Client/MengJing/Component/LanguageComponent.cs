using System.Collections.Generic;
using I2.Loc;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class LanguageComponent : Entity, IAwake, IUpdate
    {
        // 多语言插件
        public LanguageSource LanguageSource;
        public LanguageSourceData LanguageSourceData => this.LanguageSource.SourceData;

        public List<string> AllLanguage = new List<string>();

        public bool UseRuntimeModule = false; //模拟平台运行时 编辑器资源不加载

        public string DefaultLanguage;

        public string CurrentLanguage;
    }
}