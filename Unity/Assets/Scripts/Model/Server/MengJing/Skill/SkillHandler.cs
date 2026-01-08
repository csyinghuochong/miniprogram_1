using Unity.Mathematics;

namespace ET.Server
{
    public interface Shape
    {
        public float3 s_position { get; set; } //检测体坐标
        bool Contains(float3 t_point);
    }

    [EnableClass]
    public class Circle : Shape
    {
        public float3 s_position { get; set; } //检测体坐标
        public float range { get; set; } //检测范围

        public bool Contains(float3 t_point)
        {
            return math.distance(s_position, t_point) <= this.range;
        }
    }

    [EnableClass]
    public class Rectangle : Shape
    {
        public float3 s_position { get; set; } //检测体坐标
        public float3 s_forward { get; set; } //检测体朝向

        public float x_range { get; set; } //检测X范围[左右的方向]
        public float y_range { get; set; } //检测Y范围[向前的方向]

        public bool Contains(float3 t_point)
        {
            float3 direction = t_point - s_position;
            if (MathHelper.Equal(direction, float3.zero))
            {
                return true;
            }

            //与玩家正前方做点积  或者一条向量在另外一条向量上的投影
            //点乘结果 如果大于0表示目标在攻击者前方 90度=0 <90度 >0 >90度 <0
            float dot = math.dot(direction, s_forward);
            //取绝对值与矩形宽度的一半进行比较
            if (dot <= 0 || dot > this.y_range)
            {
                return false;
            }

            float3 newVec = math.mul(quaternion.Euler(0f, math.radians(90f), 0f), s_forward);
            float rightDistance = math.dot(direction, newVec);

            return math.abs(rightDistance) <= x_range;
        }
    }

    [EnableClass]
    public class Fan : Shape
    {
        public float3 s_position { get; set; } //检测体坐标

        public quaternion s_rotation { get; set; } //检测体坐标

        public float skill_distance { get; set; } //检测体坐标

        public float skill_angle { get; set; } //检测体坐标

        //矩形计算
        public bool Contains(float3 t_point)
        {
            float distance = math.distance(s_position, t_point); //距离
            float3 norVec = math.mul(s_rotation, math.forward());
            float3 temVec = t_point - s_position;
            float dot_len = math.dot((norVec).normalize(), (temVec).normalize());

            float jiajiao = math.degrees(math.acos(dot_len)); //计算两个向量间的夹角

            return distance < skill_distance && jiajiao <= skill_angle;
        }
    }

    public class SkillHandlerAttribute : BaseAttribute
    {
    }

    [EnableClass]
    [SkillHandler]
    public abstract class SkillHandler
    {
        public abstract void OnInit(Skill skill);
        public abstract void OnExecute(Skill skill);
        public abstract void OnUpdate(Skill skill, float deltaTime);
        public abstract void OnFinished(Skill skill);
    }
}