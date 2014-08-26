using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CTripOSS.Baiji.Helper;

namespace CTripOSS.Baiji.JavaGenerator
{
    public class GeneratorConfig
    {
        /// <summary>
        /// Input base URI to load Lean IDL files.
        /// </summary>
        public Uri InputBase { get; private set; }
        /// <summary>
        /// The output folder which contain the generated sources.
        /// </summary>
        public DirectoryInfo OutputFolder { get; private set; }
        /// <summary>
        /// If not-null, overrides the java namespace definitions in the IDL files.
        /// </summary>
        public string OverridePackage { get; private set; }
        /// <summary>
        /// If no namespace was set in the Lean IDL file, fall back to this namespace.
        /// </summary>
        public string DefaultPackage { get; private set; }

        private HashSet<GeneratorTweak> GeneratorTweaks { get; set; }

        /// <summary>
        /// If true, generate code for all included Lean IDLs instead of just referring to them.
        /// </summary>
        public bool GenerateIncludedCode { get; private set; }
        /// <summary>
        /// The template to use for generating source code.
        /// </summary>
        public string CodeFlavor { get; private set; }

        private GeneratorConfig(Uri inputBase,
            DirectoryInfo outputFolder,
            string overridePackage,
            string defaultPackage,
            HashSet<GeneratorTweak> generatorTweaks,
            bool generateIncludedCode,
            string codeFlavor)
        {
            InputBase = inputBase;
            OutputFolder = outputFolder;
            OverridePackage = overridePackage;
            DefaultPackage = defaultPackage;
            GeneratorTweaks = generatorTweaks;
            GenerateIncludedCode = generateIncludedCode;
            CodeFlavor = codeFlavor;
        }

        public bool ContainsTweak(GeneratorTweak tweak)
        {
            return GeneratorTweaks.Contains(tweak);
        }

        /// <summary>
        /// The template to use for generating source code
        /// </summary>
        public class Builder
        {
            private Uri inputBase = null;
            private DirectoryInfo outputFolder = null;
            private string overridePackage = null;
            private string defaultPackage = null;
            private HashSet<GeneratorTweak> generatorTweaks = new HashSet<GeneratorTweak>();
            private bool generateIncludedCode = false;
            private string codeFlavor = null;

            public Builder()
            {
            }

            public GeneratorConfig Build()
            {
                Enforce.IsNotNull(outputFolder, "output folder must be set!");
                Enforce.IsNotNull(inputBase, "input base uri must be set to load includes!");
                Enforce.IsNotNull(codeFlavor, "no code flavor selected!");

                return new GeneratorConfig(inputBase,
                    outputFolder,
                    overridePackage,
                    defaultPackage,
                    generatorTweaks,
                    generateIncludedCode,
                    codeFlavor);
            }

            public Builder InputBase(Uri inputBase)
            {
                this.inputBase = inputBase;
                return this;
            }

            public Builder OutputFolder(DirectoryInfo outputFolder)
            {
                this.outputFolder = outputFolder;
                return this;
            }

            public Builder OverridePackage(string overridePackage)
            {
                this.overridePackage = overridePackage;
                return this;
            }

            public Builder DefaultPackage(string defaultPackage)
            {
                this.defaultPackage = defaultPackage;
                return this;
            }

            public Builder AddTweak(GeneratorTweak tweak)
            {
                this.generatorTweaks.Add(tweak);
                return this;
            }

            public Builder GenerateIncludedCode(bool generateIncludedCode)
            {
                this.generateIncludedCode = generateIncludedCode;
                return this;
            }

            public Builder CodeFlavor(string codeFlavor)
            {
                this.codeFlavor = codeFlavor;
                return this;
            }
        }
    }
}
