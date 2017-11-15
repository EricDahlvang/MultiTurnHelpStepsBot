using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading;

namespace MultiTurnHelpStepsBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.ToUpper().StartsWith("HELP"))
                await context.Forward(new SteppingDialog(), ResumeAfterSteps, activity, CancellationToken.None);
            else
            {
                await context.PostAsync("I don't understand.  Send me text like: Help1, Help2, Help3, Help4.");
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterSteps(IDialogContext context, IAwaitable<object> result)
        {
            Activity reply = ((Activity)context.Activity).CreateReply("Send me text like: Help1, Help2, Help3 and I will walk you through the steps.");
            await context.PostAsync(reply);
        }
    }
}