using UnityEngine;
using System.Collections;
using Cards;

public class PlayACreatureCommand : Command
{
    private CardLogic cl;
    private int laneIndex;   // NEW: 0..2
    private int tablePos;
    private Player p;
    // NEW ctor: laneIndex added
    public PlayACreatureCommand(CardLogic cl, Player p, int laneIndex, int tablePos)
    {
        this.cl = cl;
        this.p = p;
        this.laneIndex = laneIndex;
        this.tablePos = tablePos;
    }

    public override void StartCommandExecution()
    {
        // remove and destroy the card in hand
        HandVisual playerHand = p.PArea.handVisual;

        GameObject card = Services.Get<IInstanceIdService>().Find(cl.UniqueCardID);

        playerHand.RemoveCard(card);

        GameObject.Destroy(card);

        // enable Hover Previews Back
        HoverPreview.PreviewsAllowed = true;

        TableVisual targetTable = p.PArea.tableVisuals[laneIndex];

        CreatureLogic  newCreatureLogic= targetTable.AddCreatureAtIndex(
            cl,
            tablePos,
            new Vector3(0f, -179f, 0f),
            p
        );

        p.tables[laneIndex].CreaturesOnTable.Insert(tablePos,newCreatureLogic);

    }
    
}
