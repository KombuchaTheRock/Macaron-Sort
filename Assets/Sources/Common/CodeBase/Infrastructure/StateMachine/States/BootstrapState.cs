using System;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private const string BootstrapScene = "Bootstrap";
        private const string GameplayScene = "Gameplay";
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        public void Enter() => 
            _sceneLoader.Load(BootstrapScene, OnLoaded);

        private void OnLoaded() => 
            _sceneLoader.Load(GameplayScene);

        public void Exit()
        {
            throw new NotImplementedException();
        }
    }
}