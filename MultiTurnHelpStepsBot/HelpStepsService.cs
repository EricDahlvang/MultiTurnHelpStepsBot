using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiTurnHelpStepsBot
{
    public static class HelpStepsService
    {
        static Dictionary<string, List<HelpStep>> _helpWithSteps = new Dictionary<string, List<HelpStep>>();
        static HelpStepsService()
        {
            var help1Steps = new List<HelpStep>();
            help1Steps.Add(new HelpStep() { StepText = "H1: Step 1", Description = "This is the first step." });
            help1Steps.Add(new HelpStep() { StepText = "H1: Step 2", Description = "This is the second step." });
            _helpWithSteps.Add("HELP1", help1Steps);


            var help2Steps = new List<HelpStep>();
            help2Steps.Add(new HelpStep() { StepText = "H2: Step 1", Description = "This is the first step." });
            help2Steps.Add(new HelpStep() { StepText = "H2: Step 2", Description = "This is the second step." });
            _helpWithSteps.Add("HELP2", help2Steps);

            var help3Steps = new List<HelpStep>();
            help3Steps.Add(new HelpStep() { StepText = "H3: Step 1", Description = "This is the first step." });
            help3Steps.Add(new HelpStep() { StepText = "H3: Step 2", Description = "This is the second step." });
            help3Steps.Add(new HelpStep() { StepText = "H3: Step 3", Description = "This is the third step." });
            _helpWithSteps.Add("HELP3", help3Steps);

            var help4Steps = new List<HelpStep>();
            help4Steps.Add(new HelpStep() { StepText = "H4: Step 1", Description = "This is the first step." });
            help4Steps.Add(new HelpStep() { StepText = "H4: Step 2", Description = "This is the second step." });
            help4Steps.Add(new HelpStep() { StepText = "H4: Step 3", Description = "This is the third step." });
            help4Steps.Add(new HelpStep() { StepText = "H4: Step 4", Description = "This is the third step." });
            _helpWithSteps.Add("HELP4", help4Steps);
        }

        public static List<HelpStep> GetSteps(string key)
        {
            List<HelpStep> steps = null;
            if(_helpWithSteps.TryGetValue(key.ToUpper(), out steps))
                return steps;
            
            return new List<HelpStep>();
        }
    }

    public class HelpStep
    {
        public string StepText { get; set; }
        public string Description { get; set; }
    }
}