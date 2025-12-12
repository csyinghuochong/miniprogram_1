using System.Collections.Generic;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;

namespace ET.Server
{
    [EnableMethod]
    [ComponentOf(typeof(CollisionWorldComponent))]
    public class CollisionListenerComponent : Entity, IAwake, IDestroy, IUpdate, IContactListener
    {
        public List<(long, long)> CollisionRecorder = new();

        public List<(long, long)> ToBeRemovedCollisionData = new();

        public void BeginContact(Contact contact)
        {
            //这里获取的是碰撞实体，比如诺克Q技能的碰撞体Unit，这里获取的就是它
            Unit unitA = (Unit)contact.FixtureA.UserData;
            Unit unitB = (Unit)contact.FixtureB.UserData;

            if (unitA == null || unitB == null || unitA.IsDisposed || unitB.IsDisposed)
            {
                return;
            }

            this.CollisionRecorder.Add((unitA.Id, unitB.Id));

            CollisionHandlerDispatcherComponent.Instance.HandleCollisionStart(unitA, unitB);
            CollisionHandlerDispatcherComponent.Instance.HandleCollisionStart(unitB, unitA);
        }

        public void EndContact(Contact contact)
        {
            Unit unitA = (Unit)contact.FixtureA.UserData;
            Unit unitB = (Unit)contact.FixtureB.UserData;

            if (unitA == null || unitB == null || unitA.IsDisposed || unitB.IsDisposed)
            {
                return;
            }
            
            // Id不分顺序，防止移除失败
            this.ToBeRemovedCollisionData.Add((unitA.Id, unitB.Id));
            this.ToBeRemovedCollisionData.Add((unitB.Id, unitA.Id));

            if (unitA.IsDisposed || unitB.IsDisposed)
            {
                return;
            }

            CollisionHandlerDispatcherComponent.Instance.HandleCollisionEnd(unitA, unitB);
            CollisionHandlerDispatcherComponent.Instance.HandleCollisionEnd(unitB, unitA);
        }

        public void PreSolve(Contact contact, in Manifold oldManifold)
        {
        }

        public void PostSolve(Contact contact, in ContactImpulse impulse)
        {
        }
    }
}