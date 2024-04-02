﻿
using SpoongePE.Core.NBT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpoongePE.Core.Utils;
namespace SpoongePE.Core.Game
{
    public class WorldSaver
    {
        // --https://github.com/GameHerobrine/Minecraft013-Server/blob/main/src/net/skidcode/gh/server/world/parser/vanilla/ChunkDataParser.java
        private Chunk[] chunks = new Chunk[256];
        public static int[,] locTable = new int[,] { //TODO understand how it works and what it means
		{0x115, 0x1615, 0x2B15, 0x4015, 0x5515, 0x6A15, 0x7F15, 0x9415, 0xA915, 0xBE15, 0xD315, 0xE815, 0xFD15, 0x11215, 0x12715, 0x13C15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x15115, 0x16615, 0x17B15, 0x19015, 0x1A515, 0x1BA15, 0x1CF15, 0x1E415, 0x1F915, 0x20E15, 0x22315, 0x23815, 0x24D15, 0x26215, 0x27715, 0x28C15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x2A115, 0x2B615, 0x2CB15, 0x2E015, 0x2F515, 0x30A15, 0x31F15, 0x33415, 0x34915, 0x35E15, 0x37315, 0x38815, 0x39D15, 0x3B215, 0x3C715, 0x3DC15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x3F115, 0x40615, 0x41B15, 0x43015, 0x44515, 0x45A15, 0x46F15, 0x48415, 0x49915, 0x4AE15, 0x4C315, 0x4D815, 0x4ED15, 0x50215, 0x51715, 0x52C15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x54115, 0x55615, 0x56B15, 0x58015, 0x59515, 0x5AA15, 0x5BF15, 0x5D415, 0x5E915, 0x5FE15, 0x61315, 0x62815, 0x63D15, 0x65215, 0x66715, 0x67C15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x69115, 0x6A615, 0x6BB15, 0x6D015, 0x6E515, 0x6FA15, 0x70F15, 0x72415, 0x73915, 0x74E15, 0x76315, 0x77815, 0x78D15, 0x7A215, 0x7B715, 0x7CC15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x7E115, 0x7F615, 0x80B15, 0x82015, 0x83515, 0x84A15, 0x85F15, 0x87415, 0x88915, 0x89E15, 0x8B315, 0x8C815, 0x8DD15, 0x8F215, 0x90715, 0x91C15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x93115, 0x94615, 0x95B15, 0x97015, 0x98515, 0x99A15, 0x9AF15, 0x9C415, 0x9D915, 0x9EE15, 0xA0315, 0xA1815, 0xA2D15, 0xA4215, 0xA5715, 0xA6C15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0xA8115, 0xA9615, 0xAAB15, 0xAC015, 0xAD515, 0xAEA15, 0xAFF15, 0xB1415, 0xB2915, 0xB3E15, 0xB5315, 0xB6815, 0xB7D15, 0xB9215, 0xBA715, 0xBBC15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0xBD115, 0xBE615, 0xBFB15, 0xC1015, 0xC2515, 0xC3A15, 0xC4F15, 0xC6415, 0xC7915, 0xC8E15, 0xCA315, 0xCB815, 0xCCD15, 0xCE215, 0xCF715, 0xD0C15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0xD2115, 0xD3615, 0xD4B15, 0xD6015, 0xD7515, 0xD8A15, 0xD9F15, 0xDB415, 0xDC915, 0xDDE15, 0xDF315, 0xE0815, 0xE1D15, 0xE3215, 0xE4715, 0xE5C15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0xE7115, 0xE8615, 0xE9B15, 0xEB015, 0xEC515, 0xEDA15, 0xEEF15, 0xF0415, 0xF1915, 0xF2E15, 0xF4315, 0xF5815, 0xF6D15, 0xF8215, 0xF9715, 0xFAC15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0xFC115, 0xFD615, 0xFEB15, 0x100015, 0x101515, 0x102A15, 0x103F15, 0x105415, 0x106915, 0x107E15, 0x109315, 0x10A815, 0x10BD15, 0x10D215, 0x10E715, 0x10FC15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x111115, 0x112615, 0x113B15, 0x115015, 0x116515, 0x117A15, 0x118F15, 0x11A415, 0x11B915, 0x11CE15, 0x11E315, 0x11F815, 0x120D15, 0x122215, 0x123715, 0x124C15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x126115, 0x127615, 0x128B15, 0x12A015, 0x12B515, 0x12CA15, 0x12DF15, 0x12F415, 0x130915, 0x131E15, 0x133315, 0x134815, 0x135D15, 0x137215, 0x138715, 0x139C15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x13B115, 0x13C615, 0x13DB15, 0x13F015, 0x140515, 0x141A15, 0x142F15, 0x144415, 0x145915, 0x146E15, 0x148315, 0x149815, 0x14AD15, 0x14C215, 0x14D715, 0x14EC15, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000},
        {0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000}
    };
        public static int CHUNK_HEADER = 71368960;
        private World world;

