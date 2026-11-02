using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(Need_Mood), nameof(Need_Mood.DrawOnGUI))]
public static class Need_Mood_DrawOnGUI
{
    private static readonly float barInstantMarkerSize =
        (float)(AccessTools.Field(typeof(Need), "BarInstantMarkerSize").GetValue(null) ?? 150f);

    private static void drawPyromaniaIndicator(Rect barRect, float pct)
    {
        // See `Need.DrawBarInstantMarkerAt`
        var markerSize = barInstantMarkerSize;
        if (barRect.width < 150f)
        {
            markerSize /= 2f;
        }

        var vector = new Vector2(barRect.x + (barRect.width * pct), barRect.y + barRect.height);
        GUI.DrawTexture(new Rect(vector.x - (markerSize / 2f), vector.y, markerSize, markerSize),
            TextureUtility.PyromaniaIndicator);
    }

    public static void Postfix(Rect rect, Pawn ___pawn, float customMargin)
    {
        if (___pawn.needs.TryGetNeed<NeedPyromania>() is not { } need)
        {
            return;
        }

        var threshold = need.GetMentalBreakProtectThreshold();

        // rect3 is derived from the original function
        const float drawHeight = Need.MaxDrawHeight;
        if (rect.height > drawHeight)
        {
            var num = (rect.height - drawHeight) / 2f;
            rect.height = drawHeight;
            rect.y += num;
        }

        var num2 = 14f;
        var num3 = customMargin >= 0f ? customMargin : num2 + 15f;
        if (rect.height < 50f)
        {
            num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
        }

        var rect3 = new Rect(rect.x, rect.y + (rect.height / 2f), rect.width, rect.height / 2f);
        rect3 = new Rect(rect3.x + num3, rect3.y, rect3.width - (num3 * 2f), rect3.height - num2);

        drawPyromaniaIndicator(rect3, threshold);
    }
}