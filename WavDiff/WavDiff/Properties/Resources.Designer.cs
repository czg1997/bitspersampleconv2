﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:2.0.50727.3053
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WavDiff.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WavDiff.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Bits per sample({1}) must be 16: {0} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string BitsPerSampleFailFormat {
            get {
                return ResourceManager.GetString("BitsPerSampleFailFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Main window may be occulted by this console window... に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ConsoleIntroduction {
            get {
                return ResourceManager.GetString("ConsoleIntroduction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   average diff per sample={0}. max diff={1}. You can magnify {2}x without clipping に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string DiffStatistics {
            get {
                return ResourceManager.GetString("DiffStatistics", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Please specify wave file pathes to input/output. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string Introduction {
            get {
                return ResourceManager.GetString("Introduction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Number of channels of two files is different に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string NumChannelsIsDifferent {
            get {
                return ResourceManager.GetString("NumChannelsIsDifferent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Output file is clipped! You can magnify {0}x without clipping. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string OutputFileLevelOver {
            get {
                return ResourceManager.GetString("OutputFileLevelOver", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Process started. ({0} - {1} ) x {2} ==&gt; {3} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ProcessStarted {
            get {
                return ResourceManager.GetString("ProcessStarted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   read failed: {0} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ReadFileFailFormat {
            get {
                return ResourceManager.GetString("ReadFileFailFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Read file: {0} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ReadFileStarted {
            get {
                return ResourceManager.GetString("ReadFileStarted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Read wav file failed: {0} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ReadWavFileFailed {
            get {
                return ResourceManager.GetString("ReadWavFileFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sample rate is different. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SampleRateIsDifferent {
            get {
                return ResourceManager.GetString("SampleRateIsDifferent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Two WAV files are exactly the same! に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string TwoWavFilesAreExactlyTheSame {
            get {
                return ResourceManager.GetString("TwoWavFilesAreExactlyTheSame", resourceCulture);
            }
        }
        
        /// <summary>
        ///   two wave files are too different to continue processing に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string TwoWavFilesTooDifferent {
            get {
                return ResourceManager.GetString("TwoWavFilesTooDifferent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   WAV file is too short must be longer than {1} seconds: {0} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string WavFileTooShort {
            get {
                return ResourceManager.GetString("WavFileTooShort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Write failed: {0} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string WriteFailed {
            get {
                return ResourceManager.GetString("WriteFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Write succeeded: {0} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string WriteSucceeded {
            get {
                return ResourceManager.GetString("WriteSucceeded", resourceCulture);
            }
        }
    }
}
