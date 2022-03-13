using System;

namespace uPalette.Generated
{
    public enum ColorTheme
    {
        Light,
        Classic,
        Dark,
    }

    public static class ColorThemeExtensions
    {
        public static string ToThemeId(this ColorTheme theme)
        {
            switch (theme)
            {
                case ColorTheme.Light:
                    return "ca0a054c-cd1f-415c-8946-eb80b0a46c86";
                case ColorTheme.Classic:
                    return "27944544-a27d-4596-b509-f5e0ab6f7f90";
                case ColorTheme.Dark:
                    return "e243989c-d425-4612-9a47-faf2f4cb4e69";
                default:
                    throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
            }
        }
    }

    public enum ColorEntry
    {
        Background,
        MainText,
        KeyColor1,
        KeyColor2,
        ButtonContents,
        SubButtonBackground,
        OptionButtonBackground,
        OptionButtonContents,
    }

    public static class ColorEntryExtensions
    {
        public static string ToEntryId(this ColorEntry entry)
        {
            switch (entry)
            {
                case ColorEntry.Background:
                    return "341d370a-effa-40c9-90c8-ab56c7f03dde";
                case ColorEntry.MainText:
                    return "48d15dc5-7597-44a0-8fe5-88f6fc67dcaa";
                case ColorEntry.KeyColor1:
                    return "4d4ede3c-e38a-45f4-8dd2-db084f5ac4a6";
                case ColorEntry.KeyColor2:
                    return "5adfd18e-5fc5-4bc8-958a-4be3f6ec59b6";
                case ColorEntry.ButtonContents:
                    return "ace06488-c485-4f64-a359-64ab534f0454";
                case ColorEntry.SubButtonBackground:
                    return "813656bc-9c9a-432d-a465-98344fd8b8b1";
                case ColorEntry.OptionButtonBackground:
                    return "f0b6741e-8aab-4614-901d-d938f23a1ed8";
                case ColorEntry.OptionButtonContents:
                    return "3de325d8-9dc3-4304-aef6-df868625e6f2";
                default:
                    throw new ArgumentOutOfRangeException(nameof(entry), entry, null);
            }
        }
    }

    public enum GradientTheme
    {
        Default,
    }

    public static class GradientThemeExtensions
    {
        public static string ToThemeId(this GradientTheme theme)
        {
            switch (theme)
            {
                case GradientTheme.Default:
                    return "b38a3fad-0b4f-42ed-9d55-849a4615d181";
                default:
                    throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
            }
        }
    }

    public enum GradientEntry
    {
    }

    public static class GradientEntryExtensions
    {
        public static string ToEntryId(this GradientEntry entry)
        {
            switch (entry)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(entry), entry, null);
            }
        }
    }

    public enum CharacterStyleTheme
    {
        Default,
    }

    public static class CharacterStyleThemeExtensions
    {
        public static string ToThemeId(this CharacterStyleTheme theme)
        {
            switch (theme)
            {
                case CharacterStyleTheme.Default:
                    return "2bf21534-f311-4f5a-945f-8bb282e4c047";
                default:
                    throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
            }
        }
    }

    public enum CharacterStyleEntry
    {
    }

    public static class CharacterStyleEntryExtensions
    {
        public static string ToEntryId(this CharacterStyleEntry entry)
        {
            switch (entry)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(entry), entry, null);
            }
        }
    }

    public enum CharacterStyleTMPTheme
    {
        Standard,
        Classic,
        Grunge,
    }

    public static class CharacterStyleTMPThemeExtensions
    {
        public static string ToThemeId(this CharacterStyleTMPTheme theme)
        {
            switch (theme)
            {
                case CharacterStyleTMPTheme.Standard:
                    return "10a7c335-bf25-4867-8af2-dd1b19273382";
                case CharacterStyleTMPTheme.Classic:
                    return "4015608b-c889-497a-929c-95b799fb0fc4";
                case CharacterStyleTMPTheme.Grunge:
                    return "d91fac9e-c104-42c1-a5f8-6990c1d30827";
                default:
                    throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
            }
        }
    }

    public enum CharacterStyleTMPEntry
    {
        Logo,
        Title,
        MainButton,
    }

    public static class CharacterStyleTMPEntryExtensions
    {
        public static string ToEntryId(this CharacterStyleTMPEntry entry)
        {
            switch (entry)
            {
                case CharacterStyleTMPEntry.Logo:
                    return "6921221d-1d74-4033-bee5-e0d01cb06122";
                case CharacterStyleTMPEntry.Title:
                    return "40b83e6c-74e3-4f98-ac6a-f8c4705ad8a9";
                case CharacterStyleTMPEntry.MainButton:
                    return "066cbf8f-8b4a-441c-937e-c234e2780b88";
                default:
                    throw new ArgumentOutOfRangeException(nameof(entry), entry, null);
            }
        }
    }
}
