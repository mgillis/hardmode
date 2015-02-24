using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace HardMode.Buildings
{
    /// <summary>
    /// This is just a reproduction of RimWorld.Mineable. I thought at first that I'd need to 
    /// make some changes here to get MineWithTool to work, but I didn't. So now it's just
    /// here because .. it's here.
    /// </summary>
    class Mineable : Building
    {
        // ReSharper disable once InconsistentNaming
        private int nonMiningDamageTaken;

        public override void PreApplyDamage(DamageInfo damage, out bool absorbed)
        {
            if (damage.Def.harmsHealth && damage.Def != DamageDefOf.Mining)
                nonMiningDamageTaken += Math.Min(damage.Amount, Health);
            absorbed = false;
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            base.Destroy(mode);
            if (mode != DestroyMode.Kill)
                return;
            def.building.soundMined.PlayOneShot(Position);
            if (def.building.mineableThing == null || Rand.Value >= def.building.mineableDropChance)
                return;


            var newThing = ThingMaker.MakeThing(def.building.mineableThing);
            if (newThing.def.stackLimit == 1)
            {
                newThing.stackCount = 1;
            }
            else if (nonMiningDamageTaken == 0)
            {
                newThing.stackCount = def.building.mineableYield;
            }
            else
            {
                // "mineableNonMinedEfficiency" = the minimum you'll get from something
                var pctDamaged = 1.0 - ((double)nonMiningDamageTaken / (double)MaxHealth);

                // for example, if 100% damaged, 0.7 + 0.3*0 = 0.7 ; if 75% damaged, 0.7+0.3*0.25= 0.8 or something
                var yieldPctAfterDamage = def.building.mineableNonMinedEfficiency + (1.0 - def.building.mineableNonMinedEfficiency) * pctDamaged;
                
                newThing.stackCount = Mathf.CeilToInt((float) (def.building.mineableYield * yieldPctAfterDamage));
            }
            GenSpawn.Spawn(newThing, Position);
        }
    }
}
