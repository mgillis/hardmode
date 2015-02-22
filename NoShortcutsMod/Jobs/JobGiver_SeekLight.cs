using HardMode.Utility;
using RimWorld;
using Verse;
using Verse.AI;

namespace HardMode.Jobs
{
    public class JobGiver_SeekLight : ThinkNode_JobGiver
    {
        private const int searchRadius = 40;

        protected override Job TryGiveTerminalJob(Pawn pawn)
        {
            if (!pawn.Position.IsPsychDark())
                return null;

            Log.Message("trying to find brighter spot for " + pawn);

            var brightSpot = pawn.RandomCloseReachableSpotWith(c => !c.IsPsychDark(), searchRadius);

            if (brightSpot != IntVec3.Invalid)
            {
                Log.Message("found spot " + brightSpot);
                return new Job(JobDefOf.Goto, brightSpot);
            }
            else
            {
                Log.Message("no brighter spot");
                return null;
            }

        }
    }
}