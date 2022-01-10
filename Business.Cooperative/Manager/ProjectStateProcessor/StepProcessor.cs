using Business.Cooperative.BusinessModel;
using Business.Cooperative.ProjectState.ProjectStep;
using System;
using System.Collections.Generic;
using System.Text;

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
