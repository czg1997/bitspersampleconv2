﻿using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System;

namespace ReadAllFilesOnSpecifiedFolder {
    public class ReadAllFilesOnFolder : IDisposable {
        public enum EventType {
            CollectionFinished,
            ReadProgressed,
            ReadError,
            ReadFinished,
        };

        public delegate void ProgressCallback(EventType ev, string path, string errMsg, int finished, int total);

        private ProgressCallback mCb;

        public CancellationTokenSource mCts = new CancellationTokenSource();

        public ReadAllFilesOnFolder(ProgressCallback cb) {
            mCb = cb;
        }

        public enum Option {
            Parallel = 1,
        };

        /// <summary>
        /// Run()をキャンセルする。
        /// </summary>
        public bool Cancel() {
            if (mCts == null) {
                return false;
            }

            if (mCts.Token.CanBeCanceled) {
                mCts.Cancel();
                return true;
            }
            return false;
        }

        public void Run(string root, int opt) {
            System.Diagnostics.Debug.Assert(mCts != null);

            try {
                // まずフォルダ内のファイルを列挙します。
                var pathList = WWUtil.DirectoryUtil.CollectFilesOnFolder(root, null, mCts);

                if (mCb != null) {
                    mCb(EventType.CollectionFinished, "", "", 0, pathList.Length);
                }

                int progressCounter = 0;

                if (0 != (opt & (int)Option.Parallel)) {
                    // Parallel実行する。
                    var po = new ParallelOptions();
                    po.CancellationToken = mCts.Token;
                    po.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
                    Parallel.For(0, pathList.Length, po, idx => {
                        if (po.CancellationToken.IsCancellationRequested) {
                            po.CancellationToken.ThrowIfCancellationRequested();
                        }
                        //System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;
                        string path = pathList[idx];

                        string msg = ReadOneFile(path);

                        int cnt = Interlocked.Increment(ref progressCounter);
                        if (mCb != null) {
                            mCb(msg.Length == 0 ? EventType.ReadProgressed : EventType.ReadError,
                                    path, msg, cnt, pathList.Length);
                        }
                    });
                } else {
                    // 直列実行する。
                    //System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;
                    foreach (string path in pathList) {
                        string msg = ReadOneFile(path);

                        ++progressCounter;
                        if (mCb != null) {
                            mCb(msg.Length == 0 ? EventType.ReadProgressed : EventType.ReadError,
                                    path, msg, progressCounter, pathList.Length);
                        }

                        if (mCts.IsCancellationRequested) {
                            return;
                        }
                    }
                }

                if (mCb != null) {
                    mCb(EventType.ReadFinished, "", "", progressCounter, pathList.Length);
                }
            } catch (OperationCanceledException ex) {
                Console.WriteLine("{0}", ex);
                return;
            }
        }

        private string ReadOneFile(string path) {
            var buf = new byte[1024 * 1024];

            try {
                using (var br = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read))) {
                    long totalBytes = br.BaseStream.Length;

                    //Console.WriteLine("    {0:N0} bytes, {1}", totalBytes, path);

                    // bufのサイズ単位で読む。
                    for (long pos = 0; pos < totalBytes; pos += buf.Length) {
                        int readBytes = buf.Length;
                        if (totalBytes < pos + readBytes) {
                            readBytes = (int)(totalBytes - pos);
                        }
                        br.Read(buf, 0, readBytes);
                    }
                }
            } catch (System.IO.IOException ex) {
                return ex.ToString();
            } catch (System.Exception ex) {
                return ex.ToString();
            }

            return "";
        }

        public void Dispose() {
            if (mCts != null) {
                mCts.Dispose();
                mCts = null;
            }
        }
    }
}
