using java.util.function;

namespace GES.Inside.Data.Models
{
    public static class  ServiceSelectListItemConfiguration
    {
        public static long[] NormIds_Group1 => new long[]
        {
            4, // Human Rights
            5, // Labour Rights
            3, // Environment
            2, // Corruption
            //6  // Inhumane Weapons
        };

        public static long[] NormIds_Group2 => new long[]
        {
            8,  // Taxation
            //9,  // Competition
            //10, // Consumer Rights
        };

        public static long[] ServiceIds_Standalone => new long[]
        {
            //22 // Controversial
        };

        public static long[] ServiceIds_UsingNormGroup1 => new long[]
        {
            //16, // Alert
            30, // Business Conduct (or 'Conventions' in G_Services tbl)
            //20, // Global Ethical Standard
        };

        public static long[] ServiceIds_GlobalEthicalStandard => new long[]
        {
            20, // Global Ethical Standard
        };

        public static long[] ServiceIds_UsingNormGroup2 => new long[]
        {
            //43, // Alert - Extended
            47, // Business Conduct - Extended - Taxation
        };

        public static long[] ServiceIds_SnR => new long[]
        {
            31, // Burma
            32, // Emerging Markets
            35, // Palm Oil
            44, // Carbon Risks
            45, // Water
            46, // Taxation
            //48, // Business Ethics & Culture
            //49, // Children's Rights
            //50, // Cybersecurity
            //51, // Pharma
            //42  // Low Performers
            //53, //Governance
        };

        public static long[] ServiceIds_Bespoke => new long[]
        {
            //52 // Bespoke
        };
    }
}