using System.Collections.Generic;

namespace Business.Cooperative.ProjectState
{
    public abstract class State
    {
        public abstract List<Projections> Handle(Context context);
    }
}