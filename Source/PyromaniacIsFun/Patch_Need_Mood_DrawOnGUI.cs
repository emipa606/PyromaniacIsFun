using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(Need_Mood))]
[HarmonyPatch(nameof(Need_Mood.DrawOnGUI))]
public static class Patch_Need_Mood_DrawOnGUI
{
    public static readonly float BarInstantMarkerSize = (float)typeof(Need)
        .GetField("BarInstantMarkerSize", BindingFlags.Static | BindingFlags.NonPublic)
        ?.GetValue(null)!;

    public static void DrawPyromaniaIndicator(Rect barRect, float pct)
    {
        // See `Need.DrawBarInstantMarkerAt`
        // TODO: Which const is 150f?
        var markerSize = BarInstantMarkerSize;
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
        var maxDrawHeight = Need.MaxDrawHeight;
        if (rect.height > maxDrawHeight)
        {
            var num = (rect.height - maxDrawHeight) / 2f;
            rect.height = maxDrawHeight;
            rect.y += num;
        }

        // TODO: Which const is 14f, 15f, 50f?
        var num2 = 14f;
        var num3 = customMargin >= 0f ? customMargin : num2 + 15f;
        if (rect.height < 50f)
        {
            num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
        }

        var rect3 = new Rect(rect.x, rect.y + (rect.height / 2f), rect.width, rect.height / 2f);
        rect3 = new Rect(rect3.x + num3, rect3.y, rect3.width - (num3 * 2f), rect3.height - num2);

        DrawPyromaniaIndicator(rect3, threshold);
    }
}