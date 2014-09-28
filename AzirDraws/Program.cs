using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;

using SharpDX;
using Color = System.Drawing.Color;

namespace AzirDraws
{
    class Program
    {
        private static readonly string champName = "Azir";
        private static readonly Obj_AI_Hero player = ObjectManager.Player;
        private static readonly List<Spell> spellList = new List<Spell>();
        private static Spell Q, W, E, R;
        private static Menu menu;

        public static void Main(string[] args)
        {
            // Register load event
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            // Validate champion
            if (player.ChampionName != champName)
                return;

            // Initialize spells
            Q = new Spell(SpellSlot.Q, 750);
            W = new Spell(SpellSlot.W, 420);
            E = new Spell(SpellSlot.E, 1000);
            R = new Spell(SpellSlot.R, 500);

            // Add to spell list
            spellList.AddRange(new[] { Q, W, E, R });

            // Setup menu
            SetuptMenu();

            // Register additional events
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            // Spell ranges
            foreach (var spell in spellList)
            {
                var circleEntry = menu.SubMenu("drawings").Item("drawRange" + spell.Slot.ToString()).GetValue<Circle>();
                if (circleEntry.Active)
                {
                    if (!player.IsDead)
                    {   
                        if(!spell.IsReady())
                        {
                            Utility.DrawCircle(player.Position, spell.Range, Color.Red);
                        }

                        if(spell.IsReady())
                        {
                            Utility.DrawCircle(player.Position, spell.Range, Color.Green);
                        }
                    }
                }
            }
        }

        private static void SetuptMenu()
        {
            // Create menu
            menu = new Menu(champName, champName, true);

            // Drawings
            Menu drawings = new Menu("Drawings", "drawings");
            drawings.AddItem(new MenuItem("drawRangeQ", "Q range").SetValue(true));
            drawings.AddItem(new MenuItem("drawRangeW", "W range").SetValue(true));
            drawings.AddItem(new MenuItem("drawRangeE", "E range").SetValue(true));
            drawings.AddItem(new MenuItem("drawRangeR", "R range").SetValue(true));
            menu.AddSubMenu(drawings);

            // Finalize menu
            menu.AddToMainMenu();
        }
    }
}
