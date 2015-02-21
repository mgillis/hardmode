using System.Linq;
using Verse;
using Verse.AI;

namespace HardMode.Utility
{
    static class JobUtilities
    {
        /// <summary>
        /// Make a job out of the given jobdef for pawn targetting thing, OR create a 
        /// haul job to move something out of the way so the job can be done.
        /// If it's just impossible, returns null.
        /// </summary>
        public static Job MakeJobOrHaulableIfBlocked(JobDef jobdef, Pawn pawn, Thing thing)
        {
            if (CellUtilities.EnumerateAdjacentCells(thing.Position).Any(c => c.InBounds() && c.Standable()))
            {
                return new Job(jobdef, new TargetInfo(thing));
            }

            var impediment = CellUtilities.GetHaulableImpedimentBeside(thing.Position);

            if (impediment != null)
                return HaulAIUtility.HaulAsideJobFor(pawn, impediment);

            return null;
        }
    }
}