using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Experiment.Parameter;
using Game.Scripts.Settings;
using UnityEngine;
using Zenject;
using ILogger = Game.Scripts.Infrastructure.Logger.ILogger;

namespace Game.Scripts.Core.Simulation
{
    public class CsvExporter
    {
        private readonly ILogger _logger;
        private readonly MassCalculator _massCalculator;

        [Inject]
        public CsvExporter(ILogger logger, MassCalculator massCalculator)
        {
            _logger = logger;
            _massCalculator = massCalculator;
        }

        public void ExportSimulation(string path, IReadOnlyList<SimulationPoint> points, ProjectileState projectile, SimulationSummary summary)
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

        public void ExportExperiment(string path, IReadOnlyList<ExperimentRunResult> results, ExperimentPreset preset,
            IExperimentParameter parameter)
        {
            try
            {
                var builder = BuildExperimentCsv(preset, results, parameter);
                File.WriteAllText(path, builder.ToString(), Encoding.GetEncoding("windows-1251"));
                _logger.Log($"CSV saved: {path}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to save CSV to {path}: {e.Message}");
            }
        }

        private StringBuilder BuildExperimentCsv(ExperimentPreset preset, IReadOnlyList<ExperimentRunResult> results, IExperimentParameter parameter)
        {
            var builder = new StringBuilder();
            builder.AppendLine("sep=;");
            builder.AppendLine("=== Базовые значения ===");
            builder.AppendLine("Параметр;Значение");
            builder.AppendLine($"Форма; {preset.ShapeType}");
            builder.AppendLine($"Размер (м);\"{preset.Size.ToString(CultureInfo.InvariantCulture)}\"");
            builder.AppendLine($"Плотность (кг/м³);{preset.Density.ToString(CultureInfo.InvariantCulture)} кг/м³");
            builder.AppendLine($"Масса (кг);{_massCalculator.GetMass(preset.ShapeType, preset.Density, preset.Size).ToString("F3", CultureInfo.InvariantCulture)} кг");
            builder.AppendLine($"Начальная скорость (м/с);{preset.InitialSpeed.ToString("F2", CultureInfo.InvariantCulture)} м/с");
            builder.AppendLine($"Угол запуска (град);{preset.LaunchAngle.ToString("F2", CultureInfo.InvariantCulture)} °");
            builder.AppendLine($"Начальная высота (м);{preset.InitialHeight.ToString("F2", CultureInfo.InvariantCulture)} м");
            builder.AppendLine($"Гравитация (м/с^2);{preset.Gravity.y.ToString("F2", CultureInfo.InvariantCulture)} м/с^2");
            builder.AppendLine("Сопротивление ветра не учитывалось");

            builder.AppendLine("");
            
            builder.AppendLine(
                $"№;{parameter.DisplayName},{parameter.Unit};Макс.высота,м;Макс.скорость,м/с;Дистанция,м;Время,с;");
            
            foreach (var result in results)
            {
                builder.AppendLine(
                    $"{result.RunId.ToString(CultureInfo.InvariantCulture)};" +
                    $"{result.ParameterValue.ToString(CultureInfo.InvariantCulture)};" +
                    $"{result.Summary.MaxHeight.ToString(CultureInfo.InvariantCulture)};" +
                    $"{result.Summary.MaxSpeed.ToString(CultureInfo.InvariantCulture)};" +
                    $"{result.Summary.Range.ToString(CultureInfo.InvariantCulture)};" +
                    $"{result.Summary.FlightTime.ToString(CultureInfo.InvariantCulture)};");
            }
            
            return builder;
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