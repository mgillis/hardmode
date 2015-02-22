using System;
using RimWorld;
using Verse;

namespace HardMode.Verbs
{
    public abstract class Verb_UseTool : Verb
    {
        protected abstract int TicksToNextHit();

        protected override void InitCast()
        {
            BeforeWork();
            base.InitCast();
        }

        new public void VerbTick()
        {
            base.VerbTick();
        }

        protected override bool TryShotSpecialEffect()
        {
            // XXX HACK -- but I guess this whole class is
            burstShotsLeft++;
            verbProps.ticksBetweenBurstShots = TicksToNextHit();

            bool finished;
            DoWork(currentTarget, out finished);
            return !finished;
        }

        protected abstract void BeforeWork();
        protected abstract void DoWork(TargetInfo target, out bool finished);

    }
}
