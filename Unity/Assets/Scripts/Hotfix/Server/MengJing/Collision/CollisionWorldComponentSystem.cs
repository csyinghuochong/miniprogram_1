using System;
using System.Numerics;
using Box2DSharp.Dynamics;

namespace ET.Server
{
    [EntitySystemOf(typeof(CollisionWorldComponent))]
    [FriendOf(typeof(CollisionWorldComponent))]
    public static partial class CollisionWorldComponentSystem
    {
        [Invoke(TimerInvokeType.CollisionWorldTimer)]
        [FriendOf(typeof(CollisionWorldComponent))]
        public class CollisionWorldTimer : ATimer<CollisionWorldComponent>
        {
            protected override void Run(CollisionWorldComponent self)
            {
                if (self.World == null)
                {
                    return;
                }

                foreach (var body in self.BodyToDestroy)
                {
                    self.World.DestroyBody(body);
                }

                self.BodyToDestroy.Clear();
                try
                {
                    const float DeltaTime = 100 / 1000f;
                    float scaledDeltaTime = DeltaTime * self.Scene().TimeScale;

                    if (scaledDeltaTime > 0)
                    {
                        self.World.Step(scaledDeltaTime, self.VelocityIteration, self.PositionIteration);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        [EntitySystem]
        private static void Awake(this CollisionWorldComponent self)
        {
            self.World = new Box2DSharp.Dynamics.World(new Vector2(0, 0));

            // CollisionListenerComponent collisionListener = self.AddComponent<CollisionListenerComponent>();
            // self.World.SetContactListener(collisionListener);

            self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.CollisionWorldTimer, self);
        }

        [EntitySystem]
        private static void Destroy(this CollisionWorldComponent self)
        {
            self.World.Dispose();
            self.World = null;
            self.BodyToDestroy.Clear();

            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static Body CreateStaticBody(this CollisionWorldComponent self, Vector2 position)
        {
            return self.World.CreateBody(new BodyDef() { BodyType = BodyType.StaticBody, Position = position });
        }

        public static Body CreateDynamicBody(this CollisionWorldComponent self, Vector2 position)
        {
            return self.World.CreateBody(new BodyDef() { BodyType = BodyType.DynamicBody, AllowSleep = false, Position = position });
        }

        public static Body CreateKinematicBody(this CollisionWorldComponent self, Vector2 position)
        {
            return self.World.CreateBody(new BodyDef() { BodyType = BodyType.KinematicBody, AllowSleep = false, Position = position });
        }

        public static void AddBodyTobeDestroyed(this CollisionWorldComponent self, Body body)
        {
            self.BodyToDestroy.Add(body);
        }
    }
}