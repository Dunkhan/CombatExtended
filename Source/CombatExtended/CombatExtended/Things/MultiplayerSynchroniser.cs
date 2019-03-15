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
			AmmoDef ammo = DefDatabase<AmmoDef>.GetNamedSilentFail(ammoType);
			if (ammo == null)
			{
				Log.Message("Reloadfailed - null ammo");
				return;
			}
			Pawn pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault(p => p.ThingID == thingID);
			Building_TurretGunCE building;
			if (pawn != null)
			{
				CompAmmoUser compAmmo = pawn.equipment.Primary.TryGetComp<CompAmmoUser>();
				if (compAmmo == null)
				{
					Log.Message("Reloadfailed - no ammo comp on pawn");
					return;
				}
				compAmmo.SelectedAmmo = ammo;
				compAmmo.TryStartReload();
			}
			else
			{
				building = Find.Maps.First().listerBuildings.AllBuildingsColonistOfClass<Building_TurretGunCE>().FirstOrDefault(t => t.ThingID == thingID);
				if (building == null)
				{
					Log.Message("Reloadfailed - pawn or building not found " + thingID);
					return;
				}
				CompAmmoUser compAmmo = building.CompAmmo;
				if (compAmmo == null)
				{
					Log.Message("Reloadfailed - turret ammo comp not found");
					return;
				}
				compAmmo.SelectedAmmo = ammo;
				compAmmo.turret.TryOrderReload();
			}
		}
	}
}
