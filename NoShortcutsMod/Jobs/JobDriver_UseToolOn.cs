using System.Collections.Generic;
using HardMode.Things;
using Verse;
using Verse.AI;

namespace HardMode.Jobs
{
    public class JobDriver_UseToolOn : JobDriver
    {
        protected virtual JobCondition ResultOfDespawnedTarget()
        {
            return JobCondition.Succeeded;
        }

        //Constants
        private const TargetIndex VictimInd = TargetIndex.A;

        public JobDriver_UseToolOn(Pawn pawn) : base(pawn)
        {
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.EndOnDespawned(VictimInd, ResultOfDespawnedTarget());

            yield return Toils_Reserve.Reserve(VictimInd, ReservationType.Total);

            Toil gotoCastPos = Toils_Combat.GotoCastPosition(VictimInd);
            yield return gotoCastPos;

            Toil jumpIfCannotHit = Toils_Jump.JumpIfCannotHitTarget(VictimInd, gotoCastPos);
            yield return jumpIfCannotHit;

            var verbtoil = Toils_Combat.CastVerb(VictimInd);
            var tool = (Tool) CurJob.verbToUse.ownerEquipment;
            verbtoil = verbtoil.WithEffect(tool.effecterDef, VictimInd);
            yield return verbtoil;

            yield return Toils_Jump.Jump(jumpIfCannotHit);
        }
    }
}