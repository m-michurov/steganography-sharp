using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using CommandLine;
using Steganography.Core.Encoding;
using Steganography.Core.Exceptions;

namespace Steganography.CLI {
    public static class Program {
        private const int ExitOk = 0;
        private const int ExitFailure = 1;

        private const string Ext = ".png";

        private static int Main(string[] args) =>
            Parser.Default
                .ParseArguments<EncodeOptions, DecodeOptions>(args)
                .MapResult(
                    (EncodeOptions o) => Encode(o),
                    (DecodeOptions o) => Decode(o),
                    _ => ExitFailure
                );

        private static int Encode(EncodeOptions options) {
            if (false == options.Out.EndsWith(Ext, StringComparison.Ordinal)) {
                options.Out += Ext;
            }
            
            try {
                using var container = new Bitmap(options.ContainerImage);
                var data = File.ReadAllBytes(options.Data);
                
                using (var encoded = BitmapEncoder.Encode(container, data)) {
                    encoded.Save(options.Out, ImageFormat.Png);
                }

                Console.WriteLine($"Written modified image to \"{options.Out}\".");

                if (false == options.Verify) {
                    return ExitOk;
                }

                using var saved = new Bitmap(options.Out, useIcm: false);
                var decoded = BitmapEncoder.Decode(saved);

                if (data.SequenceEqual(decoded)) {
                    return ExitOk;
                }

                Console.Error.WriteLine("Verification failed: decoded data differs from encoded data.");
                Console.Error.WriteLine("Try using a different container.");

                return ExitFailure;
            } catch (ContainerTooSmall e) {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine("Consider using a bigger image.");
            } catch (Exception e) {
                Console.Error.WriteLine(e.Message);
            }

            return ExitFailure;
        }

        private static int Decode(DecodeOptions options) {
            try {
                using var container = new Bitmap(options.ContainerImage);

                var data = BitmapEncoder.Decode(container);

                File.WriteAllBytes(options.Out, data);

                Console.WriteLine($"Written decoded data to \"{options.Out}\".");

                return ExitOk;
            } catch (ContainerTooSmall e) {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine("Consider using a bigger image.");
            } catch (Exception e) {
                Console.Error.WriteLine(e.Message);
            }

            return ExitFailure;
        }

        // ReSharper disable once ClassNeverInstantiated.Global
        // ReSharper disable once MemberCanBePrivate.Global
        [Verb("encode", HelpText = "Encode data.")]
        public sealed class EncodeOptions {
            [Value(index: 0, Required = true, HelpText = "Container image path.")]
            public string ContainerImage { get; set; } = string.Empty;

            [Value(index: 1, Required = true, HelpText = "Data to encode.")]
            public string Data { get; set; } = string.Empty;

            [Option(shortName: 'o', "out", Required = false, HelpText = "Output file name.", Default = "secret" + Ext)]
            public string Out { get; set; } = string.Empty;

            [Option(
                "verify",
                Required = false,
                HelpText = "Decode encoded data and compare it to original data.",
                Default = false
            )]
            public bool Verify { get; set; } = false;
        }

        // ReSharper disable once ClassNeverInstantiated.Global
        // ReSharper disable once MemberCanBePrivate.Global
        [Verb("decode", HelpText = "Decode data.")]
        public sealed class DecodeOptions {
            [Value(index: 0, Required = true, HelpText = "Container image path.")]
            public string ContainerImage { get; set; } = string.Empty;


            [Option(shortName: 'o', "out", Required = false, HelpText = "Output file name.", Default = "data")]
            public string Out { get; set; } = string.Empty;
        }
    }
}