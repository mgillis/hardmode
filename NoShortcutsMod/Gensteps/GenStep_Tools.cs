using System.Linq;
using HardMode.Things;
using RimWorld;
using Verse;

namespace HardMode.Gensteps
{
    /// <summary>
    /// Generates tools directly in colonist equipment.
    /// I decided to not use this after all and just generate them on the ground.
    /// </summary>
    public class Genstep_Tools : Genstep
    {
        public override void Generate()
        {
            foreach (Pawn pawn in MapInitData.colonists)
            {
                Log.Message("generating a tool");
                var tool = (Tool) ThingMaker.MakeThing(ThingDef.Named("Tool_LaserTool"), ThingDefOf.Plasteel);
                PawnGenerator.PostProcessGeneratedGear(tool, pawn);

                Log.Message("adding to equipment");
                pawn.equipment.AddEquipment(tool);

                var foundtool = pawn.equipment.AllEquipment.First(eq => eq is Tool);
                if (foundtool == null)
                {
                    // this happens if the equipment type isn't Primary, in Alpha 9
                    Log.Error("tool wasn't found in equipment after tool added to equipment");
                }
            }

        }
    }
}
