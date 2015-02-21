using System;
using System.Runtime.InteropServices;
using Verse;
using Verse.AI;

namespace HardMode.Utility
{
    static class Extensions
    {
        #region Designations
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
        #endregion

        #region Glow
        public static PsychGlow PsychGlowAt(this IntVec3 position)
        {
            return Find.GlowGrid.PsychGlowAt(position);
        }

        public static bool IsPsychDark(this IntVec3 position)
        {
            return PsychGlowAt(position) != PsychGlow.Dark;
        }
        #endregion

        #region Pawn
        public static IntVec3 RandomCloseReachableSpotWith(this Pawn pawn, Predicate<IntVec3> validator)
        {
            return CellUtilities.RandomCloseReachableSpotWith(pawn, validator);
        }
        #endregion
    }
}