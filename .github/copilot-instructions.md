# GitHub Copilot Instructions for Pyromaniac Is Fun (Continued)

## Mod Overview and Purpose
Pyromaniac Is Fun (Continued) is an update to colinfangs mod that aims to make playing with Pyromaniacs more interesting. The mod introduces unique mechanisms and features for Pyromaniacs, allowing for a variety of play styles. It is recommended that the Royalty DLC is installed for optimal functionality.

## Key Features and Systems
The mod includes the following features and systems:

1. **Fire Need Bar**: Pyromaniacs have a Fire need bar that affects their mood. The bar gradually drops and can be increased through various actions such as meditating on flames, observing wild fires, or being on fire.
2. **Pyromania Threshold Indicator**: A threshold indicator below the mood bar helps prevent Pyromaniacs from triggering "fire starting spree" if their mood is above a certain level. The threshold is lower when the Fire need bar is longer.
3. **Fire Arrows**: Pyromaniacs with bows have a chance to shoot fire arrows, dealing extra flame damage and reducing the Fire need bar.
4. **Ignite in Melee**: Pyromaniacs can ignite their target in melee attacks, dealing flame damage and reducing the Fire need bar.
5. **Incendiary Weapon Accuracy**: Pyromaniacs no longer have a forced miss radius when carrying certain incendiary weapons, allowing for more accurate shots.
6. **No Mood Bonus for Certain Weapons**: Pyromaniacs no longer receive a mood bonus when carrying certain incendiary weapons, depending on the projectile damage type.

## Coding Patterns and Conventions
In this mod, the following coding patterns and conventions are used:

1. **Harmony Patching**: The mod uses Harmony patching to modify existing RimWorld functions. Patches are located in the `HarmonyPatches` directory.
2. **Custom Needs**: A custom need called `NeedPyromania` is implemented to manage the Fire need bar.
3. **Custom Damage Definitions**: Custom damage definitions are created for fire-related damage in the XML files.

## XML Integration
XML files are used to define custom game elements such as damage definitions, needs, thoughts, and more. The following XML files are included in the mod:

- `Pyromaniac.xml`: Defines various elements related to Pyromaniacs, including damage definitions, needs, thoughts, and more.
- `Patch_Pyromaniac.xml`: Contains Harmony patches for Pyromaniacs.
- `About.xml`: Provides information about the mod and its dependencies.

## Harmony Patching
Harmony patching is a powerful technique used in RimWorld modding to modify the behavior of existing code. The mod uses Harmony to patch various functions, such as:

- `DefGeneratorGenerateImpliedDefs_PreResolve`: Generates implied definitions for Pyromaniacs.
- `Verb_LaunchProjectile_TryCastShot`: Modifies the behavior of projectile launchers to include fire arrow functionality.
- `VerbProperties_GetForceMissFactorFor`: Removes the forced miss radius for certain incendiary weapons.
- `VerbUtility_FinalSelectionWeight`: Adjusts the selection weight of verbs for Pyromaniacs.

## Suggestions for Copilot
To make the most out of Copilot when working on this mod, consider the following suggestions:

1. **Use Descriptive Comments**: Add descriptive comments to your code to help Copilot understand the purpose of each function and patch.
2. **Follow Coding Conventions**: Stick to the coding patterns and conventions used in the mod, such as naming conventions and Harmony patching.
3. **Test and Validate**: After generating code with Copilot, thoroughly test and validate the changes to ensure they work as expected and do not introduce new issues.
4. **Consult XML Documentation**: Refer to the XML documentation for RimWorld to understand the structure and purpose of each XML element.

## Additional Resources
- [RimWorld Modding Wiki](https://rimworldwiki.wikia.com/wiki/Modding)
- [Harmony Documentation](https://github.com/pardeike/Harmony/wiki)

---

This .github/copilot-instructions.md file provides a comprehensive guide for developers working on the Pyromaniac Is Fun (Continued) mod, covering the mod's purpose, key features, coding conventions, XML integration, and Harmony patching.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

