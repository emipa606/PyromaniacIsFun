#nullable enable

using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(PawnObserver))]
[HarmonyPatch("ObserveSurroundingThings")]
public static class Patch_PawnObserver_ObserveSurroundingThings
{
    public static readonly MethodInfo PawnObserver_PossibleToObserve =
        typeof(PawnObserver).GetMethod("PossibleToObserve", BindingFlags.Instance | BindingFlags.NonPublic) ??
        throw new ArgumentException("PawnObserver.PossibleToObserve is not found");

    public static void TryCreateObservedThought(Thing thing, Pawn pawn)
    {
        if (thing is not Fire fire)
        {
            return;
        }

        // See `Building_Skullspike.GiveObservedThought`
        Thought_MemoryObservation thought;
        if (fire.parent is Pawn pawnOnFire)
        {
            thought = (Thought_MemoryObservation)ThoughtMaker.MakeThought(PyromaniacUtility.ObservedBurningPawnDef);
            thought.Target = pawnOnFire;
        }
        else
        {
            thought = (Thought_MemoryObservation)ThoughtMaker.MakeThought(PyromaniacUtility.ObservedWildFireDef);
            thought.Target = fire;
        }

        // `Fire.Label` already contains `Fire.parent`
        PyromaniacUtility.ThrowText(pawn,
            () => "CF_PyromaniacIsFun_PyromaniacUtility_ObservedFire".Translate(fire.Label), 4);
        pawn.needs.mood.thoughts.memories.TryGainMemory(thought);
    }

    public static void Postfix(PawnObserver __instance, Pawn ___pawn)
    {
        var pawn = ___pawn;
        RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map,
            (_, to) => pawn.Position.InHorDistOf(to.extentsClose.ClosestCellTo(pawn.Position), 5f),
            delegate(Region reg)
            {
                foreach (var item in reg.ListerThings.ThingsInGroup(ThingRequestGroup.Fire))
                {
                    if ((bool)PawnObserver_PossibleToObserve.Invoke(__instance, new object[] { item }))
                    {
                        TryCreateObservedThought(item, pawn);
                    }
                }

                return true;
            });
    }
}