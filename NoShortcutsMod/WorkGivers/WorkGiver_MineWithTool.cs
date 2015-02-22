using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using HardMode.Utility;
using HardMode.Verbs;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace HardMode.WorkGivers
{
    class WorkGiver_MineWithTool : WorkGiver_Miner
    {
        public WorkGiver_MineWithTool(WorkGiverDef giverDef) : base(giverDef)
        {
        }

        public override bool ShouldSkip(Pawn pawn)
        {
            if (!IsMiningResearched())
                return true;

            if (pawn.GetVerbOnEquipment(typeof(Verb_MineWithTool)) == null)
                return true;

            return false;
        }


        //public override PathMode PathMode
        //{
        //    get { return PathMode.Touch; }
        //}

        private static bool IsMiningResearched()
        {
            return true;
            //return ResearchProjectDef.Named("Mining").IsFinished;
        }

        //public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        //{
        //    return base.PotentialWorkThingsGlobal(pawn);
        //}

        public override ThingRequest PotentialWorkThingRequest
        {
            get { return ThingRequest.ForGroup(ThingRequestGroup.Building); }
        }

        public override Job JobOnThing(Pawn pawn, Thing thing)
        {
            if (!thing.def.mineable)
                return null;

            if (!DesignationDefOf.Mine.IsDefinedAt(thing.Position))
                return null;

            if (!IsMiningResearched())
                return null;

            Log.Message("jobonthing for " + pawn.Nickname + ", " + thing);

            var mineverb = pawn.GetVerbOnEquipment(typeof (Verb_MineWithTool));
            if (mineverb == null)
            {
                Log.Message("wanted to mine, but no equipment with proper verb");
                // TODO get a tool
                return null;
            }

            if (!pawn.CanReserve(thing, ReservationType.Total))
            {
                Log.Message("couldn't reserve");
                return null;
            }

            Job job;
            var standable = JobUtilities.CanStandBy(pawn, thing, out job);

            if (standable)
            {
                Log.Message("standable beside, returning jobdefof mine, target " + thing + ", verb mineverb");
                return new Job(JobDefOf.Mine, new TargetInfo(thing))
                {
                    verbToUse = mineverb
                };
            }
            else
            {
                Log.Message("not standable beside, returning " + job);
                // haul job
                return job;
            }
        }
    }
}
