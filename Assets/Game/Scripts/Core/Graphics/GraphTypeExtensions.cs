namespace Assets.Game.Scripts.Core.Graphics
{
    public static class GraphTypeExtensions
    {
        public static string GetDisplayName(this GraphType type)
        {
            return type switch
            {
                GraphType.Trajectory => "Траектория y(x)",
                GraphType.HeightTime => "Высота h(t)",
                GraphType.SpeedTime => "Скорость v(t)",
                GraphType.XTime => "X(t)",
                GraphType.AccelerationTime => "Ускорение a(t)"
            };
        }
    }
}