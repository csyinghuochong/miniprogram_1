using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MaskWordComponent : Entity, IAwake, IDestroy
    {
        public string MaskWord;
        public string[] sensitiveWordsArray = null;
        public Dictionary<char, IList<string>> keyDict = new();
    }
}