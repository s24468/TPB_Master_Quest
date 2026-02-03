using UnityEngine;
using System.Collections;
using Cards;

public class DragCreatureAttack : DraggingActions
{
    // reference to the sprite with a round "Target" graphic
    private SpriteRenderer sr;

    // LineRenderer that is attached to a child game object to draw the arrow
    private LineRenderer lr;

    // reference to WhereIsTheCardOrCreature to track this object`s state in the game
    private WhereIsTheCardOrCreature whereIsThisCreature;

    // the pointy end of the arrow, should be called "Triangle" in the Hierarchy
    private Transform triangle;

    // SpriteRenderer of triangle. We need this to disable the pointy end if the target is too close.
    private SpriteRenderer triangleSR;

    // when we stop dragging, the gameObject that we were targeting will be stored in this variable.
    private GameObject Target;

    // Reference to creature manager, attached to the parent game object
    private OneCreatureManager manager;

    void Awake()
    {
        // establish all the connections
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();

        manager = GetComponentInParent<OneCreatureManager>();
        whereIsThisCreature = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    public override bool CanDrag
    {
        get
        {
            // we can drag this card if 
            // a) we can control this our player (this is checked in base.canDrag)
            // b) creature "CanAttackNow" - this info comes from logic part of our code into each creature`s manager script
            return base.CanDrag && manager.CanAttackNow;
        }
    }

    public override void OnStartDrag()
    {
        whereIsThisCreature.VisualState = VisualStates.Dragging;
        // enable target graphic
        sr.enabled = true;
        // enable line renderer to start drawing the line.
        lr.enabled = true;
    }

    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction * 2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            // draw a line between the creature and the target
            lr.SetPositions(new Vector3[] { transform.parent.position, transform.position - direction * 2.3f });
            lr.enabled = true;

            // position the end of the arrow between near the target.
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f * direction;

            // proper rotarion of arrow end
            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            // if the target is not far enough from creat
            // re, do not show the arrow
            lr.enabled = false;
            triangleSR.enabled = false;
        }
    }

    // public override void OnEndDrag()
    // {
    //     Debug.Log("End dragging");
    //     
    //     
    //     
    //     Target = null;
    //     RaycastHit[] hits; // TODO: raycast here anyway, store the results in
    //     hits = Physics.RaycastAll(origin: Camera.main.transform.position,
    //         direction: (-Camera.main.transform.position + this.transform.position).normalized, maxDistance: 30f);
    //     foreach (RaycastHit h in hits)
    //     {
    //         // Debug.Log("Cokolwiek uderzone");
    //
    //         // if ((h.transform.tag == "TopPlayer" && this.tag == "LowCreature") || // (h.transform.tag == "LowPlayer" && this.tag == "TopCreature")) // { // go face Target = h.transform.gameObject; // } // else if ((h.transform.tag == "TopCreature" && this.tag == "LowCreature") || // (h.transform.tag == "LowCreature" && this.tag == "TopCreature")) // { // // hit a creature, save parent transform // Target = h.transform.parent.gameObject; // } }
    //         bool targetValid = false;
    //         if (Target != null)
    //         {
    //             string targetID = Target.GetComponent<IDHolder>().UniqueID;
    //             Debug.Log("Target ID: " + targetID + "Low player: " + GlobalSettings.Instance.LowPlayer.PlayerID +
    //                       "TopP Player: " + GlobalSettings.Instance.TopPlayer.PlayerID);
    //             if (targetID == GlobalSettings.Instance.LowPlayer.PlayerID.ToString() ||
    //                 targetID == GlobalSettings.Instance.TopPlayer.PlayerID.ToString())
    //             {
    //                 // attack character
    //                 Debug.Log("Attacking " + Target);
    //                 Debug.Log("TargetID: " + targetID);
    //                 CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].GoFace();
    //                 targetValid = true;
    //             }
    //             else if (CreatureLogic.CreaturesCreatedThisGame[targetID] != null)
    //             {
    //                 // if targeted creature is still alive, attack creature
    //                 targetValid = true;
    //                 CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID]
    //                     .AttackCreatureWithID(targetID);
    //                 Debug.Log("Attacking " + Target);
    //             }
    //         }
    //
    //         if (!targetValid)
    //         {
    //             // not a valid target, return
    //             whereIsThisCreature.VisualState = VisualStates.LowTable;
    //             whereIsThisCreature.SetTableSortingOrder();
    //         }
    //
    //         // return target and arrow to original position
    //         transform.localPosition = Vector3.zero;
    //         sr.enabled = false;
    //         lr.enabled = false;
    //         triangleSR.enabled = false;
    //     }
    // }
    public override void OnEndDrag()
    {
        Debug.Log("End dragging");

        // 1) szukamy targetu
        Target = null;

        var origin = Camera.main.transform.position;
        var dir = (transform.position - origin).normalized;

        RaycastHit[] hits = Physics.RaycastAll(origin, dir, 200f);

        // (opcjonalnie) dla debug:
        Debug.Log($"Raycast hits: {hits.Length}");

        foreach (var h in hits)
        {
            // Debug.Log($"...");
            // ignoruj samego siebie i swoje dzieci
            if (h.transform == transform) continue;
            // ignoruj wszystko bez holdera
            if (h.transform.GetComponentInParent<IDHolder>() == null)
                continue;
            // Debug.Log($"good is!!! ->: {h.transform.name}");
            // bierz pierwszy sensowny obiekt


            //TODO ogarnąć, że to przeciwnika poprzez lower card i top cards


            Target = h.transform.gameObject;
            break;
        }

        bool targetValid = false;

        // 2) walidacja targetu i wykonanie akcji
        if (Target != null)
        {
            // IDHolder może być na parent
            var idHolder = Target.GetComponentInParent<IDHolder>();
            if (idHolder != null)
            {
                string targetID = idHolder.UniqueID;
                Debug.Log($"Target: {Target.name}, targetID: {targetID}");
                // atak w gracza
                // if (targetID == GlobalSettings.Instance.LowPlayer.PlayerID.ToString() ||
                //     targetID == GlobalSettings.Instance.TopPlayer.PlayerID.ToString())
                // {
                //     CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].GoFace();
                //     targetValid = true;
                // }
                // else
                // {
                // atak w kreaturę (bez KeyNotFound)
                if (CreatureLogic.CreaturesCreatedThisGame.TryGetValue(targetID, out var creature) &&
                    creature != null)
                {
                    CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID]
                        .AttackCreatureWithID(targetID);
                    Debug.Log($"PO CRETURE ATTACK");
                    targetValid = true;
                    Debug.Log($"PO CRETURE ATTACK XXX");

                }
                // }
            }
            else
            {
                Debug.Log($"Target {Target.name} has failed somehow");
            }
        }
        Debug.Log($"YYY");

        // 3) jeśli nieważny target -> wróć
        if (!targetValid)
        {

            whereIsThisCreature.VisualState = VisualStates.LowTable;
            Debug.Log($"ZZZ");

            whereIsThisCreature.SetTableSortingOrder();
            Debug.Log($"BBB");

        }
        Debug.Log($"AAA");

        // 4) zawsze resetuj wizual
        transform.localPosition = Vector3.zero;
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;
        Debug.Log($"HAHAHA");

    }

    // NOT USED IN THIS SCRIPT
    protected override bool DragSuccessful()
    {
        return true;
    }
}