﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WWLanBenchmark {
    class Server {
        private const int HASH_BYTES = 32;
        private const int ONE_GIGA = 1000 * 1000 * 1000;

        private BackgroundWorker mBackgroundWorker;

        public void Run(BackgroundWorker backgroundWorker, int port) {
            mBackgroundWorker = backgroundWorker;
            TcpListener server = null;
            try {
                IPAddress localAddr = IPAddress.Any;
                server = new TcpListener(localAddr, port);
                server.Start();

                while (true) {
                    mBackgroundWorker.ReportProgress(1, "Waiting for a connection...\n");

                    using (var client = server.AcceptTcpClient()) {
                        Accept(client);
                    }
                }
            } catch (SocketException e) {
                Console.WriteLine("SocketException: {0}\n", e);
            } catch (IOException e) {
                Console.WriteLine("IOException: {0}\n", e);
            } finally {
                server.Stop();
            }
        }

        struct Settings {
            public long continuousRecvGiB;
            public int testIterationCount;
        };

        private void RecvSettings(BinaryReader br, out Settings settings) {
            settings = new Settings();
            settings.continuousRecvGiB = br.ReadInt32();
            settings.testIterationCount = br.ReadInt32();
        }

        private void Accept(TcpClient client) {
            mBackgroundWorker.ReportProgress(1, string.Format("Connected from {0}\n", client.Client.RemoteEndPoint));
            using (var stream = client.GetStream()) {
                using (var br = new BinaryReader(stream)) {
                    Settings settings;
                    RecvSettings(br, out settings);

                    mBackgroundWorker.ReportProgress(1, string.Format("Settings: Recv {0}GB of data x {1} times\n",
                        settings.continuousRecvGiB, settings.testIterationCount));

                    for (int i = 0; i < settings.testIterationCount; ++i) {
                        RecvData(br, settings, i);
                    }
                    mBackgroundWorker.ReportProgress(1, "Done.\n");
                }
            }
        }

        private void RecvData(BinaryReader br, Settings settings, int idx) {
            var recvIdx = br.ReadInt32();

            mBackgroundWorker.ReportProgress(1, string.Format("({0} / {1}) Receiving {2}GB stream...\n",
                idx + 1, settings.testIterationCount, settings.continuousRecvGiB));

            var recvData = new List<byte[]>();
            var sw = new Stopwatch();

            var recvHash = br.ReadBytes(HASH_BYTES);

            sw.Start();
            for (int i = 0; i < settings.continuousRecvGiB; ++i) {
                var data = br.ReadBytes(ONE_GIGA);
                recvData.Add(data);
            }

            sw.Stop();

            mBackgroundWorker.ReportProgress(1, string.Format("    Received {0}GB in {1} seconds. {2:0.###}Gbps\n",
                settings.continuousRecvGiB, sw.ElapsedMilliseconds / 1000.0,
                (double)settings.continuousRecvGiB * 8 / (sw.ElapsedMilliseconds / 1000.0)));

            var calcHash = CalcHash(recvData);
            if (calcHash.SequenceEqual(recvHash)) {
                mBackgroundWorker.ReportProgress(1, string.Format("    SHA256 hash consistency check succeeded.\n"));
            } else {
                mBackgroundWorker.ReportProgress(1, string.Format("    SHA256 hash consistency check FAILED !!\n"));
            }

            recvData = null;
        }

        private byte[] CalcHash(List<byte[]> data) {
            using (var hash = new SHA256CryptoServiceProvider()) {
                foreach (var buff in data) {
                    hash.TransformBlock(buff, 0, ONE_GIGA, buff, 0);
                }
                hash.TransformFinalBlock(new byte[0], 0, 0);

                byte[] result = new byte[HASH_BYTES];
                Array.Copy(hash.Hash, result, HASH_BYTES);
                return result;
            }
        }
    }
}
