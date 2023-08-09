using System.Reflection;
using BepInEx;
using HarmonyLib;
using NoMatter.Data;
using NoMatter.Manager;
using NoMatter.Objects;

namespace EatWhenFullInventoryPraeyMod
{
	[BepInPlugin(ModGUID, ModName, ModVersion)]
	public class EatWhenFullInventory : BaseUnityPlugin
	{
		private const string ModName = "EatWhenFullInventory";
		private const string ModVersion = "1.0";
		private const string ModGUID = "com.RedDude.EatWhenFullInventoryPraeyMod";

		private FieldInfo _nearestInteractableItem;

		private void Update()
		{
			if (DataManager.Instance == null)
				return;

			var playerData = DataManager.Instance.PlayerData;

			if (playerData == null)
				return;

			var actor = playerData.Actor;
			if (actor == null)
			{
				return;
			}

			if (_nearestInteractableItem == null)
				_nearestInteractableItem = AccessTools.Field(typeof(Player), "_nearestInteractableItem");
			if (_nearestInteractableItem == null)
				return;

			var value = (IInteractable) _nearestInteractableItem.GetValue(actor);
			if (value == null)
				return;
			CheckEatWhenFullInventory(actor, value);
		}

		private static void CheckEatWhenFullInventory(Player actor, IInteractable interactable)
		{

			var srconsumable = interactable.Data.SDRecord as SRConsumable;
			if (srconsumable != null && srconsumable.subCategory == SubCategory.Food &&
			    interactable.CanInteract != null)
			{
				DataManager instance = DataManager.Instance;
				instance.WorldData.Remove(interactable.Data);
				interactable.Data.Consume();
				// if (itemData != null)
				// {
				// EquipLocationType lastEquip = this._lastEquip;
				// this._lastEquip = EquipLocationType.Invalid;

				// }
			}
		}
	}


	// [HarmonyPatch(typeof(DLCManager), "ProcessDLC")]
        // public static class PatchDLC
        // {
	       //  public static void Postfix(DLCManager __instance)
	       //  {
		      //   // AccessTools.Method(typeof(DLCManager), "HasElitePouckPack").Invoke(__instance, new object[] {} );
		      //   // AccessTools.Method(typeof(DLCManager), "HasRavenPack").Invoke(__instance, new object[] {} );
		      //   // AccessTools.Method(typeof(DLCManager), "HasOverlordPack").Invoke(__instance, new object[] {} );
		      //   // AccessTools.Method(typeof(DLCManager), "HasEliteOutfitPack").Invoke(__instance, new object[] {} );
		      //   // AccessTools.Method(typeof(DLCManager), "HasWolfPack").Invoke(__instance, new object[] {} );
		      //   // AccessTools.Method(typeof(DLCManager), "HasHalloweenPack").Invoke(__instance, new object[] {} );
	       //  }
        // }
}