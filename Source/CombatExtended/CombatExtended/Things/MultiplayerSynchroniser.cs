using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace CombatExtended.CombatExtended.Things
{
    class MultiplayerSynchroniser : Thing
    {
        public static bool IsActive = false;

        public static void SyncTryStartReload(string thingID, string ammoType)
        {
            Pawn pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault(p => p.ThingID == thingID);
            AmmoDef ammo = ammoType == "" ? null : DefDatabase<AmmoDef>.GetNamedSilentFail(ammoType);
            Building_TurretGunCE building;
            CompAmmoUser compAmmo;
            if (pawn != null)
            {
                compAmmo = pawn.equipment.Primary.TryGetComp<CompAmmoUser>();
                if (compAmmo != null && ammo != null)
                {
                    compAmmo.SelectedAmmo = ammo;
                    compAmmo.TryStartReload();
                    return;
                }
            }
            else
            {
                building = Find.Maps.First().listerBuildings.AllBuildingsColonistOfClass<Building_TurretGunCE>().FirstOrDefault(t => t.ThingID == thingID);
                if (building == null)
                {
                    Log.Message("Reloadfailed - pawn or building not found " + thingID);
                    return;
                }
                compAmmo = building.CompAmmo;
                if (compAmmo != null && ammo != null)
                {
                    compAmmo.SelectedAmmo = ammo;
                    compAmmo.turret.TryOrderReload();
                    return;
                }
            }
            if (ammoType == "" && compAmmo != null)
            {
                compAmmo.TryUnload();
            }
        }
    }
}
