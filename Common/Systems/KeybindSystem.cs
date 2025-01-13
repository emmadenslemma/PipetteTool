using Terraria.ModLoader;

namespace PipetteTool.Common.Systems
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind PipetteToolKeybind { get; private set; }
        public static ModKeybind ClearPipetteKeybind { get; private set; }

        public override void Load()
        {
            PipetteToolKeybind = KeybindLoader.RegisterKeybind(Mod, "PipetteTool", "Mouse3");
            ClearPipetteKeybind = KeybindLoader.RegisterKeybind(Mod, "ClearPipette", "Mouse2");
        }

        public override void Unload()
        {
            PipetteToolKeybind = null;
            ClearPipetteKeybind = null;
        }
    }
}
