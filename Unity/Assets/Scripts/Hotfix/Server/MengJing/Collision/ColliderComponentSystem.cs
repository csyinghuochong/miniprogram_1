using System.Collections.Generic;
using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Unity.Mathematics;

namespace ET.Server
{
    [EntitySystemOf(typeof(ColliderComponent))]
    [FriendOf(typeof(ColliderComponent))]
    public static partial class ColliderComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ColliderComponent self, Unit belongToUnit, ColliderType colliderType)
        {
            self.BelongToUnit = belongToUnit;

            self.ParentUnit = self.GetParent<Unit>();
            CollisionWorldComponent collisionWorldComponent = self.Scene().GetComponent<CollisionWorldComponent>();
            switch (colliderType)
            {
                case ColliderType.Static:
                    self.Body = collisionWorldComponent.CreateStaticBody(new Vector2(self.ParentUnit.Position.x, self.ParentUnit.Position.y));
                    break;
                case ColliderType.Dynamic:
                    self.Body = collisionWorldComponent.CreateDynamicBody(new Vector2(self.ParentUnit.Position.x, self.ParentUnit.Position.y));
                    break;
                case ColliderType.Kinematic:
                    self.Body = collisionWorldComponent.CreateKinematicBody(new Vector2(self.ParentUnit.Position.x, self.ParentUnit.Position.y));
                    break;
            }
        }

        [EntitySystem]
        private static void Update(this ColliderComponent self)
        {
            self.ParentUnit.Position = new float3(self.Body.GetPosition().X, self.Body.GetPosition().Y, 0);
            // self.ParentUnit.Rotation = quaternion.Euler(0, -self.Body.GetAngle(), 0);
        }

        [EntitySystem]
        private static void Destroy(this ColliderComponent self)
        {
            self.Scene().GetComponent<CollisionWorldComponent>()?.AddBodyTobeDestroyed(self.Body);
        }

        /// <summary>
        /// 线性阻尼（影响惯性）
        /// </summary>
        /// <param name="self"></param>
        /// <param name="linearDamping"></param>
        public static void SetLinearDamping(this ColliderComponent self, float linearDamping)
        {
            self.Body.LinearDamping = linearDamping;
        }
        
        /// <summary>
        /// 旋转阻尼
        /// </summary>
        /// <param name="self"></param>
        /// <param name="angularDamping"></param>
        public static void SetAngularDamping(this ColliderComponent self, float angularDamping)
        {
            self.Body.AngularDamping = angularDamping;
        }
        
        /// <summary>
        /// 圆形
        /// </summary>
        /// <param name="self"></param>
        /// <param name="radius">半径</param>
        /// <param name="offset">偏移量</param>
        /// <param name="isSensor">是否为触发器</param>
        /// <param name="layer"></param>
        public static void CreateCircleCollider(this ColliderComponent self, float radius, Vector2 offset, bool isSensor, ushort layer)
        {
            CircleShape m_CircleShape = new();
            m_CircleShape.Radius = radius;
            m_CircleShape.Position = offset;
            FixtureDef fixtureDef = new();
            fixtureDef.IsSensor = isSensor;
            fixtureDef.Shape = m_CircleShape;
            fixtureDef.Density = 1f;
            // fixtureDef.Friction = 0.3f;
            fixtureDef.UserData = self.ParentUnit;
            fixtureDef.Filter = new()
            {
                CategoryBits = layer,
                MaskBits = CollisionHelper.GetMaskBits(layer),
                GroupIndex = 0
            };

            // 禁用刚体旋转
            self.Body.IsFixedRotation = true;
            
            self.Body.CreateFixture(fixtureDef);
        }

        /// <summary>
        /// 矩形
        /// </summary>
        /// <param name="self"></param>
        /// <param name="hx">半宽</param>
        /// <param name="hy">半高</param>
        /// <param name="offset">偏移量</param>
        /// <param name="angle">角度</param>
        /// <param name="isSensor">是否为触发器</param>
        /// <param name="layer"></param>
        public static void CreateBoxCollider(this ColliderComponent self, float hx, float hy, Vector2 offset, float angle, bool isSensor, ushort layer)
        {
            PolygonShape m_BoxShape = new();
            m_BoxShape.SetAsBox(hx, hy, offset, angle);
            FixtureDef fixtureDef = new();
            fixtureDef.IsSensor = isSensor;
            fixtureDef.Shape = m_BoxShape;
            fixtureDef.Density = 1f;
            fixtureDef.Friction = 0.3f;
            fixtureDef.UserData = self.ParentUnit;
            fixtureDef.Filter = new()
            {
                CategoryBits = layer,
                MaskBits = CollisionHelper.GetMaskBits(layer),
                GroupIndex = 0
            };

            self.Body.CreateFixture(fixtureDef);
        }

        /// <summary>
        /// 多边形
        /// </summary>
        /// <param name="self"></param>
        /// <param name="points">顶点数据</param>
        /// <param name="isSensor">是否为触发器</param>
        /// <param name="layer"></param>
        public static void CreatePolygonCollider(this ColliderComponent self, List<Vector2> points, bool isSensor, ushort layer)
        {
            PolygonShape m_PolygonShape = new();
            m_PolygonShape.Set(points.ToArray());
            FixtureDef fixtureDef3 = new();
            fixtureDef3.IsSensor = isSensor;
            fixtureDef3.Shape = m_PolygonShape;
            fixtureDef3.UserData = self.ParentUnit;
            fixtureDef3.Filter = new()
            {
                CategoryBits = layer,
                MaskBits = CollisionHelper.GetMaskBits(layer),
                GroupIndex = 0
            };

            self.Body.CreateFixture(fixtureDef3);
        }

        /// <summary>
        /// 设置位置和旋转
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        /// <param name="angle">弧度(逆时针)</param>
        public static void SetColliderBodyTransform(this ColliderComponent self, Vector2 pos, float angle)
        {
            self.Body.SetTransform(pos, angle);
        }

        public static void SetColliderBodyPos(this ColliderComponent self, Vector2 pos)
        {
            self.Body.SetTransform(pos, self.Body.GetAngle());
        }

        public static void SetColliderBodyAngle(this ColliderComponent self, float angle)
        {
            self.Body.SetTransform(self.Body.GetPosition(), angle);
        }

        public static void SetColliderBodyState(this ColliderComponent self, bool state)
        {
            self.Body.IsEnabled = state;
        }

        /// <summary>
        /// 设置物体的线性速度
        /// </summary>
        /// <param name="self"></param>
        /// <param name="direction">速度方向向量</param>
        /// <param name="speed">速度大小</param>
        public static void SetVelocityInDirection(this ColliderComponent self, Vector2 direction, float speed)
        {
            if (speed == 0 || direction == Vector2.Zero)
            {
                self.Body.SetLinearVelocity(Vector2.Zero);
            }
            else
            {
                Vector2 velocity = Vector2.Normalize(direction) * speed;
                self.Body.SetLinearVelocity(velocity);
            }
        }

        /// <summary>
        /// 给物体施加持续力（需要每帧调用）
        /// </summary>
        /// <param name="self"></param>
        /// <param name="direction">力的方向</param>
        /// <param name="forceMagnitude">力的大小</param>
        /// <param name="isWake">是否唤醒物体</param>
        public static void ApplyForceInDirection(this ColliderComponent self, Vector2 direction, float forceMagnitude, bool isWake = true)
        {
            if (forceMagnitude == 0 || direction == Vector2.Zero)
            {
                self.Body.SetLinearVelocity(Vector2.Zero);
            }
            else
            {
                Vector2 force = Vector2.Normalize(direction) * forceMagnitude;
                self.Body.ApplyForce(force, self.Body.GetWorldCenter(), isWake);
            }
        }

        // /// <summary>
        // /// 血量变化的时候，动态更新角色碰撞框的大小
        // /// </summary>
        // /// <param name="self"></param>
        // /// <param name="radius"></param>
        // public static void SetBodyCircleRadius(this ColliderComponent self, float radius)
        // {
        //     if (self.Body.FixtureList.Count > 0)
        //     {
        //         Shape shape = self.Body.FixtureList[0].Shape;
        //         if (shape is CircleShape circle)
        //         {
        //             circle.Radius = radius;
        //         }
        //     }
        // }
    }
}