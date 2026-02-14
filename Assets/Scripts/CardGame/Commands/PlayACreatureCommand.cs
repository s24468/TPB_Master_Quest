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

        // place creature on the correct lane/table visual
        // NOTE: requires PlayerArea.tableVisuals (TableVisual[]) to exist and be filled in Inspector
        TableVisual targetTable = p.PArea.tableVisuals[laneIndex];

        CreatureLogic  newCreatureLogic= targetTable.AddCreatureAtIndex(
            cl,
            tablePos,
            new Vector3(0f, -179f, 0f),
            p
        );

        p.tables[laneIndex].CreaturesOnTable.Insert(tablePos,newCreatureLogic);

    }
    // public PlayACreatureCommand(CardLogic cl, Player p, int tablePos, string creatureID)
    // {
    //     this.p = p;
    //     this.cl = cl;
    //     this.tablePos = tablePos;
    //     this.creatureID = creatureID;
    // }
    //
    // public override void StartCommandExecution()
    // {
    //     // remove and destroy the card in hand 
    //     HandVisual PlayerHand = p.PArea.handVisual;
    //     GameObject card = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
    //     PlayerHand.RemoveCard(card);
    //     GameObject.Destroy(card);
    //     // enable Hover Previews Back
    //     HoverPreview.PreviewsAllowed = true;
    //     // move this card to the spot 
    //     p.PArea.tableVisual.AddCreatureAtIndex(cl.ca, creatureID, tablePos,new Vector3(0f, -179f, 0f));
    // }
}
