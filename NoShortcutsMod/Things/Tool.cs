using Verse;

namespace HardMode.Things
{
    public class Tool : Equipment
    {
        // XXX this should be a property on a thingdef
        public EffecterDef effecterDef = EffecterDef.Named("LaserToolUse");

        new public void InitVerbs()
        {
            base.InitVerbs();
            Log.Warning("InitVerbs: Tool " + this + " picked up by " + holder);
        }
    }
}