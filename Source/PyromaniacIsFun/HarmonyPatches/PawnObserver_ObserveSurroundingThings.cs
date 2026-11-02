#nullable enable

using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(PawnObserver), "ObserveSurroundingThings")]
public static class PawnObserver_ObserveSurroundingThings
{
    private static readonly MethodInfo pawnObserverPossibleToObserve =
        AccessTools.Method(typeof(PawnObserver), "PossibleToObserve");

    private static void tryCreateObservedThought(Thing thing, Pawn pawn)
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
                    if ((bool)(pawnObserverPossibleToObserve.Invoke(__instance, [item]) ?? false))
                    {
                        tryCreateObservedThought(item, pawn);
                    }
                }

                return true;
            });
    }
}