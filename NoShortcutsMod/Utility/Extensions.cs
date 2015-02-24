using System;
using Verse;

namespace HardMode.Utility
{
    static class Extensions
    {
        // Designation extensions
        public static Designation At(this DesignationDef def, IntVec3 position)
        {
            return Find.DesignationManager.DesignationAt(position, def);
        }

        public static bool IsDefinedAt(this DesignationDef def, IntVec3 position)
        {
            return def.At(position) != null;
        }

        public static Designation On(this DesignationDef def, Thing t)
        {
            return Find.DesignationManager.DesignationOn(t, def);
        }

        public static bool IsDefinedOn(this DesignationDef def, Thing t)
        {
            return def.On(t) != null;
        }

        // Position - Glow extensions
        public static PsychGlow PsychGlowAt(this IntVec3 position)
        {
            return Find.GlowGrid.PsychGlowAt(position);
        }

        public static bool IsPsychDark(this IntVec3 position)
        {
            return PsychGlowAt(position) == PsychGlow.Dark;
        }

        // Pawn extensions
        public static IntVec3 RandomCloseReachableSpotWith(this Pawn pawn, Predicate<IntVec3> validator, int maxRadius)
        {
            return CellUtilities.RandomCloseReachableSpotWith(pawn, validator, maxRadius);
        }

    }
}