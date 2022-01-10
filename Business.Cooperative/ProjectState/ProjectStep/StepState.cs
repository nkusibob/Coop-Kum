using Business.Cooperative.BusinessModel;
using System.Collections.Generic;

namespace Business.Cooperative.ProjectState.ProjectStep
{
    public abstract class StepState
    {
        public abstract IManager Handle(StepContext stepContext);

    }
}