        public WorldSaver(World w)
        {
            this.world = w;
        }
        public void SaveChunks(FileStream outputFile)
        {


            BinaryWriter writer = new BinaryWriter(outputFile);
            for (int x = 0; x < 32; ++x)
            {
                for (int z = 0; z < 32; ++z)
                {

                    writer.Write(locTable[x, z]);

                }
            }
            // offset 0x1000
            for (int chunkX = 0; chunkX < 16; ++chunkX)
            {
                for (int chunkZ = 0; chunkZ < 16; ++chunkZ)
                {
                    int start = 4096 + (chunkX * 21 * 4096) + (chunkZ * 21 * 16 * 4096);
                    writer.Seek(start, SeekOrigin.Begin);
                    Chunk c = world._chunks[chunkX, chunkZ];

                    writer.Write(new byte[] { 04, 0x41, 01, 00 }); // It's CHUNK_HEADER


                    foreach (var bl in c.BlockData)
                    {
                        writer.Write((byte)(bl));

                    }

                    writer.Write(Chunk.CompressBlockMetadata(c.BlockMetadata));
                    writer.Write(Chunk.CompressBlockMetadata(c.SkyLight));

                    // У нас нет просчёта света по блокам :\
                    writer.Write(Chunk.CompressBlockMetadata(c.BlockLight));

                    for (int x = 0; x < 16; ++x)
                    { //taken from 0.1.0 decomp project (ReMinecraftPE)
                        for (int z = 0; z < 16; ++z)
                        {
                            writer.Write((byte)0xff);
                        }
                    }
                }
            }
            writer.Flush();
        }
        public void SaveLevelDat(FileStream outputFile)
        {
            world._levelDat = new NbtFile();
            NbtCompound levelRootTag = world._levelDat.RootTag;

            levelRootTag.Add(new NbtString("LevelName", world.LevelName));
            levelRootTag.Add(new NbtLong("RandomSeed", world.Seed));

            levelRootTag.Add(new NbtInt("SpawnX", world.SpawnX));
            levelRootTag.Add(new NbtInt("SpawnY", world.SpawnY));
            levelRootTag.Add(new NbtInt("SpawnZ", world.SpawnZ));

            levelRootTag.Add(new NbtLong("Time", world.worldTime));
            outputFile.Seek(8, SeekOrigin.Begin);
            world._levelDat.SaveToStream(outputFile, NbtCompression.GZip);
        }

        public void SaveAll()
        {
            if (world.name.ToLower().Contains("unknown")) return;
            if (!Directory.Exists(Path.Combine("worlds", world.LevelName))) Directory.CreateDirectory(Path.Combine("worlds", world.LevelName));
            Logger.PInfo("Saving " + world.LevelName + "...");
            FileStream chunksDat = File.OpenWrite(Path.Combine("worlds", world.LevelName, "chunks.dat"));
            FileStream levelDat = File.OpenWrite(Path.Combine("worlds", world.LevelName, "level.dat"));

            SaveChunks(chunksDat);
            SaveLevelDat(levelDat);

            chunksDat.Dispose();
            levelDat.Dispose();
            Logger.PInfo(world.LevelName + " is saved!");
        }



        public void LoadChunks(FileStream inputFile)
        {

            BinaryReader chunkReader = new BinaryReader(inputFile);

            int[,] chunkMetadata = Chunk.ReadMetadata(chunkReader);
            world._chunks = new Chunk[16, 16];

            for (var xz = 0; xz < 16 * 16; xz++)
            {
                var x = xz % 16;
                var z = xz / 16;

                var offset = chunkMetadata[x, z];
                if (offset == 0)
                    continue;

                inputFile.Seek(offset, SeekOrigin.Begin);
                world._chunks[x, z] = Chunk.From(chunkReader);
            }
        }
        public void LoadLevelDat(FileStream inputFile)
        {
            world._levelDat = new NbtFile();

            inputFile.Seek(8, SeekOrigin.Begin);
            world._levelDat.LoadFromStream(inputFile, NbtCompression.AutoDetect);

            NbtCompound levelRootTag = world._levelDat.RootTag;

            world.name = levelRootTag["LevelName"].StringValue;
            world.worldSeed = (int)levelRootTag["RandomSeed"].LongValue;
            world.spawnX = levelRootTag["SpawnX"].IntValue;
            world.spawnY = levelRootTag["SpawnY"].IntValue;
            world.spawnZ = levelRootTag["SpawnZ"].IntValue;
            world.worldTime = (int)levelRootTag["Time"].LongValue;
        }
        public void LoadAll()
        {
            if (world.name.ToLower().Contains("unknown")) return;
            if (!Directory.Exists(Path.Combine("worlds", world.LevelName))) return;
            Logger.PInfo("Loading " + world.LevelName + "...");
            FileStream chunksDat = File.OpenRead(Path.Combine("worlds", world.LevelName, "chunks.dat"));
            FileStream levelDat = File.OpenRead(Path.Combine("worlds", world.LevelName, "level.dat"));

            LoadChunks(chunksDat);
            LoadLevelDat(levelDat);

            chunksDat.Dispose();
            levelDat.Dispose();
            Logger.PInfo(world.LevelName + " is loaded!");
        }
    }
}
