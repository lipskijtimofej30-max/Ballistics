using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Experiment.Parameter;
using Game.Scripts.Settings;
using SFB;
using Zenject;
using ILogger = Game.Scripts.Infrastructure.Logger.ILogger;

namespace Game.Scripts.Core.Simulation
{
    public class DataExporter
    {
        private readonly ILogger _logger;
        private readonly CsvExporter _csvExporter;
        private readonly OpenRouterService _openRouterService;

        [Inject]
        public DataExporter(ILogger logger, CsvExporter csvExporter, OpenRouterService openRouterService)
        {
            _logger = logger;
            _csvExporter = csvExporter;
            _openRouterService = openRouterService;
        }
        
        public void ExportCsv(IReadOnlyList<SimulationPoint> points, ProjectileState projectile, SimulationSummary summary)
        {
            var extensions = new[] { new ExtensionFilter("CSV файлы", "csv") };
 
            string path = StandaloneFileBrowser.SaveFilePanel(
                "Сохранить результаты симуляции", "", "simulation", extensions);
 
            if (IsCanceledAndFormatPath(ref path, "csv")) return;

            _ = _openRouterService.SendPromptAsync("Зайка. как дела, сладенький мой")
                .ContinueWith((Task<string> t) =>
                {
                    if (t.IsFaulted)
                        _logger.LogError(t.Exception?.ToString() ?? "OpenRouter request failed");
                }, TaskScheduler.Default);
        }
        
        private bool IsCanceledAndFormatPath(ref string path, string extension)
        {
            if (string.IsNullOrEmpty(path))
            {
                _logger.Log("User canceled save");
                return true;
            }

            if (!path.EndsWith($".{extension}", StringComparison.OrdinalIgnoreCase))
                path += $".{extension}";
            return false;
        }

        public void ExportTableExperimentCsv(IReadOnlyList<ExperimentRunResult> results, IExperimentParameter parameter,
            ExperimentPreset preset)
        {
            var extensions = new[] { new ExtensionFilter("CSV файлы", "csv")};

            string path = StandaloneFileBrowser.SaveFilePanel(
                "Сохранить таблицу прогонов", "", "experimentTable", extensions);
            
            if (IsCanceledAndFormatPath(ref path, "csv")) return;
            
            _csvExporter.ExportExperiment(path, results, preset, parameter);
        }
    }
}