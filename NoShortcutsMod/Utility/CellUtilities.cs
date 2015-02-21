using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace HardMode.Utility
{
    static class CellUtilities
    {
        public static IEnumerable<IntVec3> EnumerateAdjacentCells(IntVec3 position)
        {
            for (var index = 0; index < 8; ++index)
            {
                yield return position + GenAdj.AdjacentCells[index];
            }
        }

        public static IntVec3? GetStandableBeside(IntVec3 position)
        {
            return EnumerateAdjacentCells(position).FirstOrDefault(c => c.InBounds() && c.Standable());
        }

        public static bool IsStandableBeside(IntVec3 position)
        {
            return GetStandableBeside(position) != null;
        }

        /// <summary>
        /// Get the thing that's blocking position. Can be null, if nothing haulable is there.
        /// </summary>
        public static Thing GetHaulableImpedimentBeside(IntVec3 position)
        {
            return (
                from c in EnumerateAdjacentCells(position)
                where c.InBounds() && c.Walkable() && !c.Standable()
                select c.GetFirstHaulable())
                .FirstOrDefault();
        }

        public static IntVec3 RandomCloseSpotWith(IntVec3 start, Predicate<IntVec3> validator)
        {
            var squareRadius = 0;
            var foundSq = IntVec3.Invalid;

            for (var searches = 0; searches < 30; searches++)
            {
                squareRadius += 4;
                if (GenCellFinder.TryFindRandomMapCellNearWith(start, squareRadius, validator, out foundSq))
                {
                    return foundSq;
                }
            }
            return foundSq;

        }

        public static IntVec3 RandomCloseReachableSpotWith(Pawn pawn, Predicate<IntVec3> validator)
        {
            Predicate<IntVec3> combined = c => validator(c) && 
                    c.Standable() && 
                    !Find.PawnDestinationManager.DestinationIsReserved(c) && 
                    pawn.CanReach(c, PathMode.OnCell, Danger.Some);
            return RandomCloseSpotWith(pawn.Position, combined);
        }
    }
}