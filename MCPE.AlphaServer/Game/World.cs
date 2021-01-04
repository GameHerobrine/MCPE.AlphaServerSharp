﻿using MCPE.AlphaServer.Packets;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MCPE.AlphaServer.Game {
    public class World {
        public static World The;
        public Server Server => Server.The;

        public List<Entity> Entities = new List<Entity>();
        public List<UdpConnection> Players = new List<UdpConnection>();
        public Dictionary<string, object> Metadata = new Dictionary<string, object>();

        public int LastEID; // Last Entity ID

        public UdpConnection GetPlayerByName(string name) => Players.FirstOrDefault(P => P.Player?.Username == name);
        public async Task AddPlayer(UdpConnection toAdd) {
            var newPlayer = Server.Clients[toAdd.EndPoint];

            foreach (var P in Players) {
                await Server.Send(P, new AddPlayerPacket(newPlayer.Player));
            }

            Players.Add(newPlayer);
        }


        public async Task MovePlayer(UdpConnection toMove, Vector3 position, float pitch, float yaw) {
            var player = Server.Clients[toMove.EndPoint];

            player.Player.Position = position;

            //// Test
            await Server.Send(player, new MovePlayerPacket(position, player.Player.EID, new Vector3(pitch, yaw + 1f, 0f)));

            // for (int i = 0; i < 10; i++) {
            //     for (float f = 95; f < 105; f++) {
            //         for (float g = 95; g < 105; g++) {
            //             await Server.Send(player, new MovePlayerPacket(new Vector3(f, 80, g), i, new Vector3(1f, 1f, 0f)));
            //         }
            //     }
            // }

            foreach (var P in Players) {
                if (P.Player.CID == player.Player.CID)
                    continue;
                await Server.Send(P, new MovePlayerPacket(position, P.Player.EID, new Vector3(pitch, yaw, 0f)));
            }
        }
    }
}