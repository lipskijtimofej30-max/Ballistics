using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Assets.Game.Scripts.Settings;
using UnityEngine;
using Zenject;
using ILogger = Game.Scripts.Infrastructure.Logger.ILogger;

namespace Game.Scripts.Core.Simulation
{
    public class CsvExporter
    {
        private readonly ILogger _logger;

        [Inject]
        public CsvExporter(ILogger logger)
        {
            _logger = logger;
        }

        public void Export(string path, IReadOnlyList<SimulationPoint> points, ProjectileState projectile, SimulationSummary summary)
        {
            try
            {
                var builder = BuildCsvContent(points, projectile, summary);
                File.WriteAllText(path, builder.ToString(), Encoding.GetEncoding("windows-1251"));
                _logger.Log($"CSV saved: {path}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to save CSV to {path}: {e.Message}");
            }
        }

        private StringBuilder BuildCsvContent(IReadOnlyList<SimulationPoint> points, ProjectileState projectile,
            SimulationSummary summary)
        {
            var builder = new StringBuilder();
            builder.AppendLine("sep=;");

            // --- Параметры снаряда (заморожены на момент запуска, не live) ---
            builder.AppendLine("=== Параметры снаряда ===");
            builder.AppendLine("Параметр;Значение");
            builder.AppendLine($"Форма;{projectile.ShapeType}");
            builder.AppendLine($"Размер (м);\"{projectile.Size.ToString(CultureInfo.InvariantCulture)}\"");
            builder.AppendLine($"Плотность (кг/м³);{projectile.Density.ToString(CultureInfo.InvariantCulture)} кг/м³");
            builder.AppendLine($"Масса (кг);{projectile.Mass.ToString("F3", CultureInfo.InvariantCulture)} кг");
            builder.AppendLine();

            // --- Начальные условия — берём из ПЕРВОЙ точки, а не из настроек ---
            if (points.Count > 0)
            {
                var initial = points[0];
                float initialSpeed = initial.Velocity.magnitude;
                float initialAngle = Mathf.Atan2(initial.Velocity.y, initial.Velocity.x) * Mathf.Rad2Deg;

                builder.AppendLine("=== Начальные условия (по факту, на старте прогона) ===");
                builder.AppendLine("Параметр;Значение");
                builder.AppendLine($"Начальная скорость (м/с);{initialSpeed.ToString("F2", CultureInfo.InvariantCulture)} м/с");
                builder.AppendLine($"Угол запуска (град);{initialAngle.ToString("F2", CultureInfo.InvariantCulture)} °");
                builder.AppendLine($"Начальная высота (м);{initial.Position.y.ToString("F2", CultureInfo.InvariantCulture)} м");
                builder.AppendLine();
            }

            // --- Максимумы — чистая производная от точек (SimulationAnalyzer) ---
            builder.AppendLine("=== Результаты полёта ===");
            builder.AppendLine($"Макс. высота (м);{summary.MaxHeight.ToString("F3", CultureInfo.InvariantCulture)} м");
            builder.AppendLine($"Макс. скорость (м/с);{summary.MaxSpeed.ToString("F3", CultureInfo.InvariantCulture)} м/с");
            builder.AppendLine($"Дальность (м);{summary.Range.ToString("F3", CultureInfo.InvariantCulture)} м");
            builder.AppendLine($"Время полёта (с);{summary.FlightTime.ToString("F3", CultureInfo.InvariantCulture)} с");
            builder.AppendLine();

            builder.AppendLine(
                "Time;PosX;PosY;" +
                "VelX;VelY;" +
                "AccX;AccY;" +
                "ForceX;ForceY;");

            foreach (var point in points)
            {
                builder.AppendLine(
                    $"{point.Time.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.Position.x.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.Position.y.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.Velocity.x.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.Velocity.y.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.Acceleration.x.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.Acceleration.y.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.TotalForce.x.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.TotalForce.y.ToString(CultureInfo.InvariantCulture)};");
            }

            return builder;
        }
    }
}