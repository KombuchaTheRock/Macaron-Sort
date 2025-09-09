using System;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        public void Enter() => 
            _sceneLoader.Load("BootstrapScene");

        public void Exit()
        {
            throw new NotImplementedException();
        }
    }
}