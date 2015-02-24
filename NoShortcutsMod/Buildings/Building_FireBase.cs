using RimWorld;
using UnityEngine;
using Verse;

namespace HardMode.Buildings
{
    /// <summary>
    /// This is here for any future buildings-with-a-fire-and-worktable-and-heat.
    /// TODO this should be a Comp? if possible?
    /// </summary>
    public class Building_FireBase : Building_WorkTable_HeatPush
    {
        #region copy-paste from campfire in alpha 9
        private static readonly Graphic FireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", ShaderDatabase.MotePostLight, IntVec2.one, Color.white);

        public override void DrawAt(Vector3 drawLoc)
        {
            base.DrawAt(drawLoc);
            FireGraphic.Draw(drawLoc, IntRot.north, this);
        }
        #endregion

    }
}
