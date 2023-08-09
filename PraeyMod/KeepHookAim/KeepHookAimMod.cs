using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using NoMatter.Data;
using NoMatter.Manager;
using NoMatter.Objects;
using UnityEngine;

namespace UnlockArmorsPraeyMod
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class KeepHookAimMod : BaseUnityPlugin
    {
        private const string ModName = "KeepHookAimMod";
        private const string ModVersion = "1.0";
        private const string ModGUID = "com.RedDude.KeepHookAimMod";
        
        private FieldInfo _shotStage;
        private bool autoFeedbackHook = true;
        private bool quickShot = false;
        
        private FieldInfo _controls;

        private void Update()
        {
	        if(DataManager.Instance == null)
		        return;

	        var playerData = DataManager.Instance.PlayerData;
	        
	        if(playerData == null)
		        return;

	        var actor = playerData.Actor;
	        if (actor == null)
	        {
		        return;
	        }
	        
	        if (Input.GetKeyDown(KeyCode.Alpha0))
	        {
		        autoFeedbackHook = !autoFeedbackHook;
		        Debug.Log("autoFeedbackHook: " + autoFeedbackHook);
	        }
		        
	        
	        if(autoFeedbackHook)
				AutoFeedbackHook(playerData);
        }

        private void AutoFeedbackHook(PlayerData playerData)
        {
	        var ONLY_WHEN_HOOK_EQUIPPED = 1;
	        var ALL_THE_TIME = 2;
	        var autoFeedbackHookType = ONLY_WHEN_HOOK_EQUIPPED;
	        
	        if (_shotStage == null)
		        _shotStage = AccessTools.Field(typeof(GrapplingHookLogic), "_shotStage");
	        
	        var crosshairPosition = Camera.main.ScreenToWorldPoint(UIManager.Inst.InGameHUD.crosshairLockRect.position);
	        var player = playerData.Actor;
	        var distance = Vector3.Distance(crosshairPosition, player.gameObject.transform.position);
	        
	        var shotStateValue =
		        (GrapplingHookLogic.ShotStage) _shotStage.GetValue(player.GrapplingHookLogic);
	        {
		        // UIManager.Inst.InGameHUD.ToggleCrosshair(false);
		        // UIManager.Inst.InGameHUD.ToggleCrosshairUpdate(false);
		        // playerData.Actor.GrapplingHookLogic.Equip
		        
		        // if (this._lastEquipmentHand == Player.Equipment.Melee)
			        
		        // Debug.Log(playerData.Actor.);

		        if (!player.GrapplingHookLogic.Aim)
		        {
			        if (player.CurrentEquippedWeapon == Player.Equipment.GrapplingHook)
			        {
				        if (shotStateValue == GrapplingHookLogic.ShotStage.None)
				        {
					        if (distance < 7.5f) 
					        {
						        // Debug.Log("firstShow");
						        _shotStage.SetValue(player.GrapplingHookLogic,
							        GrapplingHookLogic.ShotStage.CheckShot);
					        }
				        }
			        }
			        
			        if (shotStateValue == GrapplingHookLogic.ShotStage.CheckShotAllowed)
			        {
				        if (distance > 7.5f && player.IsClimbOrStab)
				        {
					        Debug.Log("hiding");
					        _shotStage.SetValue(player.GrapplingHookLogic, GrapplingHookLogic.ShotStage.None);
					        player.GrapplingHookLogic.ToggleOffGrapplingHookCrosshair();
				        }
				        else
				        {
					        Debug.Log("showing");
					        // _shotStage.SetValue(playerData.Actor.GrapplingHookLogic,
						        // GrapplingHookLogic.ShotStage.CheckShot);
					        ToggleOnGrapplingHookCrosshair(player.GrapplingHookLogic);
					       
					        // playerData.Actor.GrapplingHookLogic.ToggleOffGrapplingHookCrosshair();
				        }
			        }

			        if (player.CurrentEquippedWeapon != Player.Equipment.GrapplingHook &&
			            (shotStateValue == GrapplingHookLogic.ShotStage.CheckShot ||
			             shotStateValue == GrapplingHookLogic.ShotStage.CheckShotAllowed))
			        {
				        // Debug.Log(playerData.Actor.CurrentEquippedWeapon);
				        // Debug.Log("stop");
				       
				        _shotStage.SetValue(player.GrapplingHookLogic, GrapplingHookLogic.ShotStage.None);
				        player.GrapplingHookLogic.ToggleOffGrapplingHookCrosshair();
				        // UIManager.Inst.InGameHUD.ToggleCrosshairUpdate(false);
				        // UIManager.Inst.InGameHUD.ToggleCrosshair(false);
			        }

			        // QuickShot(player);
		        }
	        }
        }

        private void QuickShot(Player player)
        {
	        if(!quickShot)
		        return;
	        
	        // if (_controls == null)
		       //  _controls = AccessTools.Field(typeof(InGameMode), "_controls");

	       var shotStateValue =
		       (GrapplingHookLogic.ShotStage) _shotStage.GetValue(player.GrapplingHookLogic);
		       
	        var isAimButton = false;

	        var generalAction = Controls.Inst.GeneralAction;
	        var validAimButton = player.CurrentAction != AnimatorAction.Roll && player.TryAction != AnimatorAction.OnHit &&
	                             player.TryAction != AnimatorAction.PushFlyBackwards &&
	                             player.TryAction != AnimatorAction.PushFlyForward &&
	                             player.TryAction != AnimatorAction.PushFlyLeft &&
	                             player.TryAction != AnimatorAction.PushFlyRight && !player.DashToggle;
	        if (validAimButton)
	        {
		        if (generalAction.Aim.IsPressed)
		        {
			        isAimButton = true;
		        }

		        if (generalAction.Aim.WasReleased)
		        {
			        isAimButton = true;
		        }
	        }

	        if (isAimButton)
		        Debug.Log("isAimButton");

	        if (player.CurrentEquippedWeapon != Player.Equipment.GrapplingHook && player.IsClimbOrStab && isAimButton)
	        {
		        player.GrapplingHookLogic.Equip = true;
		        return;
	        }

	        if (player.CurrentEquippedWeapon == Player.Equipment.GrapplingHook && 
		        shotStateValue == GrapplingHookLogic.ShotStage.CheckShotAllowed && validAimButton &&
		        Input.GetKeyDown(KeyCode.Q) || player.IsClimbOrStab && isAimButton)
	        {
		        player.ReleaseClimb();
		        var checkShot = AccessTools.Method(typeof(GrapplingHookLogic), "DoAim");
		        checkShot.Invoke(player.GrapplingHookLogic, new object[] { });
		        player.GrapplingHookLogic.FireGrapplingHook();
	        }
        }

        private void LateUpdate()
        {
        if (_shotStage == null)
		      return;
	         
	       var playerData = DataManager.Instance.PlayerData;
	       if(playerData == null)
		      return;
        
	       //  
	       //  var crosshairPosition = Camera.main.ScreenToWorldPoint(UIManager.Inst.InGameHUD.crosshairLockRect.position);
	       //  var distance = Vector3.Distance(crosshairPosition, playerData.Actor.gameObject.transform.position);
	       //  
	       var shotStateValue =
		      (GrapplingHookLogic.ShotStage) _shotStage.GetValue(playerData.Actor.GrapplingHookLogic);
	       {
		       if (!playerData.Actor.GrapplingHookLogic.Aim)
		       {
			       if (shotStateValue == GrapplingHookLogic.ShotStage.CheckShotAllowed)
			       {
				       UIManager.Inst.InGameHUD.UpdateCrosshairColor(Color.yellow);
				       UIManager.Inst.InGameHUD.ToggleCrosshairUpdate(true);
			       }
		       }

		       //   // UIManager.Inst.InGameHUD.ToggleCrosshair(false);
		      //   // UIManager.Inst.InGameHUD.ToggleCrosshairUpdate(false);
		      //   // playerData.Actor.GrapplingHookLogic.Equip
		      //   
		      //   // if (this._lastEquipmentHand == Player.Equipment.Melee)
			     //    
		      //   // Debug.Log(playerData.Actor.);
        //
		      //   if (playerData.Actor.GrapplingHookLogic.Aim) return;
        //
		      //
	       }
        }

        public void ToggleOnGrapplingHookCrosshair(GrapplingHookLogic actorGrapplingHookLogic)
        {
	        var shotState = (GrapplingHookLogic.ShotStage)_shotStage.GetValue(actorGrapplingHookLogic);
	        if (shotState != GrapplingHookLogic.ShotStage.None)
		        return;
	        
			// UIManager.Inst.InGameHUD.m_Crosshair.fadeInOnActiveCurve = actorGrapplingHookLogic.fadeInCrosshairCurve;
	        UIManager.Inst.InGameHUD.ToggleCrosshair(true);
	        UIManager.Inst.InGameHUD.ToggleCrosshairUpdate(true);
        }

    }
}