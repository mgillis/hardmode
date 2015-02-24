using System;
using RimWorld;
using Verse;

namespace HardMode.Verbs
{
    /// <summary>
    /// Create a child of this to implement a with-tool verb.
    /// </summary>
    public abstract class Verb_UseTool : Verb
    {
        protected abstract int TicksToNextHit();
        protected abstract void BeforeWork();
        protected abstract void DoWork(TargetInfo target, out bool finished);

        protected override void InitCast()
        {
            BeforeWork();
            base.InitCast();
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

    }
}
