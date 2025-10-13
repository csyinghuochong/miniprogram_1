namespace ET.Client
{
    [EntitySystemOf(typeof(SetUnitTransformComponent))]
    [FriendOf(typeof(SetUnitTransformComponent))]
    public static partial class SetUnitTransformComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SetUnitTransformComponent self)
        {
            self.Unit = self.GetParent<Unit>();
            self.Transform = self.Unit.GetComponent<GameObjectComponent>().GameObject.transform;
        }

        [EntitySystem]
        private static void Update(this SetUnitTransformComponent self)
        {
            self.Unit.Position = self.Transform.position;
            self.Unit.Rotation = self.Transform.rotation;
        }
    }
}