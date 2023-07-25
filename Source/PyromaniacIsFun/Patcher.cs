using HarmonyLib;
using Mlie;
using UnityEngine;
using Verse;

namespace CF_PyromaniacIsFun;

public class Patcher : Mod
{
    public static Settings Settings = new Settings();
    private static string currentVersion;
    private string meleeIgniteChanceBuffer;
    private string needPyromaniaGainFromMeditationMultiplierBuffer;
    private string needPyromaniaGainPerBurningPawnPerDayBuffer;
    private string needPyromaniaGainPerWildFirePerDayBuffer;
    private string needPyromaniaGainSelfOnFirePerDayBuffer;
    private string needPyromaniaPerFireArrowBuffer;
    private string needPyromaniaPerIgniteBuffer;

    public Patcher(ModContentPack pack) : base(pack)
    {
        Settings = GetSettings<Settings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(pack.ModMetaData);
        DoPatching();
    }

    public override string SettingsCategory()
    {
        return "Pyromaniac Is Fun";
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var list = new Listing_Standard();
        list.Begin(inRect);

        {
            var rect = list.Label("CF_PyromaniacIsFun_SettingText_CostPerArrow.label".Translate(), tooltip: null);
            Widgets.TextFieldNumeric(rect.RightPartPixels(50), ref Settings.NeedPyromaniaPerFireArrow,
                ref needPyromaniaPerFireArrowBuffer, 0, 1);
        }
        {
            var rect = list.Label("CF_PyromaniacIsFun_SettingText_CostPerMelee.label".Translate(), tooltip: null);
            Widgets.TextFieldNumeric(rect.RightPartPixels(50), ref Settings.NeedPyromaniaPerIgnite,
                ref needPyromaniaPerIgniteBuffer, 0, 1);
        }
        {
            var rect = list.Label("CF_PyromaniacIsFun_SettingText_MeleeIgnite.label".Translate(),
                tooltip: "CF_PyromaniacIsFun_SettingText_MeleeIgnite.description".Translate());
            Widgets.TextFieldNumeric(rect.RightPartPixels(50), ref Settings.MeleeIgniteChance,
                ref meleeIgniteChanceBuffer, 0, 0.99f);
        }
        {
            var rect = list.Label("CF_PyromaniacIsFun_SettingText_WildFire.label".Translate(), tooltip: null);
            Widgets.TextFieldNumeric(rect.RightPartPixels(50), ref Settings.NeedPyromaniaGainPerWildFirePerDay,
                ref needPyromaniaGainPerWildFirePerDayBuffer, 0, 1);
        }
        {
            var rect = list.Label("CF_PyromaniacIsFun_SettingText_BurningPawn.label".Translate(), tooltip: null);
            Widgets.TextFieldNumeric(rect.RightPartPixels(50), ref Settings.NeedPyromaniaGainPerBurningPawnPerDay,
                ref needPyromaniaGainPerBurningPawnPerDayBuffer, 0, 1);
        }
        {
            var rect = list.Label("CF_PyromaniacIsFun_SettingText_SelfOnFire.label".Translate(), tooltip: null);
            Widgets.TextFieldNumeric(rect.RightPartPixels(50), ref Settings.NeedPyromaniaGainSelfOnFirePerDay,
                ref needPyromaniaGainSelfOnFirePerDayBuffer, 0, 1);
        }
        {
            var rect = list.Label("CF_PyromaniacIsFun_SettingText_Meditation.label".Translate(),
                tooltip: "CF_PyromaniacIsFun_SettingText_Meditation.description".Translate());
            Widgets.TextFieldNumeric(rect.RightPartPixels(50), ref Settings.NeedPyromaniaGainFromMeditationMultiplier,
                ref needPyromaniaGainFromMeditationMultiplierBuffer, 0, 100);
        }
        list.CheckboxLabeled("CF_PyromaniacIsFun_SettingText_MoodIncendiaryWeapon.label".Translate(),
            ref Settings.HappyWhenCarryingTrulyIncendiaryWeapon,
            "CF_PyromaniacIsFun_SettingText_MoodIncendiaryWeapon.description".Translate());

        list.CheckboxLabeled("CF_PyromaniacIsFun_SettingText_AimIncendiaryWeapon.label".Translate(),
            ref Settings.RemoveForcedMissRadius,
            "CF_PyromaniacIsFun_SettingText_AimIncendiaryWeapon.description".Translate());

        if (currentVersion != null)
        {
            list.Gap();
            GUI.contentColor = Color.gray;
            list.Label("CF_PyromaniacIsFun_SettingText_CurrentVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        list.End();
        base.DoSettingsWindowContents(inRect);
    }

    public void DoPatching()
    {
        var harmony = new Harmony("com.colinfang.PyromaniacIsFun");
        harmony.PatchAll();
    }
}

// Attribute is used for static `Texture2D`