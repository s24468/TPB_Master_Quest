using System.Collections.Generic;
using UnityEngine;
using Cards;

public class RemoveCardFromHandCommand : Command
{
    private CardLogic cardLogic;
    private Player p;

    public RemoveCardFromHandCommand(Player p, CardLogic cardLogic)
    {
        this.cardLogic = cardLogic;
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        HandManager playerHand = p.PArea.handManager;

        GameObject card = Services.Get<IInstanceIdService>().Find(cardLogic.UniqueCardID);

        playerHand.RemoveCard(card);

        GameObject.Destroy(card);
        HoverPreview.PreviewsAllowed = true;


        CommandExecutionComplete();
    }
}