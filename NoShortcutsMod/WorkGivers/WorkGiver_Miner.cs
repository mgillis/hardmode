using System.Collections.Generic;
using HardMode.Utility;
using RimWorld;
using Verse;
using Verse.AI;

namespace HardMode
{
    class WorkGiver_Miner : RimWorld.WorkGiver_Miner
    {
        public WorkGiver_Miner(WorkGiverDef giverDef) : base(giverDef)
        {
        }

        public override PathMode PathMode
        {
            get { return PathMode.Touch; }
        }

        private static bool IsMiningResearched()
        {
            return ResearchProjectDef.Named("Mining").IsFinished;
        }

        public override ThingRequest PotentialWorkThingRequest
        {
            get { return ThingRequest.ForGroup(ThingRequestGroup.Nothing); }
        }
        
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return IsMiningResearched() ? Find.ListerThings.AllThings.FindAll(t => t is Mineable) : null;
        }

        public override Job JobOnThing(Pawn pawn, Thing t)
        {
            if (!t.def.mineable)
                return null;

            if (!DesignationDefOf.Mine.IsDefinedAt(t.Position))
                return null;

            Log.Message("jobonthing for " + pawn.Nickname + ", " + t);
            if (!IsMiningResearched())
                return null;

            Log.Message("trying to reserve");
            if (!pawn.CanReserve((TargetInfo)t, ReservationType.Total))
                return null;

            return JobUtilities.MakeJobOrHaulableIfBlocked(JobDefOf.Mine, pawn, t);
        }
    }
}
