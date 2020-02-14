using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace EgguWare.Overrides
{
    public class hkPlayerInput
    {
        public static int count = 0;
        public static uint Clock = 0;
        public static byte Analog;
        public static int sequence = 0;
        public static int consumed = 0;
        public static uint buffer = 0;
        public static float Yaw, Pitch;
        public static List<PlayerInputPacket> clientsidePackets = new List<PlayerInputPacket>();
        public static FieldInfo SimField =
            typeof(PlayerInput).GetField("_simulation", BindingFlags.NonPublic | BindingFlags.Instance);
        public static uint GetSim(PlayerInput instance) =>
           (uint)SimField.GetValue(instance);

        public static void SetSim(PlayerInput instance, uint value) =>
            SimField.SetValue(instance, value);

        public static FieldInfo TickField = 
		    typeof(PlayerInput).GetField("_tick", BindingFlags.NonPublic | BindingFlags.Instance);
	    
	    public static float GetTick(PlayerInput instance) =>
		    (float)TickField.GetValue(instance);
	    
	    public static void SetTick(PlayerInput instance, float value) =>
		    TickField.SetValue(instance, value); 

        public static void OV_FixedUpdate(PlayerInput instance)
        {
            Player player = Player.player;
            if (count % 6 == 0u)
            {
                SetTick(instance, Time.realtimeSinceStartup);
                instance.keys[0] = player.movement.jump;
                instance.keys[1] = player.equipment.primary;
                instance.keys[2] = player.equipment.secondary;
                instance.keys[3] = player.stance.crouch;
                instance.keys[4] = player.stance.prone;
                instance.keys[5] = player.stance.sprint;
                instance.keys[6] = player.animator.leanLeft;
                instance.keys[7] = player.animator.leanRight;
                instance.keys[8] = false;
                for (int i = 0; i < (int)ControlsSettings.NUM_PLUGIN_KEYS; i++)
                {
                    int num = instance.keys.Length - (int)ControlsSettings.NUM_PLUGIN_KEYS + i;
                    instance.keys[num] = Input.GetKey(ControlsSettings.getPluginKeyCode(i));
                }
                Analog = (byte)(player.movement.horizontal << 4 | player.movement.vertical);
                player.life.simulate(instance.simulation);
                player.stance.simulate(instance.simulation, player.stance.crouch, player.stance.prone, player.stance.sprint);
                Pitch = player.look.pitch;
                Yaw = player.look.yaw;
                player.movement.simulate(instance.simulation, 0, (int)(player.movement.horizontal - 1), (int)(player.movement.vertical - 1), player.look.look_x, player.look.look_y, player.movement.jump, player.stance.sprint, Vector3.zero, PlayerInput.RATE);
                sequence++;
                if (player.stance.stance == EPlayerStance.DRIVING)
                {
                    clientsidePackets.Add(new DrivingPlayerInputPacket());
                }
                else
                {
                    clientsidePackets.Add(new WalkingPlayerInputPacket());
                }
                PlayerInputPacket playerInputPacket = clientsidePackets[clientsidePackets.Count - 1];
                playerInputPacket.sequence = sequence;
                playerInputPacket.recov = instance.recov;

                player.equipment.simulate(instance.simulation, player.equipment.primary, player.equipment.secondary, player.stance.sprint);
                player.animator.simulate(instance.simulation, player.animator.leanLeft, player.animator.leanRight);
                buffer += PlayerInput.SAMPLES;
                SetSim(instance, GetSim(instance) + 1);
            }
            if (consumed < buffer)
            {
                consumed++;
                player.equipment.tock(Clock);
                Clock++;
            }
            if (consumed == buffer && clientsidePackets.Count > 0 && !Provider.isServer)
            {
                ushort num2 = 0;
                for (byte b = 0; b < instance.keys.Length; b++)
				    if (instance.keys[b])
					    num2 |= (ushort) (1 << b);
                PlayerInputPacket playerInputPacket2 = clientsidePackets[clientsidePackets.Count - 1];
                playerInputPacket2.keys = num2;
                if (playerInputPacket2 is DrivingPlayerInputPacket)
                {
                    DrivingPlayerInputPacket drivingPlayerInputPacket = playerInputPacket2 as DrivingPlayerInputPacket;
                    InteractableVehicle vehicle = player.movement.getVehicle();
                    if (vehicle != null)
                    {
                        Transform transform = vehicle.transform;
                        if (vehicle.asset.engine == EEngine.TRAIN)
                        {
                            drivingPlayerInputPacket.position = new Vector3(vehicle.roadPosition, 0f, 0f);
                        }
                        else
                        {
                            drivingPlayerInputPacket.position = transform.position;
                        }
                        drivingPlayerInputPacket.angle_x = MeasurementTool.angleToByte2(transform.rotation.eulerAngles.x);
                        drivingPlayerInputPacket.angle_y = MeasurementTool.angleToByte2(transform.rotation.eulerAngles.y);
                        drivingPlayerInputPacket.angle_z = MeasurementTool.angleToByte2(transform.rotation.eulerAngles.z);
                        drivingPlayerInputPacket.speed = (byte)(Mathf.Clamp(vehicle.speed, -100f, 100f) + 128f);
                        drivingPlayerInputPacket.physicsSpeed = (byte)(Mathf.Clamp(vehicle.physicsSpeed, -100f, 100f) + 128f);
                        drivingPlayerInputPacket.turn = (byte)(vehicle.turn + 1);
                    }
                }
                else
                {
                    WalkingPlayerInputPacket walkingPlayerInputPacket = playerInputPacket2 as WalkingPlayerInputPacket;
                    walkingPlayerInputPacket.analog = Analog;
                    walkingPlayerInputPacket.position = instance.transform.localPosition;
                    walkingPlayerInputPacket.yaw = Yaw;
                    walkingPlayerInputPacket.pitch = Pitch;
                }
                instance.channel.openWrite();
                instance.channel.write((byte)clientsidePackets.Count);
                foreach (PlayerInputPacket playerInputPacket3 in clientsidePackets)
                {
                    if (playerInputPacket3 is DrivingPlayerInputPacket)
                    {
                        instance.channel.write(1);
                    }
                    else
                    {
                        instance.channel.write(0);
                    }
                    playerInputPacket3.write(instance.channel);
                }
                instance.channel.closeWrite("askInput", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT);
            }
            count++;
        }
    }
}
