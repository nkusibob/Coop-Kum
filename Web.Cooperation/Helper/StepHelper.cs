namespace Web.Cooperation.Helper
{
    public static class StepHelper
    {
        public static string GetStepReviewedClass(bool? reviewed)
        {
            if (reviewed.HasValue)
            {
                if (reviewed.Value)
                {
                    return "reviewed";
                }
                else
                {
                    return "not-reviewed";
                }
            }
            else
            {
                return "pending-review";
            }
        }
    }

}
