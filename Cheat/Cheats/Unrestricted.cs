using EgguWare.Attributes;
using EgguWare.Menu.Tabs;
using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace EgguWare.Cheats
{
    [Comp]
    public class Unrestricted : MonoBehaviour
    {
        FieldInfo standSpeed = typeof(PlayerMovement).GetField("SPEED_STAND", BindingFlags.NonPublic | BindingFlags.Static);
        FieldInfo sprintSpeed = typeof(PlayerMovement).GetField("SPEED_SPRINT", BindingFlags.NonPublic | BindingFlags.Static);
        FieldInfo proneSpeed = typeof(PlayerMovement).GetField("SPEED_PRONE", BindingFlags.NonPublic | BindingFlags.Static);
        FieldInfo jumpHeight = typeof(PlayerMovement).GetField("JUMP", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly float SPEED_STAND = 4.5f;
		private static readonly float SPEED_PRONE = 1.5f;
		private static readonly float SPEED_SPRINT = 7f;
        private static readonly float JUMP = 7f;

        void Update()
        {
            if (!Provider.isConnected)
                G.UnrestrictedMovement = false;
        }

        void FixedUpdate()
        {
            PlayerFlight();
            if (G.UnrestrictedMovement)
            {
                standSpeed.SetValue(sprintSpeed, G.Settings.MiscOptions.RunspeedMult);
                proneSpeed.SetValue(sprintSpeed, G.Settings.MiscOptions.RunspeedMult);
                sprintSpeed.SetValue(sprintSpeed, G.Settings.MiscOptions.RunspeedMult);
                jumpHeight.SetValue(jumpHeight, G.Settings.MiscOptions.JumpMult);
            }
            else
            {
                standSpeed.SetValue(sprintSpeed, SPEED_STAND);
                proneSpeed.SetValue(sprintSpeed, SPEED_PRONE);
                sprintSpeed.SetValue(sprintSpeed, SPEED_SPRINT);
                jumpHeight.SetValue(jumpHeight, JUMP);
            }
        }

        public static void PlayerFlight()
        {
            if (Player.player == null)
                return;
            Player plr = Player.player;

            if (!G.Settings.MiscOptions.PlayerFlight)
            {
                ItemCloudAsset asset = plr.equipment.asset as ItemCloudAsset;
                plr.movement.itemGravityMultiplier = asset?.gravity ?? 1;
                return;
            }

            plr.movement.itemGravityMultiplier = 0;

            float multiplier = G.Settings.MiscOptions.PlayerFlightSpeedMult;

            if (Input.GetKey(KeyCode.Space))
                plr.transform.position += plr.transform.up / 5 * multiplier;

            if (Input.GetKey(KeyCode.LeftControl))
                plr.transform.position -= plr.transform.up / 5 * multiplier;

            if (Input.GetKey(KeyCode.W))
                plr.transform.position += plr.transform.forward / 5 * multiplier;

            if (Input.GetKey(KeyCode.S))
                plr.transform.position -= plr.transform.forward / 5 * multiplier;

            if (Input.GetKey(KeyCode.A))
                plr.transform.position -= plr.transform.right / 5 * multiplier;

            if (Input.GetKey(KeyCode.D))
                plr.transform.position += plr.transform.right / 5 * multiplier;
        }
    }
}
