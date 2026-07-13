using Assets.Game.Scripts.UX;
using Assets.Game.Scripts.View;
using Assets.Game.Scripts.View.UseCase;
using Game.Scripts.UX;
using Game.Scripts.View;
using Game.Scripts.View.UseCase;
using Game.Scripts.View.View;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Installer
{
    public class ViewInstaller : MonoInstaller
    {
        [SerializeField] private ProjectileView projectileView;
        [SerializeField] private SimulationView simulationView;
        [SerializeField] private EnvironmentView environmentView;
        [SerializeField] private SetupPanelView setupPanelView;
        [SerializeField] private TelemetryPanelView telemetryPanel;
        [SerializeField] private ResultsPanelView resultsPanel;
        [SerializeField] private ToolbarView toolbarView;
        [SerializeField] private VisualizationView visualizationView;
        [SerializeField] private ExperimentSettingsView experimentSettingsView;
        [SerializeField] private IntegratorView integratorView;
        [SerializeField] private ModeControllerView modeControllerView;
        [SerializeField] private ExperimentTableView experimentTableView;
        [SerializeField] private ParameterCanvasInteractable parameterCanvasInteractable;
        [SerializeField] private GraphView graphView;
        [Header("Trajectory Renderer")]
        [SerializeField] private TrajectoryRenderer _previewTrajectoryRenderer;
        [SerializeField] private TrajectoryRenderer _liveTrajectoryRenderer;
        override public void InstallBindings()
        {
            Container.Bind<ParameterCanvasInteractable>().FromInstance(parameterCanvasInteractable).AsSingle();
            BindGraphView();
            BindProjectileView();
            BindSimulationView();
            BindEnvironmentView();
            BindIntegratorView();
            Container.BindInterfacesAndSelfTo<SetupDirtyTracker>().AsSingle();
            Container.Bind<ExperimentTableView>().FromInstance(experimentTableView).AsSingle();
            BindToolbarView();
            Container.Bind<ModeControllerView>().FromInstance(modeControllerView).AsSingle();
            Container.Bind<SetupPanelView>().FromInstance(setupPanelView).AsSingle();
            Container.Bind<TelemetryPanelView>().FromInstance(telemetryPanel).AsSingle();
            Container.Bind<ResultsPanelView>().FromInstance(resultsPanel).AsSingle();
            BindExperimentView();
            BindTrajectoryRenderer();
            BindVisualization();
        }

        private void BindGraphView()
        {
            Container.Bind<GraphView>().FromInstance(graphView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GraphUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<OverflowGraphUX>().AsSingle();
        }

        private void BindExperimentView()
        {
            Container.Bind<ExperimentSettingsView>().FromInstance(experimentSettingsView).AsSingle();
            Container.BindInterfacesAndSelfTo<ExperimentSettingsUseCase>().AsSingle().NonLazy();
        }

        private void BindIntegratorView()
        {
            Container.Bind<IntegratorView>().FromInstance(integratorView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<IntegratorUseCase>().AsSingle().NonLazy();
        }

        private void BindVisualization()
        {
            Container.Bind<VisualizationView>().FromInstance(visualizationView).AsSingle();
            Container.BindInterfacesAndSelfTo<VisualizationUseCase>().AsSingle().NonLazy();
        }

        private void BindTrajectoryRenderer()
        {
            Container.Bind<TrajectoryRenderer>().WithId("Live").FromInstance(_liveTrajectoryRenderer).AsCached();
            Container.Bind<TrajectoryRenderer>().WithId("Preview").FromInstance(_previewTrajectoryRenderer).AsCached();
            Container.BindInterfacesAndSelfTo<TrajectoryPreviewUseCase>().AsSingle();
        }

        private void BindToolbarView()
        {
            Container.Bind<ToolbarView>().FromInstance(toolbarView).AsSingle();
        }

        private void BindEnvironmentView()
        {
            Container.Bind<EnvironmentView>().FromInstance(environmentView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EnvironmentUseCase>().AsSingle().NonLazy();
        }

        private void BindProjectileView()
        {
            Container.Bind<ProjectileView>().FromInstance(projectileView).AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileUseCase>().AsSingle();
        }

        private void BindSimulationView()
        {
            Container.Bind<SimulationView>().FromInstance(simulationView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SimulationUseCase>().AsSingle().NonLazy();
        }
    }
}