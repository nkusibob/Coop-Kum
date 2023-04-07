using System.Collections.Generic;

namespace Business.Cooperative.ProjectState
{
    public class Context
    {
        private State state;

        // Constructor
        public Context(State state)
        {
            this.State = state;
        }

        // Gets or sets the state
        public State State
        {
            get { return state; }
            set
            {
                state = value;
            }
        }

        public List<Projection> Request()
        {
            return state.Handle(this);
        }
    }
}