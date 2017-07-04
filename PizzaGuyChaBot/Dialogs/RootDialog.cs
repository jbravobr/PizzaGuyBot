﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Linq;

namespace PizzaGuyChaBot.Dialogs
{
    [LuisModel("cfc6d9c1-49ea-4474-8d79-eacf5327bfcb", "e373378781f5469d9cd828be194966cc")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        public RootDialog() { }

        [LuisIntent("IncompletedOrder")]
        public async Task IncompletedOrder(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Vamos lá, você quer uma pizza né? Então me diz, qual sabor?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("MissedPaymentForm")]
        public async Task MissedPaymentForm(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Tá bom, parece que você quer pagar com cartão, sería Crédito ou Débito?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Desculpa não entendi direito o que você disse.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Order")]
        public async Task Order(IDialogContext context, LuisResult result)
        {
			await context.PostAsync($"Tá bom, agora preciso saber então qual o sabor da pizza..");
			context.Wait(MessageReceived);
        }

        [LuisIntent("Payment")]
        public async Task Payment(IDialogContext context, LuisResult result)
        {
			await context.PostAsync($"Ótimo, então vamos confirmar o seu pedido!");
			context.Wait(MessageReceived);
        }

        [LuisIntent("PickFlavour")]
		public async Task PickFlavour(IDialogContext context, LuisResult result)
		{
            var sabores = result.Entities.Select(x => x.Entity).Aggregate((f, s) => $"{f},{s}");

            IMessageActivity message = Activity.CreateMessageActivity();
            message.Text = $"Tá bom anotei aqui então os sabores. Agora me diz qual vai ser a forma de pagamento?";
            message.Locale = "pt-br";
            message.Attachments.Add(new Attachment { ContentType = "text/plain", Name = "Sabores", Content = sabores });

            await context.PostAsync(message);
			context.Wait(MessageReceived);
		}
    }
}