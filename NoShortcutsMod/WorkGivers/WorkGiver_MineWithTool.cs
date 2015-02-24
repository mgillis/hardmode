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
    /// <summary>
    /// Mine with a tool that has Verb_MineWithTool on it.
    /// </summary>
    /* 
    <remarks>

     How WorkGivers actually work

     tldr: return something useful from PotentialWorkThingRequest and then make
      JobOnThing as efficient as you can. PotentialWorkThingsGlobal sucks. and don't
      forget about ShouldSkip.

     
     In the WorkGiverDef, you can set either of scanThings (default true) and 
     scanCells (defaultFalse).
      
     - First, ShouldSkip(pawn) is called. This is your first chance to tell the AI 
       that this pawn can't do this work right now. This saves cycles, so do it.
     - PathMode returns how a pawn can get to its work targets. (default Touch)
      
     Then, if scanThings, one of two possibilities:
        - PotentialWorkThingRequest is a general "what class of Things will this need", which
          restricts the search into a smaller closest-first list of potential targets. If this 
          is overriden with a useful result, JobOnThing(pawn, t) is checked from the closest thing
          outward from the pawn and the first non-null result is used.
     
        - BUT, if it's undefined, PotentialWorkThingsGlobal is called, which must supply an 
          IEnumerable<Thing> of every potential target of this work on the map. Then 
          JobOnThing(pawn, t) is (potentially) called on every one as it searches through the
          full enumerable list for the closest one.
     
     If scanCells:
        - PotentialWorkCellsGlobal is called, the list is searched for the closest, and 
          JobOnCell(pawn, cell) is used.
     

     The heart of the workgiver stuff is JobGiver_WorkRoot.TryGiveTerminalJob, which 
     calls this:
     pawn.Position.ClosestThingReachable(
         giver.PotentialWorkThingRequest,                        // thingReq
         giver.PathMode,                                         // pathMode
         TraverseParms.For(pawn, Danger.Deadly, true),           // traverseParams
         9999f,                                                  // maxDistance
         (Thing t) =>                                            // validator
            !t.IsForbidden(pawn.Faction) ? giver.HasJobOnThing(pawn, t) : false,  
         giver.PotentialWorkThingsGlobal(pawn),                  // customGlobalSearchSet
         -1                                                      // searchRegionsMax
         )
      
     if PotentialWorkThingRequest is defined, this calls the relatively efficient:
         maxRegions = searchRegionsMax < 0 ? 30 : searchRegionsMax;
         position.RegionwiseBFSWorker(thingReq, pathMode, traverseParams, validator, null, 0, maxRegions, maxDistance)
     
     but if it is undefined, then it's this, which iterates through everything in customGlobalSearchSet, and suckkkks:
         position.ClosestThing_Global(
            customGlobalSearchSet == null ? Find.ListerThings.ThingsMatching(thingReq) : customGlobalSearchSet, 
            maxDistance, 
            t => position.CanReach(t, pathMode, traverseParams) && validator(t)
         );
     
     </remarks>
     */
    class WorkGiver_MineWithTool : WorkGiver
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

        public override PathMode PathMode
        {
            get { return PathMode.Touch; }
        }

        private static bool IsMiningResearched()
        {
            return true;
            //return ResearchProjectDef.Named("Mining").IsFinished;
        }

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
