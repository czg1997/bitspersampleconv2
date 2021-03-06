﻿// 日本語。

using System;
using WWUtil;

namespace WWMath {
    // Richard G. Lyons, Understanding Digital Signal Processing, 3 rd Ed., Pearson, 2011, pp. 744
    public class WWSlidingDFT {
        int mN;
        SlidingDFTbin[] mBins;
        Delay mDelayN;

        /// <summary>
        /// Sliding DFT
        /// 注：この関数は、compensation引数を省略して呼ぶと結果をN分の1します。WWDftCpu.Idft1dやWWRadix2Fft.InverseFft(compensation引数省略)と組み合わせると時間ドメイン値が戻ります。
        /// </summary>
        /// <param name="N">DFTサイズ。</param>
        /// <param name="compensation">出力値を乗算する係数。省略時は1/Nになる。</param>
        public WWSlidingDFT(int N, double? compensation = null) {
            if (N <= 0) {
                throw new ArgumentOutOfRangeException("N");
            }

            double c = 1.0 / N;
            if (compensation != null) {
                c = (double)compensation;
            }

            mN = N;
            mBins = new SlidingDFTbin[N];
            for (int i = 0; i < mBins.Length; ++i) {
                mBins[i] = new SlidingDFTbin(i, N, c);
            }

            mDelayN = new Delay(N);
        }

        /// <summary>
        /// 時間ドメイン値x(n)を1個入力すると周波数ドメイン値X^m(q)が出る。qは時間、mは周波数, 0≦m＜N
        /// </summary>
        /// <param name="x">時間ドメイン値x(n)</param>
        /// <returns>m要素の周波数ドメイン値X^m(q) mは周波数, 0≦m＜N </returns>
        public WWComplex[] Filter(double x) {
            // N comb filter
            double delay = mDelayN.Filter(x);
            double combOut = x - delay;

            // complex resonator
            var r = new WWComplex[mN];
            for (int i = 0; i < mN; ++i) {
                r[i] = mBins[i].Filter(combOut);
            }

            return r;
        }

        /// <summary>
        /// 窓関数をかけた周波数ドメイン値X^m(q)を戻す。
        /// 同様の処理をWWGoertzel (Stable Goertzel algorithm)で行うこともできるだろう。
        /// </summary>
        public WWComplex[] FilterWithWindow(double x, WWWindowFunc.WindowType wt) {
            var r = Filter(x);
            var rW = new WWComplex[r.Length];

            var w = WWWindowFunc.FreqDomainWindowCoeffs(wt);

            switch (w.Length) {
            case 2:
                for (int i = 0; i < mN; ++i) {
                    int il = i - 1;
                    if (il < 0) {
                        il = mN - 1;
                    }
                    int ir = i + 1;
                    if (mN <= ir) {
                        ir = 0;
                    }

                    rW[i] = WWComplex.Add(
                        WWComplex.Mul(r[il], w[1] * 0.5),
                        WWComplex.Mul(r[i],  w[0]),
                        WWComplex.Mul(r[ir], w[1] * 0.5));
                }
                return rW;
            case 3:
                for (int i = 0; i < mN; ++i) {
                    var pos = new int[5];
                    for (int offs = -2; offs <= 2; ++offs) {
                        int p = i + offs;
                        if (p < 0) {
                            p += mN;
                        }
                        if (mN <= p) {
                            p -= mN;
                        }

                        pos[offs + 2] = p;
                    }

                    rW[i] = WWComplex.Add(
                        WWComplex.Mul(r[pos[0]], w[2] * 0.5),
                        WWComplex.Mul(r[pos[1]], w[1] * 0.5),
                        WWComplex.Mul(r[pos[2]], w[0]),
                        WWComplex.Mul(r[pos[3]], w[1] * 0.5),
                        WWComplex.Mul(r[pos[4]], w[2] * 0.5));
                }
                return rW;
            default:
                System.Diagnostics.Debug.Assert(false);
                return null;
            }
        }

        class SlidingDFTbin {
            int mN; //< DFT size N
            double mCompensation;
            int mM; //< bin number m 0≦m＜N
            WWComplex mE;

            DelayT<WWComplex> mDelay;

            public SlidingDFTbin(int m, int N, double compensation) {
                if (N <= 0) {
                    throw new ArgumentOutOfRangeException("N");
                }
                if (m < 0 || N <= m) {
                    throw new ArgumentOutOfRangeException("m");
                }

                mN = N;
                mCompensation = compensation;
                mM = m;

                double θ = 2.0 * Math.PI * m / N;

                mE = new WWComplex(Math.Cos(θ), Math.Sin(θ));

                mDelay = new DelayT<WWComplex>(1);
                mDelay.Fill(WWComplex.Zero());
            }

            /// <summary>
            /// 時間ドメイン値xを1サンプル投入。周波数ビン X^m(q)が出てくる。
            /// 起動後0～N-1サンプルまでは、出力値は過渡値となる。
            /// </summary>
            /// <param name="x">時間ドメインサンプル値x</param>
            /// <returns>周波数ビン X^m(q)。起動後0～N-1サンプルまでは、出力値は過渡値となる。</returns>
            public WWComplex Filter(double x) {

                // Complex resonator
                var delay = mDelay.GetNthDelayedSampleValue(0);
                WWComplex acc = WWComplex.Add(new WWComplex(x, 0), delay);

                WWComplex m = WWComplex.Mul(acc, mE);

                mDelay.Filter(m);

                return WWComplex.Mul(m, mCompensation);
            }
        };
    }
}
