using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UICreateRoleComponent : Entity, IAwake
    {
        public float LastLoginTime;
        
        public TMP_InputField InputField_RoleName;
        public Button Button_Create;    
    }
}