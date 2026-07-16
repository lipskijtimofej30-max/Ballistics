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
                var utf8WithBom = new UTF8Encoding(true);
                File.WriteAllText(path, builder.ToString(), utf8WithBom);
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
                var utf8WithBom = new UTF8Encoding(true);
                File.WriteAllText(path, builder.ToString(), utf8WithBom);
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
            var ruCulture = CultureInfo.GetCultureInfo("ru-RU");

            //builder.AppendLine("sep=;");
            builder.AppendLine("=== Базовые значения ===");
            builder.AppendLine("Параметр;Значение");
            builder.AppendLine($"Форма; {preset.ShapeType}");
            builder.AppendLine($"Размер (м);\"{preset.Size.ToString("F3", ruCulture)}\"");
            builder.AppendLine($"Плотность (кг/м³);{preset.Density.ToString("F3", ruCulture)} кг/м^3");
            builder.AppendLine($"Масса (кг);{_massCalculator.GetMass(preset.ShapeType, preset.Density, preset.Size).ToString("F3", ruCulture)} кг");
            builder.AppendLine($"Начальная скорость (м/с);{preset.InitialSpeed.ToString("F3", ruCulture)} м/с");
            builder.AppendLine($"Угол запуска (град);{preset.LaunchAngle.ToString("F3", ruCulture)} °");
            builder.AppendLine($"Начальная высота (м);{preset.InitialHeight.ToString("F3", ruCulture)} м");
            builder.AppendLine($"Гравитация (м/с^2);{preset.Gravity.y.ToString("F3", ruCulture)} м/с^2");
            builder.AppendLine("Сопротивление ветра не учитывалось");

            builder.AppendLine("");
            
            builder.AppendLine(
                $"№;{parameter.DisplayName},{parameter.Unit};Макс.высота,м;Макс.скорость,м/с;Дистанция,м;Время,с;");
            
            foreach (var result in results)
            {
                builder.AppendLine(
                    $"{result.RunId.ToString("F3", ruCulture)};" +
                    $"{result.ParameterValue.ToString("F3", ruCulture)};" +
                    $"{result.Summary.MaxHeight.ToString("F3", ruCulture)};" +
                    $"{result.Summary.MaxSpeed.ToString("F3", ruCulture)};" +
                    $"{result.Summary.Range.ToString("F3", ruCulture)};" +
                    $"{result.Summary.FlightTime.ToString("F3", ruCulture)};");
            }
            
            return builder;
        }

        private StringBuilder BuildCsvContent(IReadOnlyList<SimulationPoint> points, ProjectileState projectile,
            SimulationSummary summary)
        {
            var builder = new StringBuilder();
            var ruCulture = CultureInfo.GetCultureInfo("ru-RU");
            //builder.AppendLine("sep=;");

            // --- Параметры снаряда (заморожены на момент запуска, не live) ---
            builder.AppendLine("=== Параметры снаряда ===");
            builder.AppendLine("Параметр;Значение");
            builder.AppendLine($"Форма;{projectile.ShapeType}");
            builder.AppendLine($"Размер (м);\"{projectile.Size.ToString("F3", ruCulture)}\"");
            builder.AppendLine($"Плотность (кг/м³);{projectile.Density.ToString("F3", ruCulture)} кг/м³");
            builder.AppendLine($"Масса (кг);{projectile.Mass.ToString("F3", ruCulture)} кг");
            builder.AppendLine();

            // --- Начальные условия — берём из ПЕРВОЙ точки, а не из настроек ---
            if (points.Count > 0)
            {
                var initial = points[0];
                float initialSpeed = initial.Velocity.magnitude;
                float initialAngle = Mathf.Atan2(initial.Velocity.y, initial.Velocity.x) * Mathf.Rad2Deg;

                builder.AppendLine("=== Начальные условия (по факту, на старте прогона) ===");
                builder.AppendLine("Параметр;Значение");
                builder.AppendLine($"Начальная скорость (м/с);{initialSpeed.ToString("F3", ruCulture)} м/с");
                builder.AppendLine($"Угол запуска (град);{initialAngle.ToString("F3", ruCulture)} °");
                builder.AppendLine($"Начальная высота (м);{initial.Position.y.ToString("F3", ruCulture)} м");
                builder.AppendLine();
            }

            // --- Максимумы — чистая производная от точек (SimulationAnalyzer) ---
            builder.AppendLine("=== Результаты полёта ===");
            builder.AppendLine($"Макс. высота (м);{summary.MaxHeight.ToString("F3", ruCulture)} м");
            builder.AppendLine($"Макс. скорость (м/с);{summary.MaxSpeed.ToString("F3", ruCulture)} м/с");
            builder.AppendLine($"Дальность (м);{summary.Range.ToString("F3", ruCulture)} м");
            builder.AppendLine($"Время полёта (с);{summary.FlightTime.ToString("F3", ruCulture)} с");
            builder.AppendLine();

            builder.AppendLine(
                "Time;PosX;PosY;" +
                "VelX;VelY;" +
                "AccX;AccY;" +
                "ForceX;ForceY;");

            foreach (var point in points)
            {
                builder.AppendLine(
                    $"{point.Time.ToString("F3", ruCulture)};" +
                    $"{point.Position.x.ToString("F3", ruCulture)};" +
                    $"{point.Position.y.ToString("F3", ruCulture)};" +
                    $"{point.Velocity.x.ToString("F3", ruCulture)};" +
                    $"{point.Velocity.y.ToString("F3", ruCulture)};" +
                    $"{point.Acceleration.x.ToString("F3", ruCulture)};" +
                    $"{point.Acceleration.y.ToString("F3", ruCulture)};" +
                    $"{point.TotalForce.x.ToString("F3", ruCulture)};" +
                    $"{point.TotalForce.y.ToString("F3", ruCulture)};");
            }

            return builder;
        }
    }
}