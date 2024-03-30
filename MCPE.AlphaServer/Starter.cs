﻿using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SpoongePE.Core.Game;
using SpoongePE.Core.Game.Generator;
using SpoongePE.Core.Network;
using SpoongePE.Core.RakNet;
using SpoongePE.Core.Utils;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace SpoongePE.Core;

internal static class Starter
{
    private static async Task Main(string[] _)
    {
        //#if DEBUG
        // Directory.SetCurrentDirectory("work");
        //#endif
        ServerProperties prop = new YmlProp().LoadServerProp();
        var mainWorld = new World(666);
        Block.init();
        Logger.LogBackend = new LoggerConfiguration()
        .WriteTo.Console(theme: SystemConsoleTheme.Colored)
        .MinimumLevel.Debug()
        .CreateLogger();
        FlatWorldGenerator.generateChunks(mainWorld);

        //   mainWorld.PrintEntitiesData();
        Console.WriteLine("Level Data:");
        mainWorld.PrintLevelData();



        Logger.Info("SpoongePE.Core starting.");
        RakNetServer.Properties = prop;
        RakNetServer rak = new RakNetServer(prop.serverPort);
        GameServer handler = new GameServer(mainWorld);
        rak.Start(handler);
        AppDomain.CurrentDomain.ProcessExit += (s, e) => Shutdown(rak, handler);

        Logger.Info("SpoongePE.Core started.");

        await Task.Delay(Timeout.Infinite);
    }

    private static async void Shutdown(RakNetServer rak, GameServer handler)
    {
        handler.ServerWorld.KickAll();
      //  await Task.Delay(100);
        rak.Stop();

       // handler.ServerWorld.World.
        Logger.Info("Server is stopped!");
    }
}