using System;
using System.Collections.Generic;
using Game.Scripts.Infrastructure.Logger;
using SFB;
using Zenject;

namespace Game.Scripts.Core.Simulation
{
    public class SimulationExporter
    {
        private readonly ILogger _logger;
        private readonly CsvExporter _csvExporter;

        [Inject]
        public SimulationExporter(ILogger logger, CsvExporter csvExporter)
        {
            _logger = logger;
            _csvExporter = csvExporter;
        }
        
        public void ExportCsv(IReadOnlyList<SimulationPoint> points, ProjectileState projectile, SimulationSummary summary)
        {
            var extensions = new[] { new ExtensionFilter("CSV файлы", "csv") };
 
            string path = StandaloneFileBrowser.SaveFilePanel(
                "Сохранить результаты симуляции", "", "simulation", extensions);
 
            if (string.IsNullOrEmpty(path))
            {
                _logger.Log("User canceled save");
                return;
            }
 
            if (!path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                path += ".csv";
 
            _csvExporter.Export(path, points, projectile, summary);
        }
    }
}