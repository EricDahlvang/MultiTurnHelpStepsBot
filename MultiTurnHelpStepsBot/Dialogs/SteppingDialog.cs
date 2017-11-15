using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading;
using System.Collections.Generic;

namespace MultiTurnHelpStepsBot.Dialogs
{
    [Serializable]
    public class SteppingDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var text = activity.Text.ToUpper();
            if (text.StartsWith("HELP"))
            {
                var foundSteps = HelpStepsService.GetSteps(text);
                if (foundSteps.Count > 0)
                {
                    context.ConversationData.SetValue<string>("Help", activity.Text.ToUpper());
                    context.ConversationData.SetValue<int>("Step", 1);

                    await SendStepMessage(context, activity, foundSteps, 1);
                }
                else
                {
                    await SendIdk(context, activity);
                }
                context.Wait(MessageReceivedAsync);
            }
            else if (text == "NEXT")
            {
                var help = context.ConversationData.GetValue<string>("Help");
                var step = context.ConversationData.GetValue<int>("Step");
                var foundSteps = HelpStepsService.GetSteps(help);
                step++;
                if (step > foundSteps.Count)
                {
                    Activity reply = activity.CreateReply("The Previous step was the last one in this help. All done. (back to the beginning)");
                    await context.PostAsync(reply);
                    context.Done(true);
                }
                else
                {
                    await SendStepMessage(context, activity, foundSteps, step);
                    context.Wait(MessageReceivedAsync);
                }
            }
            else if (text == "BACK")
            {
                var help = context.ConversationData.GetValue<string>("Help");
                var step = context.ConversationData.GetValue<int>("Step");
                var foundSteps = HelpStepsService.GetSteps(help);
                step--;
                if (step < 1)
                {
                    Activity reply = activity.CreateReply("Sorry, there is no previous step.");
                    await context.PostAsync(reply);
                }
                else
                {
                    await SendStepMessage(context, activity, foundSteps, step);
                }
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                await SendIdk(context, activity);
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task SendStepMessage(IDialogContext context, Activity activity, List<HelpStep> foundSteps, int step)
        {
            context.ConversationData.SetValue<int>("Step", step);

            Activity reply = activity.CreateReply(foundSteps[step-1].StepText + " of " + foundSteps.Count);
            reply.AddHeroCard(foundSteps[step - 1].Description, new[] { "Next", "Back" }, new[] { foundSteps.Count == step ? "Done" : "Next", "Back" });
            await context.PostAsync(reply);
        }

        private async Task SendIdk(IDialogContext context, Activity activity)
        {
            Activity reply = activity.CreateReply("I did not understand.  Please send me text like: Help1, Help2, Help3.  Or, if walking through steps, Next or Back");
            await context.PostAsync(reply);
        }
    }
}