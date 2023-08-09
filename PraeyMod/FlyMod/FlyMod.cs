using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using NoMatter.Data;
using NoMatter.Manager;
using NoMatter.Objects;
using UnityEngine;

namespace FlyPraeyMod
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class FlyMod : BaseUnityPlugin
    {
        private const string ModName = "FlyMod";
        private const string ModVersion = "1.0";
        private const string ModGUID = "com.RedDude.FlyMod";
        
        private GameObject freeCamera;
        private bool cameraActive;
        private bool cameraKeyUp;

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
	        
	        FlyCamera(actor);
        }

        private void FlyCamera(Player player)
        {
	        if (freeCamera == null)
	        {
		        cameraActive = false;
	        }

	        var currentActiveCamera = cameraActive;
	        if (Input.GetKeyUp(KeyCode.RightShift) && !cameraKeyUp)
	        {
		        cameraKeyUp = true;

		        if (freeCamera == null)
		        {
			        freeCamera = new GameObject();
			        // freeCamera.AddComponent<Camera>();
			        freeCamera.AddComponent<DebugCamera>();
		        }
		        // else {
		        //  // if (!freeCamera) return;
		        //
		        // }

		        var cameraControl = freeCamera.GetComponent<DebugCamera>();
		      
		        currentActiveCamera = !cameraActive;
		        if (currentActiveCamera)
		        {
			        cameraControl.target = player.transform;
			        cameraControl.actor = player;
		        }
		        else
		        {
			        cameraControl.target = null;
			        cameraControl.actor = null;
		        }
		        cameraControl.ToggleActive();
		        var debugViewCameraControl = freeCamera.GetComponent<DebugViewCameraControl>();
		        // player.gameObject.transform.position = cameraControl.target.position;
	        }
	        else
	        {
		        cameraKeyUp = false;
	        }

	        if (currentActiveCamera != cameraActive)
	        {
		        Debug.Log("cameraActive : " + cameraActive);
		        cameraActive = currentActiveCamera;
		        
		        if(!cameraActive){
			        player.invincible = false;
			        // player.IgnoreAllInputExceptCamera(false);
			        player.GetComponent<Rigidbody>().useGravity = true;
				}
	        }
        }
    }
}