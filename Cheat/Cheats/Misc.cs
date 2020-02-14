using EgguWare.Attributes;
using EgguWare.Utilities;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Cheats
{
    [Comp]
    public class Misc : MonoBehaviour
    {
        public static Misc instance;
        public Rect VanishPlayerRect = new Rect(Screen.width - 395, 50, 200, 100);

        void Start() => instance = this;
        void Update()
        {
            if (Player.player)
            {
                Player.player.look.isOrbiting = G.Settings.MiscOptions.FreeCam;
                Player.player.look.isTracking = G.Settings.MiscOptions.FreeCam;
            }

            if (G.Settings.MiscOptions.Spam && !String.IsNullOrEmpty(G.Settings.MiscOptions.SpamText) && !PlayerLifeUI.chatting) // lets the player type in chat while spamming
                ChatManager.instance.channel.send("askChat", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, (byte)EChatMode.GLOBAL, G.Settings.MiscOptions.SpamText);
        }

        void OnGUI()
        {
            if (!G.Settings.MiscOptions.ShowVanishPlayers || G.BeingSpied || !Provider.isConnected || !(Provider.clients.Count > 0))
                return;
            GUI.skin = AssetUtilities.Skin;
            Rect tempRect = GUILayout.Window(7, VanishPlayerRect, VanishPlayerWindow, "Vanished Players");
            VanishPlayerRect.x = tempRect.x;
            VanishPlayerRect.y = tempRect.y;
        }

        void VanishPlayerWindow(int winid)
        {
            GUILayout.Space(0);
            foreach (SteamPlayer player in Provider.clients)
                if (Vector3.Distance(player.player.transform.position, Vector3.zero) < 10)
                    GUILayout.Label(player.playerID.characterName);
            GUI.DragWindow();
        }

        void FixedUpdate()
        {
            if (Player.player && Provider.isConnected)
            {
                InteractableVehicle car = Player.player?.movement?.getVehicle();
                if (car != null && car && Provider.isConnected && !Provider.isLoading)
                {
                    Rigidbody rigidbody = car.GetComponent<Rigidbody>();
                    if (G.Settings.MiscOptions.VehicleNoClip)
                    {
                        rigidbody.constraints = RigidbodyConstraints.None;
                        rigidbody.freezeRotation = false;
                        rigidbody.useGravity = false;
                        rigidbody.isKinematic = true;
                        Transform transform = car.transform;

                        if (Input.GetKey(KeyCode.W))
                            rigidbody.MovePosition(transform.position + transform.forward * (car.asset.speedMax * Time.fixedDeltaTime));

                        if (Input.GetKey(KeyCode.S))
                            rigidbody.MovePosition(transform.position - transform.forward * (car.asset.speedMax * Time.fixedDeltaTime));

                        if (Input.GetKey(KeyCode.A))
                            transform.Rotate(0f, -2f, 0f);

                        if (Input.GetKey(KeyCode.D))
                            transform.Rotate(0f, 2f, 0f);

                        if (Input.GetKey(KeyCode.UpArrow))
                            transform.Rotate(-1.5f, 0f, 0f);

                        if (Input.GetKey(KeyCode.DownArrow))
                            transform.Rotate(1.5f, 0f, 0f);

                        if (Input.GetKey(KeyCode.LeftArrow))
                            transform.Rotate(0f, 0f, 1.5f);

                        if (Input.GetKey(KeyCode.RightArrow))
                            transform.Rotate(0f, 0f, -1.5f);

                        if (Input.GetKey(KeyCode.Q))
                            transform.position = transform.position + new Vector3(0f, .2f, 0f);

                        if (Input.GetKey(KeyCode.E))
                            transform.position = transform.position - new Vector3(0f, .2f, 0f);
                    }
                    else
                    {
                        rigidbody.useGravity = true;
                        rigidbody.isKinematic = false;
                    }
                }
            }
        }
    }
}
