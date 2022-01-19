using Business.Cooperative.BusinessModel;

namespace Business.Cooperative.ProjectState.ProjectStep
{
    public abstract class StepState
    {
        public abstract IManager Handle(StepContext stepContext);
    }
}