using HardMode.Utility;
using RimWorld;
using Verse;
using Verse.AI;

namespace HardMode
{
  public class JobGiver_SeekLight : ThinkNode_JobGiver
  {
    protected override Job TryGiveTerminalJob(Pawn pawn)
    {
        if (!pawn.Position.IsPsychDark())
            return null;

        Log.Message("trying to find brighter spot for " + pawn);

        var brightSpot = pawn.RandomCloseReachableSpotWith(c => !c.IsPsychDark());

        Log.Message("found spot " + brightSpot);
        
        if (brightSpot != IntVec3.Invalid)
            return new Job(JobDefOf.Goto, brightSpot);

        return null;
    }

  }
}

