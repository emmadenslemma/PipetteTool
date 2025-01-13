using PipetteTool.Common.Systems;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PipetteTool.Common.Players
{
    public class PipetteToolPlayer : ModPlayer
    {
        private int lastSelectedItem = -1;

        private Item[] Inventory => Player.inventory;
        private int SelectedItem
        {
            get => Player.selectedItem;
            set => Player.selectedItem = value;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.PipetteToolKeybind.JustPressed
                && SelectedItem != 58)
            {
                var tile = GetTileAtCursor();

                var itemSlot = GetItemSlotForTile(tile);

                if (itemSlot != -1)
                {
                    SelectItem(itemSlot);
                }
            }

            if (ShouldDeselectItem(triggersSet))
            {
                DeselectItem();
            }
        }

        public override bool PreItemCheck()
        {
            if (lastSelectedItem != -1 && Inventory[SelectedItem].stack == 0)
            {
                DeselectItem();
            }

            return true;
        }

        private int GetItemSlotForTile(Tile tile)
        {
            if (tile.HasTile)
            {
                ushort tileId = tile.TileType;

                int itemSlot = GetItemSlotForTileID(tileId);

                if (itemSlot != -1)
                {
                    return itemSlot;
                }
                else
                {
                    if (TileID.Sets.Conversion.Grass[tileId])
                    {
                        return GetItemSlotForTileID(TileID.Dirt);
                    }
                    else if (TileID.Sets.Conversion.JungleGrass[tileId] || TileID.Sets.Conversion.MushroomGrass[tileId])
                    {
                        return GetItemSlotForTileID(TileID.Mud);
                    }
                }
            }
            else if (tile.WallType > 0)
            {
                ushort wallId = tile.WallType;

                return GetItemSlotForWallID(wallId);
            }

            return -1;
        }

        private int GetItemSlotForTileID(ushort tileId)
        {
            // Include coin slots (50-53) and ammunition (54-57 when checking for placeable tiles
            for (int i = 0; i < 58; i++)
            {
                if (Inventory[i].createTile == tileId)
                {
                    return i;
                }
            }

            return -1;
        }

        private int GetItemSlotForWallID(ushort wallId)
        {
            for (int i = 0; i < 50; i++)
            {
                if (Inventory[i].createWall == wallId)
                {
                    return i;
                }
            }

            return -1;
        }

        private static bool ShouldDeselectItem(TriggersSet triggersSet)
        {
            return KeybindSystem.ClearPipetteKeybind.JustPressed
                || triggersSet.MouseRight
                || triggersSet.SmartSelect
                || triggersSet.Hotbar1
                || triggersSet.Hotbar2
                || triggersSet.Hotbar3
                || triggersSet.Hotbar4
                || triggersSet.Hotbar5
                || triggersSet.Hotbar6
                || triggersSet.Hotbar7
                || triggersSet.Hotbar8
                || triggersSet.Hotbar9
                || triggersSet.Hotbar10
                || triggersSet.HotbarMinus
                || triggersSet.HotbarPlus
                || Player.GetMouseScrollDelta() != 0;
        }

        private void SelectItem(int itemSlot)
        {
            if (itemSlot >= 10 && lastSelectedItem == -1)
            {
                lastSelectedItem = SelectedItem;
            }
            else if (itemSlot < 10)
            {
                lastSelectedItem = -1;
            }

            SelectedItem = itemSlot;
        }

        private void DeselectItem()
        {
            if (lastSelectedItem != -1)
            {
                SelectedItem = lastSelectedItem;
                lastSelectedItem = -1;
            }
        }

        private Tile GetTileAtCursor()
        {
            var (tX, tY) = GetTilePositionAtCursor();
            var tile = Main.tile[tX, tY];

            return tile;
        }

        private (int, int) GetTilePositionAtCursor()
        {
            int x = (int)((Main.mouseX + Main.screenPosition.X) / 16f);
            int y = Entity.gravDir == 1
                ? (int)((Main.mouseY + Main.screenPosition.Y) / 16f)
                : (int)((Main.screenPosition.Y + Main.screenHeight - Main.mouseY) / 16f);

            return (x, y);
        }
    }
}
