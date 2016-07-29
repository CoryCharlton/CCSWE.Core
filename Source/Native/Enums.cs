using System;

namespace CCSWE.Native
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Event Message constants
    /// </summary>
    public enum EM : uint
    {
        GETRECT = 0x00B2,

        GETLIMITTEXT = (WM.USER + 37),
        POSFROMCHAR = (WM.USER + 38),
        CHARFROMPOS = (WM.USER + 39),
        SCROLLCARET = (WM.USER + 49),
        CANPASTE = (WM.USER + 50),
        DISPLAYBAND = (WM.USER + 51),
        EXGETSEL = (WM.USER + 52),
        EXLIMITTEXT = (WM.USER + 53),
        EXLINEFROMCHAR = (WM.USER + 54),
        EXSETSEL = (WM.USER + 55),
        FINDTEXT = (WM.USER + 56),
        FORMATRANGE = (WM.USER + 57),
        GETCHARFORMAT = (WM.USER + 58),
        GETEVENTMASK = (WM.USER + 59),
        GETOLEINTERFACE = (WM.USER + 60),
        GETPARAFORMAT = (WM.USER + 61),
        GETSELTEXT = (WM.USER + 62),
        HIDESELECTION = (WM.USER + 63),
        PASTESPECIAL = (WM.USER + 64),
        REQUESTRESIZE = (WM.USER + 65),
        SELECTIONTYPE = (WM.USER + 66),
        SETBKGNDCOLOR = (WM.USER + 67),
        SETCHARFORMAT = (WM.USER + 68),
        SETEVENTMASK = (WM.USER + 69),
        SETOLECALLBACK = (WM.USER + 70),
        SETPARAFORMAT = (WM.USER + 71),
        SETTARGETDEVICE = (WM.USER + 72),
        STREAMIN = (WM.USER + 73),
        STREAMOUT = (WM.USER + 74),
        GETTEXTRANGE = (WM.USER + 75),
        FINDWORDBREAK = (WM.USER + 76),
        SETOPTIONS = (WM.USER + 77),
        GETOPTIONS = (WM.USER + 78),
        FINDTEXTEX = (WM.USER + 79),
        GETWORDBREAKPROCEX = (WM.USER + 80),
        SETWORDBREAKPROCEX = (WM.USER + 81),

        /* Richedit v2.0 messages */
        SETUNDOLIMIT = (WM.USER + 82),
        REDO = (WM.USER + 84),
        CANREDO = (WM.USER + 85),
        GETUNDONAME = (WM.USER + 86),
        GETREDONAME = (WM.USER + 87),
        STOPGROUPTYPING = (WM.USER + 88),
        SETTEXTMODE = (WM.USER + 89),
        GETTEXTMODE = (WM.USER + 90),

        AUTOURLDETECT = (WM.USER + 91),
        GETAUTOURLDETECT = (WM.USER + 92),
        SETPALETTE = (WM.USER + 93),
        GETTEXTEX = (WM.USER + 94),
        GETTEXTLENGTHEX = (WM.USER + 95),
        SHOWSCROLLBAR = (WM.USER + 96),

        /* Far East specific messages */
        SETPUNCTUATION = (WM.USER + 100),
        GETPUNCTUATION = (WM.USER + 101),
        SETWORDWRAPMODE = (WM.USER + 102),
        GETWORDWRAPMODE = (WM.USER + 103),
        SETIMECOLOR = (WM.USER + 104),
        GETIMECOLOR = (WM.USER + 105),
        SETIMEOPTIONS = (WM.USER + 106),
        GETIMEOPTIONS = (WM.USER + 107),
        CONVPOSITION = (WM.USER + 108),

        SETLANGOPTIONS = (WM.USER + 120),
        GETLANGOPTIONS = (WM.USER + 121),
        GETIMECOMPMODE = (WM.USER + 122),

        FINDTEXTW = (WM.USER + 123),
        FINDTEXTEXW = (WM.USER + 124),

        /* BiDi specific messages */
        SETBIDIOPTIONS = (WM.USER + 200),
        GETBIDIOPTIONS = (WM.USER + 201),
    }

    /// <summary>
    /// Font weights
    /// </summary>
    public enum FW : ushort
    {
        DONTCARE = 0,
        THIN = 100,
        EXTRALIGHT = 200,
        LIGHT = 300,
        NORMAL = 400,
        MEDIUM = 500,
        SEMIBOLD = 600,
        BOLD = 700,
        EXTRABOLD = 800,
        HEAVY = 900,

        ULTRALIGHT = EXTRALIGHT,
        REGULAR = NORMAL,
        DEMIBOLD = SEMIBOLD,
        ULTRABOLD = EXTRABOLD,
        BLACK = HEAVY,
    }

    public enum LF : int
    {
        FACESIZE = 32,
    }

    /// <summary>
    /// Scrollbar constants
    /// </summary>
    public enum SB : int
    {
        HORZ = 0,
        VERT = 1,
        CTL = 2,
        BOTH = 3,
    }

    /// <summary>
    /// SCROLLINFO fMask constants
    /// </summary>
    [Flags]
    public enum SIF: uint 
    {
        RANGE = 0x1,
        PAGE = 0x2,
        POS = 0x4,
        DISABLENOSCROLL = 0x8,
        TRACKPOS = 0x10,
        ALL = RANGE | PAGE | POS | TRACKPOS,
    }

    [Flags]
    public enum SHGFI: uint
    {
        ICON = 0x100,
        LARGEICON = 0x0,
        SMALLICON = 0x1,
    }

    /// <summary>
    /// Window messages
    /// </summary>
    public enum WM : uint
    {
        USER = 0x400,
        SETREDRAW = 11,
        EXITSIZEMOVE = 0x232,
        QUERYENDSESSION = 0x11,
        NOTIFY = 0x004E,

        /* RichEdit messages */
        CONTEXTMENU = 0x007B,
        PRINTCLIENT = 0x0318,
    }

    /// <summary>
    /// Window styles
    /// </summary>
    public enum WS: uint
    {
        CHILD = 0x40000000,
        CLIPCHILDREN = 0x02000000,
        VISIBLE = 0x10000000,
    }
    // ReSharper restore InconsistentNaming
}
