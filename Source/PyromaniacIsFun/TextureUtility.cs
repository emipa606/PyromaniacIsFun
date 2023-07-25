using UnityEngine;
using Verse;

namespace CF_PyromaniacIsFun;

[StaticConstructorOnStartup]
public static class TextureUtility
{
    public static readonly Texture2D PyromaniaIndicator = ContentFinder<Texture2D>.Get("UI/Icons/PassionMinor");
}