using Business.Cooperative.BusinessModel;

namespace Business.Cooperative.ProjectState.ProjectStep
{
    public class StepContext
    {
        private StepState stepState;

        // Constructor
        public StepContext(StepState stepState)
        {
            this.StepState = stepState;
        }

        // Gets or sets the state
        public StepState StepState
        {
            get { return stepState; }
            set
            {
                stepState = value;
            }
        }

        public IManager Request()
        {
            return stepState.Handle(this);
        }
    }
}