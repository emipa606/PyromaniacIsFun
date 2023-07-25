#nullable enable

using System;
using RimWorld;
using Verse;

namespace CF_PyromaniacIsFun;

public class ThoughtWorker_NeedPyromania : ThoughtWorker
{
    // See `ThoughtWorker_NeedJoy`
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
        if (p.needs.TryGetNeed<NeedPyromania>() is { } need)
        {
            return need.CurCategory switch
            {
                PyromaniaCategory.VeryLow => ThoughtState.ActiveAtStage(0),
                PyromaniaCategory.Low => ThoughtState.ActiveAtStage(1),
                PyromaniaCategory.Satisfied => ThoughtState.Inactive,
                PyromaniaCategory.High => ThoughtState.ActiveAtStage(2),
                PyromaniaCategory.VeryHigh => ThoughtState.ActiveAtStage(3),
                var cat => throw new NotImplementedException($"{cat} is not handled")
            };
        }

        return ThoughtState.Inactive;
    }
}