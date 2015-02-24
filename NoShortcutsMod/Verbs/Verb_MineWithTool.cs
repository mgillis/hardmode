using System;
using RimWorld;
using Verse;

namespace HardMode.Verbs
{
    public class Verb_MineWithTool : Verb_UseTool
    {
        private const int BaseTicksBetweenPickHits = 120;
        private const int BaseDamagePerPickHit = 80;
        private int amount;

        // XXX DEBUG
        new public void Notify_PickedUp()
        {
            Log.Warning(caster + " got " + ownerEquipment + " that spawned/notified a Verb_MineWithTool");
            base.Notify_PickedUp();
        }

        protected override int TicksToNextHit()
        {
            return (int) Math.Round(BaseTicksBetweenPickHits/(double) CasterPawn.GetStatValue(StatDefOf.MiningSpeed));
        }

        protected override void BeforeWork()
        {
            amount = BaseDamagePerPickHit;
            if (ResearchProjectDef.Named("PneumaticPicks").IsFinished)
                amount = (int)Math.Round((double)amount * 1.2f);
        }

        protected override void DoWork(TargetInfo target, out bool finished)
        {
            BodyPartDamageInfo bodyPartDamageInfo = new BodyPartDamageInfo(BodyPartHeight.Top,
                BodyPartDepth.Outside);
            DamageInfo dinfo = new DamageInfo(DamageDefOf.Mining, amount, caster, bodyPartDamageInfo);

            target.Thing.TakeDamage(dinfo);
            if (target.ThingDestroyed)
            {
                MineStrikeManager.CheckStruckOre(target.Cell, target.Thing.def, caster);
                finished = true;
            }
            else
                finished = false;
        }
    }
}
