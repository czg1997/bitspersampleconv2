﻿using System.Collections.Generic;
using WWMath;

namespace WWIIRFilterDesign {
    /// <summary>
    /// 実係数rational functionの並列接続
    /// </summary>
    public class IIRFilterParallel : IIRFilterGraph {
        private List<IIRFilterBlockReal> mFilterBlockList = new List<IIRFilterBlockReal>();

        public IIRFilterParallel() {
        }

        /// <summary>
        /// 多項式pは並列接続される。(p同士を足していく感じになる)
        /// </summary>
        public void Add(RealRationalPolynomial p) {
            var b = new IIRFilterBlockReal(p);
            mFilterBlockList.Add(b);
        }

        // 入力値xを受け取ると、出力yが出てくる。
        public double Filter(double x) {
            double y = 0;

            foreach (var b in mFilterBlockList) {
                y += b.Filter(x);
            }

            return y;
        }

        public int BlockCount() { return mFilterBlockList.Count; }

        public IIRFilterBlockReal GetNthBlock(int nth) {
            return mFilterBlockList[nth];
        }

        /// <summary>
        /// 同じフィルター特性で、ディレイの状態も同じだが、ディレイの実体が異なるインスタンスを作る。
        /// </summary>
        public IIRFilterGraph CreateCopy() {
            var r = new IIRFilterParallel();

            for (int i = 0; i < mFilterBlockList.Count; ++i) {
                var fb = mFilterBlockList[i];
                r.mFilterBlockList.Add(fb.CreateCopy());
            }

            return r;
        }
    }
}
