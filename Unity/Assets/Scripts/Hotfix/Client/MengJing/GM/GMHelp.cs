namespace ET.Client
{
    public static class GMHelp
    {
        public static void SendGmCommand(Scene scene, string gm)
        {
            C2M_GMCommand request = new() { GMMsg = gm };
            scene.GetComponent<ClientSenderComponent>().Send(request);
        }
    }
}