using Business.Cooperative.Interfaces;
using Business.Cooperative.ProjectState.ProjectStep;

namespace Business.Cooperative.Manager.ProjectStateProcessor
{
    public class StepProcessor
    {
        private IManager _manager;

        public StepProcessor(IManager manager)
        {
            this._manager = manager;
        }

        public IManager StartProject()
        {
            var context = new StepContext(new ProjectStarted(_manager));
            _manager = context.Request();
            return _manager;
        }
    }
}