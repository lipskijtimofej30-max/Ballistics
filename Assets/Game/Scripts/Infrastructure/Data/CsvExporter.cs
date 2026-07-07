using System;
using Game.Scripts.Infrastructure.Logger;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Zenject;

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

        public void Export(string path, IReadOnlyList<SimulationPoint> points)
        {
            try
            {
                var builder = BuildCsvContent(points);
                File.WriteAllText(path, builder.ToString(), new UTF8Encoding(true));
                _logger.Log($"CSV saved: {path}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to save CSV to {path}: {e.Message}");   
            }
        }

        private StringBuilder BuildCsvContent(IReadOnlyList<SimulationPoint> points)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("sep=;");

            builder.AppendLine(
                "Time;PosX;PosY;PosZ;" +
                "VelX;VelY;VelZ;" +
                "AccX;AccY;AccZ;" +
                "ForceX;ForceY;ForceZ");

            foreach (var point in points)
            {
                builder.AppendLine(
                    $"{point.Time.ToString(CultureInfo.InvariantCulture)};" +

                    $"{point.Position.x.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.Position.y.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.Position.z.ToString(CultureInfo.InvariantCulture)};" +

                    $"{point.Velocity.x.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.Velocity.y.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.Velocity.z.ToString(CultureInfo.InvariantCulture)};" +

                    $"{point.Acceleration.x.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.Acceleration.y.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.Acceleration.z.ToString(CultureInfo.InvariantCulture)};" +

                    $"{point.TotalForce.x.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.TotalForce.y.ToString(CultureInfo.InvariantCulture)};" +
                    $"{point.TotalForce.z.ToString(CultureInfo.InvariantCulture)}");
            }

            return builder;
        }
    }
}