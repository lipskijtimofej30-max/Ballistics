namespace Assets.Game.Scripts.Core.Graphics
{
    public static class GraphTypeExtensions
    {
        public static string GetDisplayName(this GraphType type)
        {
            return type switch
            {
                GraphType.Trajectory => "Траектория y(x)",
                GraphType.SpeedTime => "Скорость v(t)",
                GraphType.XTime => "Дальность x(t)",
                GraphType.AccelerationTime => "Ускорение a(t)"
            };
        }
    }
}