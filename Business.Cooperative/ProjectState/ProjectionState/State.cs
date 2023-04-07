using System.Collections.Generic;

namespace Business.Cooperative.ProjectState
{
    public abstract class State
    {
        public abstract List<Projection> Handle(Context context);
    }
}