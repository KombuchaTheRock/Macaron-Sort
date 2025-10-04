using Sources.Common.CodeBase.Services.InputService;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.ResourceLoader;
using Sources.Common.CodeBase.Services.SaveService;
using Sources.Common.CodeBase.Services.Settings;
using Sources.Common.CodeBase.Services.SoundService;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Common.CodeBase.Services.WindowService;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller, ICoroutineRunner
    {
        [SerializeField] private SoundPool _soundPool;

        public override void InstallBindings()
        {
            BindSceneLoader();
            BindResourceLoader();
            BindStaticDataService();
            BindPauseService();
            BindInputService();
            BindGameProgress();
            BindPlayerLevel();
            BindSoundService();
            BindGameSettingsSaveLoader();
            BindGameSettings();
            BindWindowService();
        }

        private void BindPauseService()
        {
            Container.Bind<IPauseService>()
                .To<PauseService>()
                .AsSingle();
        }

        private void BindWindowService()
        {
            Container.Bind<IWindowService>()
                .To<WindowService>()
                .AsSingle();
        }

        private void BindGameSettingsSaveLoader()
        {
            Container.Bind<ISettingsSaveLoader>()
                .To<SettingsSaveLoader>()
                .AsSingle();
        }

        private void BindGameSettings()
        {
            Container.Bind<IGameSettings>()
                .To<GameSettings>()
                .AsSingle();
        }

        private void BindSoundService()
        {
            Container.Bind<ISoundService>()
                .To<SoundService>()
                .AsSingle()
                .WithArguments(_soundPool);
        }

        private void BindPlayerLevel()
        {
            Container.BindInterfacesTo<PlayerLevel>()
                .AsSingle();
        }

        private void BindGameProgress()
        {
            Container.Bind<ISerializer>()
                .To<JsonUtilitySerializer>()
                .AsSingle();

            Container.Bind<ISaveSystem>()
                .To<SaveSystem>()
                .AsSingle();

            Container.Bind<IGameProgressService>()
                .To<GameProgressService>()
                .AsSingle();
        }

        private void BindInputService()
        {
            if (Application.isEditor)
                Container.BindInterfacesTo<StandaloneInput>()
                    .AsSingle();
            else
                Container.BindInterfacesTo<MobileInput>()
                    .AsSingle();
        }


        private void BindSceneLoader()
        {
            Container.Bind<SceneLoader>()
                .AsSingle()
                .WithArguments(this);
        }

        private void BindResourceLoader()
        {
            Container.Bind<IResourceLoader>()
                .To<ResourceLoader>()
                .AsSingle();
        }

        private void BindStaticDataService()
        {
            Container.BindInterfacesTo<StaticDataService>()
                .AsSingle();
        }
    }
}