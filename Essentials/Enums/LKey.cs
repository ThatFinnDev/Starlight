using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Starlight.Enums;

[System.Serializable]
[JsonConverter(typeof(StringEnumConverter))]
public enum LKey
{
    None = 0,

    // Letters A-Z
    A = 1, B = 2, C = 3, D = 4, E = 5, F = 6, G = 7, H = 8, I = 9, J = 10,
    K = 11, L = 12, M = 13, N = 14, O = 15, P = 16, Q = 17, R = 18, S = 19, T = 20,
    U = 21, V = 22, W = 23, X = 24, Y = 25, Z = 26,

    // Extended latin letters
    À = 101, Á = 102, Â = 103, Ã = 104, Ä = 105, Å = 106, Æ = 107, Ç = 108,
    È = 109, É = 110, Ê = 111, Ë = 112, Ì = 113, Í = 114, Î = 115, Ï = 116,
    Ñ = 117, Ò = 118, Ó = 119, Ô = 120, Õ = 121, Ö = 122, Ø = 123, Ù = 124,
    Ú = 125, Û = 126, Ü = 127, Ý = 128, Ÿ = 129, ß = 130,

    // Numbers
    Alpha0 = 201, Alpha1 = 202, Alpha2 = 203, Alpha3 = 204, Alpha4 = 205,
    Alpha5 = 206, Alpha6 = 207, Alpha7 = 208, Alpha8 = 209, Alpha9 = 210,

    // Function keys
    F1 = 301, F2 = 302, F3 = 303, F4 = 304, F5 = 305, F6 = 306,
    F7 = 307, F8 = 308, F9 = 309, F10 = 310, F11 = 311, F12 = 312,

    // Modifiers
    LeftShift = 401, RightShift = 402, LeftControl = 403, RightControl = 404,
    LeftAlt = 405, RightAlt = 406,

    // Navigation
    UpArrow = 501, DownArrow = 502, LeftArrow = 503, RightArrow = 504,
    PageUp = 505, PageDown = 506, Home = 507, End = 508, Insert = 509, Delete = 510,

    // Common keys
    Space = 601, Return = 602, Enter = 602, Escape = 603, Tab = 604, Backspace = 605,

    // Symbols
    Minus = 701, Equals = 702, LeftBracket = 703, RightBracket = 704,
    LeftCurly = 705, RightCurly = 706, Semicolon = 707, Quote = 708, Comma = 709,
    Period = 710, Slash = 711, Backslash = 712, Plus = 713, Underscore = 714,
    Colon = 715, Exclamation = 716, Question = 717, At = 718, Hash = 719,
    Dollar = 720, Percent = 721, Caret = 722, Ampersand = 723, Asterisk = 724,
    LeftParen = 725, RightParen = 726, Tilde = 727, BackQuote = 728, Paragraph = 729,
    Degree = 730, Euro = 731 , Less = 732, Greater = 733, Pipe = 734 , DoubleQuote = 735,
    
    // Numpad
    Keypad0 = 801, Keypad1 = 802, Keypad2 = 803, Keypad3 = 804, Keypad4 = 805,
    Keypad5 = 806, Keypad6 = 807, Keypad7 = 808, Keypad8 = 809, Keypad9 = 810,
    KeypadDivide = 811, KeypadMultiply = 812, KeypadMinus = 813,
    KeypadPlus = 814, KeypadEnter = 815, KeypadPeriod = 816,

    // Cyrillic 
    А = 901, Б = 902, В = 903, Г = 904, Д = 905, Е = 906, Ё = 907, Ж = 908,
    З = 909, И = 910, Й = 911, К = 912, Л = 913, М = 914, Н = 915, О = 916,
    П = 917, Р = 918, С = 919, Т = 920, У = 921, Ф = 922, Х = 923, Ц = 924,
    Ч = 925, Ш = 926, Щ = 927, Ъ = 928, Ы = 929, Ь = 930, Э = 931, Ю = 932, Я = 933,

    // Hiragana 
    //The fallback font doesn't support it :/
    /*あ = 1001, い = 1002, う = 1003, え = 1004, お = 1005,
    か = 1006, き = 1007, く = 1008, け = 1009, こ = 1010,
    さ = 1011, し = 1012, す = 1013, せ = 1014, そ = 1015,
    た = 1016, ち = 1017, つ = 1018, て = 1019, と = 1020,
    な = 1021, に = 1022, ぬ = 1023, ね = 1024, の = 1025,
    は = 1026, ひ = 1027, ふ = 1028, へ = 1029, ほ = 1030,
    ま = 1031, み = 1032, む = 1033, め = 1034, も = 1035,
    や = 1036, ゆ = 1037, よ = 1038,
    ら = 1039, り = 1040, る = 1041, れ = 1042, ろ = 1043,
    わ = 1044, を = 1045, ん = 1046*/
}
