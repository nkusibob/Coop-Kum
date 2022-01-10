using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Cooperative.ProjectState
{
    public class Context
    {
        State state;
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
        public List<Projections> Request()
        {
            return state.Handle(this);
        }
        
    }
}
