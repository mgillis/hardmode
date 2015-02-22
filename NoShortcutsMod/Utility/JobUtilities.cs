using System.Linq;
using Verse;
using Verse.AI;

namespace HardMode.Utility
{
    static class JobUtilities
    {
        /// <summary>
        /// Check if a thing is standable beside. If not, set job to a 
        /// haul job to move something out of the way. Otherwise, job is null.
        /// In some cases, when pawn can't stand by thing but there's no way to fix that by hauling,
        /// job will still be null when CanStandBy is false.
        /// </summary>
        public static bool CanStandBy(Pawn pawn, Thing thing, out Job job)
        {
            if (CellUtilities.EnumerateAdjacentCells(thing.Position).Any(c => c.InBounds() && c.Standable()))
            {
                Log.Message("found any standable cell beside " + thing);
                job = null;
                return true;
            }

            var impediment = CellUtilities.GetHaulableImpedimentBeside(thing.Position);

            if (impediment != null)
            {
                Log.Message("impediment beside " + thing + " of " + impediment);
                job = HaulAIUtility.HaulAsideJobFor(pawn, impediment);
                return false;
            }

            Log.Message("can't stand beside & no impediment haulable");
            job = null;
            return false;
        }

        /// <summary>
        /// Might be null
        /// </summary>
        public static Verb GetVerbOnEquipment(this Pawn pawn, System.Type verbType)
        {
            return pawn.equipment.AllEquipmentVerbs.FirstOrDefault(v => v.GetType() == verbType);
        }
    }
}