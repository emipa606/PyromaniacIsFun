#nullable enable

using System;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace CF_PyromaniacIsFun;

public class NeedPyromania : Need
{
    public const float ThresholdVeryLow = 0.1f;
    public const float ThresholdLow = 0.3f;
    public const float ThresholdSatisfied = 0.6f;
    public const float ThresholdHigh = 0.8f;
    public string ExplanationAll = "";
    public string? ExplanationFromAdjustExternally;
    public string ExplanationFromObservedFire = "";
    private float lastAdjustExternallyAmount;
    private int lastAdjustExternallyTick = -999;

    // End of see `Need_joy`

    public float lastDelta;

    public NeedPyromania(Pawn pawn) : base(pawn)
    {
        // See `Need_Beauty`
        threshPercents =
        [
            ThresholdVeryLow,
            ThresholdLow,
            ThresholdSatisfied,
            ThresholdHigh
        ];
    }

    // See `Need_Joy`
    public bool IsAdjustExternally => Find.TickManager.TicksGame < lastAdjustExternallyTick + 10;

    public override int GUIChangeArrow =>
        IsFrozen ? 0 : IsAdjustExternally ? Math.Sign(lastAdjustExternallyAmount) : Math.Sign(lastDelta);

    public PyromaniaCategory CurCategory => CurLevel switch
    {
        < ThresholdVeryLow => PyromaniaCategory.VeryLow,
        < ThresholdLow => PyromaniaCategory.Low,
        < ThresholdSatisfied => PyromaniaCategory.Satisfied,
        < ThresholdHigh => PyromaniaCategory.High,
        _ => PyromaniaCategory.VeryHigh
    };

    public void AdjustExternally(float amount, string? explaination = null)
    {
        // Called by `JobDriver_Meditate.MeditationTick`
        // Or by e.g. fire arrow
        CurLevel = Mathf.Clamp01(CurLevel + amount);
        lastAdjustExternallyTick = Find.TickManager.TicksGame;
        lastAdjustExternallyAmount = amount;
        ExplanationFromAdjustExternally = explaination;
    }

    public override string GetTipString()
    {
        var text = base.GetTipString();
        return text + "\n\n" + ExplanationAll;
    }

    public void CheckSelfOnFire()
    {
        if (!pawn.HasAttachment(ThingDefOf.Fire))
        {
            return;
        }

        var thought = (Thought_Memory)ThoughtMaker.MakeThought(PyromaniacUtility.SelfOnFireDef);
        PyromaniacUtility.ThrowText(pawn, () => "CF_PyromaniacIsFun_PyromaniacUtility_IAmOnFire".Translate(), 4);
        pawn.needs.mood.thoughts.memories.TryGainMemory(thought);
    }

    public float GainFromObservedFireInterval()
    {
        CheckSelfOnFire();
        var numWildFire = 0;
        var minAgeWildFire = int.MaxValue;
        var numBurningPawn = 0;
        var minAgeBurningPawn = int.MaxValue;
        var numSelfOnFire = 0;
        var minAgeSelfOnFire = int.MaxValue;
        var sb = new StringBuilder();
        if (pawn.needs.mood?.thoughts.memories is { } handler)
        {
            foreach (var thought in handler.Memories)
            {
                if (thought.def == PyromaniacUtility.ObservedWildFireDef)
                {
                    numWildFire += 1;
                    minAgeWildFire = Math.Min(minAgeWildFire, thought.age);
                }
                else if (thought.def == PyromaniacUtility.ObservedBurningPawnDef)
                {
                    numBurningPawn += 1;
                    minAgeBurningPawn = Math.Min(minAgeBurningPawn, thought.age);
                }
                else if (thought.def == PyromaniacUtility.SelfOnFireDef)
                {
                    numSelfOnFire += 1;
                    minAgeSelfOnFire = Math.Min(minAgeSelfOnFire, thought.age);
                }
            }
        }

        float gain = 0;
        if (numWildFire > 0)
        {
            var gainFromWildFire = numWildFire * Patcher.Settings.NeedPyromaniaGainPerWildFirePerDay;
            gain += gainFromWildFire;
            sb.AppendLine("CF_PyromaniacIsFun_NeedPyromania.SawWildFire".Translate(numWildFire,
                (gainFromWildFire * 100).ToString("F0"),
                (PyromaniacUtility.ObservedWildFireDef.DurationTicks - minAgeWildFire).ToStringTicksToPeriod())
            );
        }

        if (numBurningPawn > 0)
        {
            var gainFromBurningPawn = numBurningPawn * Patcher.Settings.NeedPyromaniaGainPerBurningPawnPerDay;
            gain += gainFromBurningPawn;
            sb.AppendLine("CF_PyromaniacIsFun_NeedPyromania.SawBurningPawn".Translate(numBurningPawn,
                (gainFromBurningPawn * 100).ToString("F0"),
                (PyromaniacUtility.ObservedBurningPawnDef.DurationTicks - minAgeBurningPawn).ToStringTicksToPeriod())
            );
        }

        if (numSelfOnFire > 0)
        {
            var gainFromSelfOnFire = numSelfOnFire * Patcher.Settings.NeedPyromaniaGainSelfOnFirePerDay;
            gain += gainFromSelfOnFire;
            sb.AppendLine("CF_PyromaniacIsFun_NeedPyromania.IAmOnFire".Translate(numSelfOnFire,
                (gainFromSelfOnFire * 100).ToString("F0"),
                (PyromaniacUtility.SelfOnFireDef.DurationTicks - minAgeSelfOnFire).ToStringTicksToPeriod())
            );
        }

        ExplanationFromObservedFire = sb.ToString();
        return gain;
    }

    public override void NeedInterval()
    {
        if (IsFrozen)
        {
            return;
        }

        if (IsAdjustExternally)
        {
            ExplanationAll = ExplanationFromAdjustExternally ?? "";
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine(
            "CF_PyromaniacIsFun_NeedPyromania.BaseChangeRate".Translate((def.fallPerDay * 100).ToString("F0")));
        var gain = -def.fallPerDay;
        var gainFromObservedFireInterval = GainFromObservedFireInterval();
        if (gainFromObservedFireInterval > 0)
        {
            gain += gainFromObservedFireInterval;
            sb.Append(ExplanationFromObservedFire);
        }

        sb.AppendLine()
            .AppendLine("CF_PyromaniacIsFun_NeedPyromania.FinalChangeRate".Translate((gain * 100).ToString("F0"),
                (gain * 100 / GenDate.HoursPerDay).ToString("F0")));
        lastDelta = gain * NeedTunings.NeedUpdateInterval / GenDate.TicksPerDay;
        CurLevel = Mathf.Clamp01(CurLevel + lastDelta);
        ExplanationAll = sb.ToString();
    }

    public float GetMentalBreakProtectThreshold()
    {
        // A pawn would be protected from FireStartingSpree is his mood is higher than this threshold
        var threshold = 1 - CurLevel;
        var breakThresholdExtreme = pawn.mindState.mentalBreaker.BreakThresholdExtreme;
        if (threshold < breakThresholdExtreme)
        {
            threshold = breakThresholdExtreme;
        }

        return threshold;
    }
}