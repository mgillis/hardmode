using System.Linq;
using HardMode.Things;
using RimWorld;
using Verse;

namespace HardMode.Gensteps
{
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
                    Log.Error("tool wasn't found in equipment after tool added to equipment");
                }
            }

        }
    }
}
