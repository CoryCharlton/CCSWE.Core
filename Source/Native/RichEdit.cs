using System;
using System.Runtime.InteropServices;
using COLORREF = System.UInt32;
using DWORD = System.UInt32;
using LCID = System.UInt32;
using WORD = System.UInt16;

namespace CCSWE.Native
{
    // TODO: Needs comments ...
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Defines constants used in a <see cref="CHARFORMAT"/> or <see cref="CHARFORMAT2"/> dwEffects
    /// </summary>
    [Flags]
    public enum CFE : uint
    {
        BOLD = 0x0001,
        ITALIC = 0x0002,
        UNDERLINE = 0x0004,
        STRIKEOUT = 0x0008,
        PROTECTED = 0x0010,
        LINK = 0x0020,
        AUTOCOLOR = 0x40000000, // Note this corresponds to CFM_COLOR, which controls it 

        SUBSCRIPT = 0x00010000, // Superscript and subscript are 
        SUPERSCRIPT = 0x00020000, //  mutually exclusive			 

        SMALLCAPS = CFM.SMALLCAPS,
        ALLCAPS = CFM.ALLCAPS,
        HIDDEN = CFM.HIDDEN,
        OUTLINE = CFM.OUTLINE,
        SHADOW = CFM.SHADOW,
        EMBOSS = CFM.EMBOSS,
        IMPRINT = CFM.IMPRINT,
        DISABLED = CFM.DISABLED,
        REVISED = CFM.REVISED,

        // Note CFE_AUTOCOLOR and CFE_AUTOBACKCOLOR correspond to CFM.COLOR and CFM.BACKCOLOR, respectively, which control them 
        AUTOBACKCOLOR = CFM.BACKCOLOR,
    }

    /// <summary>
    /// Defines constants used in a <see cref="CHARFORMAT"/> or <see cref="CHARFORMAT2"/> dwMask
    /// </summary>
    [Flags]
    public enum CFM : uint
    {
        BOLD = 0x00000001,
        ITALIC = 0x00000002,
        UNDERLINE = 0x00000004,
        STRIKEOUT = 0x00000008,
        PROTECTED = 0x00000010,
        LINK = 0x00000020,
        SIZE = 0x80000000,
        COLOR = 0x40000000,
        FACE = 0x20000000,
        OFFSET = 0x10000000,
        CHARSET = 0x08000000,

        SMALLCAPS = 0x0040, // (*)	
        ALLCAPS = 0x0080, // (*)	
        HIDDEN = 0x0100, // (*)	
        OUTLINE = 0x0200, // (*)	
        SHADOW = 0x0400, // (*)	
        EMBOSS = 0x0800, // (*)	
        IMPRINT = 0x1000, // (*)	
        DISABLED = 0x2000,
        REVISED = 0x4000,

        BACKCOLOR = 0x04000000,
        LCID = 0x02000000,
        UNDERLINETYPE = 0x00800000, // (*)	
        WEIGHT = 0x00400000,
        SPACING = 0x00200000, // (*)	
        KERNING = 0x00100000, // (*)	
        STYLE = 0x00080000, // (*)	
        ANIMATION = 0x00040000, // (*)	
        REVAUTHOR = 0x00008000,

        SUBSCRIPT = CFE.SUBSCRIPT | CFE.SUPERSCRIPT,
        SUPERSCRIPT = SUBSCRIPT,

        ALL = (EFFECTS | SIZE | FACE | OFFSET | CHARSET),
        ALL2 = (ALL | EFFECTS2 | BACKCOLOR | LCID | UNDERLINETYPE | WEIGHT | REVAUTHOR | SPACING | KERNING | STYLE | ANIMATION),

        EFFECTS = (BOLD | ITALIC | UNDERLINE | COLOR | STRIKEOUT | CFE.PROTECTED | LINK),
        EFFECTS2 = (EFFECTS | DISABLED | SMALLCAPS | ALLCAPS | HIDDEN | OUTLINE | SHADOW | EMBOSS | IMPRINT | DISABLED | REVISED | SUBSCRIPT | SUPERSCRIPT | BACKCOLOR),
    }

    /// <summary>
    /// enum for use with EM_GET/SETTEXTMODE
    /// </summary>
    [Flags]
    public enum TEXTMODE
    {
        TM_PLAINTEXT = 1,
        TM_RICHTEXT = 2,	/* default behavior */
        TM_SINGLELEVELUNDO = 4,
        TM_MULTILEVELUNDO = 8,	/* default behavior */
        TM_SINGLECODEPAGE = 16,
        TM_MULTICODEPAGE = 32	/* default behavior */
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
    public class CHARFORMAT
    {
        public UInt32 cbSize = (UInt32) Marshal.SizeOf(typeof (CHARFORMAT));
        public CFM dwMask;
        public CFE dwEffects;
        public int yHeight;
        public int yOffset; // > 0 for superscript, < 0 for subscript 
        public COLORREF crTextColor;
        public byte bCharSet;
        public byte bPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int) LF.FACESIZE)] 
        public string szFaceName;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4, CharSet=CharSet.Auto)]
    public class CHARFORMAT2
    {
        public UInt32 cbSize = (UInt32) Marshal.SizeOf(typeof (CHARFORMAT2));
        public CFM dwMask;
        public CFE dwEffects;
        public int yHeight;
        public int yOffset;
        public COLORREF crTextColor;
        public byte bCharSet;
        public byte bPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int) LF.FACESIZE)] 
        public string szFaceName;
        public WORD wWeight;
        public short sSpacing;
        public COLORREF crBackColor;
        public LCID lcid;
        public DWORD dwReserved;
        public short sStyle;
        public WORD wKerning;
        public byte bUnderlineType;
        public byte bAnimation;
        public byte bRevAuthor;
        public byte bReserved1;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4, CharSet=CharSet.Auto)]
    public class CHARRANGE
    {
        public int cpMin;
        public int cpMax;

        public CHARRANGE(int min, int max)
        {
            cpMax = max;
            cpMin = min;
        }
    }

    /// <summary>
    /// <see cref="CHARFORMAT"/>/<see cref="CHARFORMAT2"/> underline constants
    /// </summary>
    public enum CFU: byte
    {
        CF1UNDERLINE = 0xFF, // map charformat's bit underline to CF2.
        INVERT = 0xFE, // For IME composition fake a selection.	
        UNDERLINEDOTTED = 0x4, // (*) displayed as ordinary underline	
        UNDERLINEDOUBLE = 0x3, // (*) displayed as ordinary underline	
        UNDERLINEWORD = 0x2, // (*) displayed as ordinary underline	
        UNDERLINE = 0x1,
        UNDERLINENONE = 0,

        UNDERLINEDASH = 0x05,
        UNDERLINEDASHDOT = 0x06,
        UNDERLINEDASHDOTDOT = 0x07,
        UNDERLINEWAVE = 0x08,
        UNDERLINETHICK = 0x09,
        UNDERLINEHAIRLINE = 0x0A, /* (*) displayed as ordinary underline	*/
    }

    /// <summary>
    /// Edit control options
    /// </summary>
    public enum ECO : uint
    {
        AUTOWORDSELECTION = 0x00000001,
        AUTOVSCROLL = 0x00000040,
        AUTOHSCROLL = 0x00000080,
        NOHIDESEL = 0x00000100,
        READONLY = 0x00000800,
        WANTRETURN = 0x00001000,
        SAVESEL = 0x00008000,
        SELECTIONBAR = 0x01000000,
        VERTICAL = 0x00400000, // FE specific 
    }
    
    /// <summary>
    /// Edit control options operation
    /// </summary>
    public enum ECOOP: ushort 
    {
        SET = 0x0001,
        OR = 0x0002,
        AND = 0x0003,
        XOR = 0x0004,
    }

    /// <summary>
    /// Edit style constants
    /// </summary>
    public enum ES : int // NOTE: Should this be uint?
    {
        SAVESEL = 0x00008000,
        SUNKEN = 0x00004000,
        DISABLENOSCROLL = 0x00002000,
        // same as WS_MAXIMIZE, but that doesn't make sense so we re-use the value 
        SELECTIONBAR = 0x01000000,
        // same as ES_UPPERCASE, but re-used to completely disable OLE drag'n'drop 
        NOOLEDRAGDROP = 0x00000008,

        // New edit control extended style 
        EX_NOCALLOLEINIT = 0x01000000,

        // These flags are used in FE Windows 
        VERTICAL = 0x00400000,
        NOIME = 0x00080000,

        SELFIME = 0x00040000,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
    public class FORMATRANGE
    {
        public IntPtr hdc;
        public IntPtr hdcTarget;
        public RECT rc;
        public RECT rcPage;
        public CHARRANGE chrg;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4, CharSet=CharSet.Auto)]
    public class PARAFORMAT
    {
        public UInt32 cbSize = (UInt32) Marshal.SizeOf(typeof (PARAFORMAT));
        public PFM dwMask;
        public WORD wNumbering;
        public WORD wReserved;
        public int dxStartIndent;
        public int dxRightIndent;
        public int dxOffset;
        public PFA wAlignment;
        public short cTabCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_TAB_STOPS)] 
        public int[] rgxTabs;
    }

    [StructLayout(LayoutKind.Sequential, Pack=4, CharSet=CharSet.Auto)]
    public class PARAFORMAT2
    {
        public UInt32 cbSize = (UInt32) Marshal.SizeOf(typeof (PARAFORMAT));
        public PFM dwMask;
        public WORD wNumbering; // NOTE: Convert to enum?
        public WORD wReserved;
        public int dxStartIndent;
        public int dxRightIndent;
        public int dxOffset;
        public PFA wAlignment;
        public short cTabCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_TAB_STOPS)] 
        public int[] rgxTabs;

        // PARAFORMAT2 from here onwards.
        public int dySpaceBefore;
        public int dySpaceAfter;
        public int dyLineSpacing;
        public short sStyle;
        public byte bLineSpacingRule;
        public byte bOutlineLevel;
        public WORD wShadingWeight;
        public WORD wShadingStyle;
        public WORD wNumberingStart;
        public WORD wNumberingStyle;
        public WORD wNumberingTab;
        public WORD wBorderSpace;
        public WORD wBorderWidth;
        public WORD wBorders;
    }

    /// <summary>
    /// Alignent contstants for <see cref="PARAFORMAT"/>/<see cref="PARAFORMAT2"/>
    /// </summary>
    public enum PFA : ushort
    {
        LEFT = 0x0001,
        RIGHT = 0x0002,
        CENTER = 0x0003,
        JUSTIFY = 0x0004, // New paragraph-alignment option 2.0 (*)
    }

    /// <summary>
    /// Defines constants used in a <see cref="PARAFORMAT"/> or <see cref="PARAFORMAT2"/> dwMask
    /// </summary>
    [Flags]
    public enum PFM : uint
    {
        STARTINDENT = 0x00000001,
        RIGHTINDENT = 0x00000002,
        OFFSET = 0x00000004,
        ALIGNMENT = 0x00000008,
        TABSTOPS = 0x00000010,
        NUMBERING = 0x00000020,
        OFFSETINDENT = 0x80000000,
        ALL = (STARTINDENT | RIGHTINDENT | OFFSET | ALIGNMENT | TABSTOPS | NUMBERING | OFFSETINDENT | DIR),

        // PARAFORMAT 2.0 masks and effects 
        SPACEBEFORE = 0x00000040,
        SPACEAFTER = 0x00000080,
        LINESPACING = 0x00000100,
        STYLE = 0x00000400,
        BORDER = 0x00000800, // (*)	
        SHADING = 0x00001000, // (*)	
        NUMBERINGSTYLE = 0x00002000, // (*)	
        NUMBERINGTAB = 0x00004000, // (*)	
        NUMBERINGSTART = 0x00008000, // (*)	

        DIR = 0x00010000,
        RTLPARA = 0x00010000, // (Version 1.0 flag) 
        KEEP = 0x00020000, // (*)	
        KEEPNEXT = 0x00040000, // (*)	
        PAGEBREAKBEFORE = 0x00080000, // (*)	
        NOLINENUMBER = 0x00100000, // (*)	
        NOWIDOWCONTROL = 0x00200000, // (*)	
        DONOTHYPHEN = 0x00400000, // (*)	
        SIDEBYSIDE = 0x00800000, // (*)	

        TABLE = 0xc0000000, // (*)	

        // Note: PARAFORMAT has no effects 
        EFFECTS = (DIR | KEEP | KEEPNEXT | TABLE | PAGEBREAKBEFORE | NOLINENUMBER | NOWIDOWCONTROL | DONOTHYPHEN | SIDEBYSIDE | TABLE),

        ALL2 = (ALL | EFFECTS | SPACEBEFORE | SPACEAFTER | LINESPACING | STYLE | SHADING | BORDER | NUMBERINGTAB | NUMBERINGSTART | NUMBERINGSTYLE),
    }

    /// <summary>
    /// Defines constants used in a <see cref="PARAFORMAT"/> or <see cref="PARAFORMAT2"/> wNumbering
    /// </summary>
    public enum PFN : ushort
    {
        NONE = 0,
        BULLET = 0x0001,
    }

    [Flags]
    public enum PFE : uint
    {
        RTLPARA = (PFM.DIR >> 16),
        RTLPAR = (PFM.RTLPARA >> 16), // (Version 1.0 flag) 
        KEEP = (PFM.KEEP >> 16), // (*)	
        KEEPNEXT = (PFM.KEEPNEXT >> 16), // (*)	
        PAGEBREAKBEFORE = (PFM.PAGEBREAKBEFORE >> 16), // (*)	
        NOLINENUMBER = (PFM.NOLINENUMBER >> 16), // (*)	
        NOWIDOWCONTROL = (PFM.NOWIDOWCONTROL >> 16), // (*)	
        DONOTHYPHEN = (PFM.DONOTHYPHEN >> 16), // (*)	
        SIDEBYSIDE = (PFM.SIDEBYSIDE >> 16), // (*)	

        TABLEROW = 0xc000, // These 3 options are mutually	
        TABLECELLEND = 0x8000,//  exclusive and each imply	
        TABLECELL = 0x4000,//  that para is part of a table
    }

    /// <summary>
    /// Selection related constants
    /// </summary>
    [Flags]
    public enum SCF : uint
    {
        DEFAULT = 0x0000,		// set the default charformat or paraformat
        SELECTION = 0x0001,
        WORD = 0x0002,
        ALL = 0x0004,		// not valid with SCF_SELECTION or SCF_WORD
        USEUIRULES = 0x0008,		// modifier for SCF_SELECTION, says that the format came from a toolbar, etc. and therefore UI formatting rules should be used instead of strictly formatting the selection.
    }

    public partial class Constants
    {
        //#define cchTextLimitDefault 32767

        /*
        // Options for EM_SETLANGOPTIONS and EM_GETLANGOPTIONS
        #define IMF_AUTOKEYBOARD		0x0001
        #define IMF_AUTOFONT			0x0002
        #define IMF_IMECANCELCOMPLETE	0x0004	// high completes the comp string when aborting, low cancels.
        #define IMF_IMEALWAYSSENDNOTIFY 0x0008

        // Values for EM_GETIMECOMPMODE
        #define ICM_NOTOPEN				0x0000
        #define ICM_LEVEL3				0x0001
        #define ICM_LEVEL2				0x0002
        #define ICM_LEVEL2_5			0x0003
        #define ICM_LEVEL2_SUI			0x0004

        // New notifications 

        #define EN_MSGFILTER			0x0700
        #define EN_REQUESTRESIZE		0x0701
        #define EN_SELCHANGE			0x0702
        #define EN_DROPFILES			0x0703
        #define EN_PROTECTED			0x0704
        #define EN_CORRECTTEXT			0x0705			// PenWin specific 
        #define EN_STOPNOUNDO			0x0706
        #define EN_IMECHANGE			0x0707			// Far East specific 
        #define EN_SAVECLIPBOARD		0x0708
        #define EN_OLEOPFAILED			0x0709
        #define EN_OBJECTPOSITIONS		0x070a
        #define EN_LINK					0x070b
        #define EN_DRAGDROPDONE			0x070c

        // BiDi specific notifications 

        #define EN_ALIGN_LTR			0x0710
        #define EN_ALIGN_RTL			0x0711

        // Event notification masks 

        #define ENM_NONE				0x00000000
        #define ENM_CHANGE				0x00000001
        #define ENM_UPDATE				0x00000002
        #define ENM_SCROLL				0x00000004
        #define ENM_KEYEVENTS			0x00010000
        #define ENM_MOUSEEVENTS			0x00020000
        #define ENM_REQUESTRESIZE		0x00040000
        #define ENM_SELCHANGE			0x00080000
        #define ENM_DROPFILES			0x00100000
        #define ENM_PROTECTED			0x00200000
        #define ENM_CORRECTTEXT			0x00400000		// PenWin specific 
        #define ENM_SCROLLEVENTS		0x00000008
        #define ENM_DRAGDROPDONE		0x00000010

        // Far East specific notification mask 
        #define ENM_IMECHANGE			0x00800000		// unused by RE2.0 
        #define ENM_LANGCHANGE			0x01000000
        #define ENM_OBJECTPOSITIONS		0x02000000
        #define ENM_LINK				0x04000000
        */

        /*
        // new word break function actions 
        #define WB_CLASSIFY			3
        #define WB_MOVEWORDLEFT		4
        #define WB_MOVEWORDRIGHT	5
        #define WB_LEFTBREAK		6
        #define WB_RIGHTBREAK		7

        // Far East specific flags 
        #define WB_MOVEWORDPREV		4
        #define WB_MOVEWORDNEXT		5
        #define WB_PREVBREAK		6
        #define WB_NEXTBREAK		7

        #define PC_FOLLOWING		1
        #define	PC_LEADING			2
        #define	PC_OVERFLOW			3
        #define	PC_DELIMITER		4
        #define WBF_WORDWRAP		0x010
        #define WBF_WORDBREAK		0x020
        #define	WBF_OVERFLOW		0x040	
        #define WBF_LEVEL1			0x080
        #define	WBF_LEVEL2			0x100
        #define	WBF_CUSTOM			0x200

        // Far East specific flags 
        #define IMF_FORCENONE           0x0001
        #define IMF_FORCEENABLE         0x0002
        #define IMF_FORCEDISABLE        0x0004
        #define IMF_CLOSESTATUSWINDOW   0x0008
        #define IMF_VERTICAL            0x0020
        #define IMF_FORCEACTIVE         0x0040
        #define IMF_FORCEINACTIVE       0x0080
        #define IMF_FORCEREMEMBER       0x0100
        #define IMF_MULTIPLEEDIT        0x0400

        // Word break flags (used with WB_CLASSIFY) 
        #define WBF_CLASS			((BYTE) 0x0F)
        #define WBF_ISWHITE			((BYTE) 0x10)
        #define WBF_BREAKLINE		((BYTE) 0x20)
        #define WBF_BREAKAFTER		((BYTE) 0x40)


        // new data types 

        #ifdef _WIN32
        // extended edit word break proc (character set aware) 
        typedef LONG (*EDITWORDBREAKPROCEX)(char *pchText, LONG cchText, BYTE bCharSet, INT action),
        #endif
        */




        //public const uint yHeightCharPtsMost 1638

        /*
        typedef struct _textrange
        {
	        CHARRANGE chrg,
	        LPSTR lpstrText,	// allocated by caller, zero terminated by RichEdit 
        } TEXTRANGEA,

        typedef struct _editstream
        {
	        DWORD dwCookie,		// user value passed to callback as first parameter 
	        DWORD dwError,		// last error 
	        EDITSTREAMCALLBACK pfnCallback,
        } EDITSTREAM,

        // stream formats 

        #define SF_TEXT			0x0001
        #define SF_RTF			0x0002
        #define SF_RTFNOOBJS	0x0003		// outbound only 
        #define SF_TEXTIZED		0x0004		// outbound only 
        #define SF_UNICODE		0x0010		// Unicode file of some kind 

        // Flag telling stream operations to operate on the selection only 
        // EM_STREAMIN will replace the current selection 
        // EM_STREAMOUT will stream out the current selection 
        #define SFF_SELECTION	0x8000

        // Flag telling stream operations to operate on the common RTF keyword only 
        // EM_STREAMIN will accept the only common RTF keyword 
        // EM_STREAMOUT will stream out the only common RTF keyword 
        #define SFF_PLAINRTF	0x4000

        typedef struct _findtext
        {
	        CHARRANGE chrg,
	        LPSTR lpstrText,
        } FINDTEXTA,

        typedef struct _findtextw
        {
	        CHARRANGE chrg,
	        LPWSTR lpstrText,
        } FINDTEXTW,
        */

        /*
        typedef struct _findtextexa
        {
	        CHARRANGE chrg,
	        LPSTR lpstrText,
	        CHARRANGE chrgText,
        } FINDTEXTEXA,

        typedef struct _findtextexw
        {
	        CHARRANGE chrg,
	        LPWSTR lpstrText,
	        CHARRANGE chrgText,
        } FINDTEXTEXW,
        */

        // all paragraph measurements are in twips 
        //#define lDefaultTab 720


        /*
        typedef struct _nmhdr
        {
	        HWND	hwndFrom,
	        _WPAD	_wPad1,
	        UINT	idFrom,
	        _WPAD	_wPad2,
	        UINT	code,
	        _WPAD	_wPad3,
        } NMHDR,
        #endif  // !WM_NOTIFY 

        typedef struct _msgfilter
        {
	        NMHDR	nmhdr,
	        UINT	msg,
	        _WPAD	_wPad1,
	        WPARAM	wParam,
	        _WPAD	_wPad2,
	        LPARAM	lParam,
        } MSGFILTER,

        typedef struct _reqresize
        {
	        NMHDR nmhdr,
	        RECT rc,
        } REQRESIZE,

        typedef struct _selchange
        {
	        NMHDR nmhdr,
	        CHARRANGE chrg,
	        WORD seltyp,
        } SELCHANGE,
        */

        /*
        #define SEL_EMPTY		0x0000
        #define SEL_TEXT		0x0001
        #define SEL_OBJECT		0x0002
        #define SEL_MULTICHAR	0x0004
        #define SEL_MULTIOBJECT	0x0008
        */
        /* 
        #define GCM_RIGHTMOUSEDROP  0x8000

        typedef struct _endropfiles
        {
            NMHDR nmhdr,
            HANDLE hDrop,
            LONG cp,
            BOOL fProtected,
        } ENDROPFILES,

        typedef struct _enprotected
        {
            NMHDR nmhdr,
            UINT msg,
            _WPAD	_wPad1,
            WPARAM wParam,
            _WPAD	_wPad2,
            LPARAM lParam,
            CHARRANGE chrg,
        } ENPROTECTED,

        typedef struct _ensaveclipboard
        {
            NMHDR nmhdr,
            LONG cObjectCount,
            LONG cch,
        } ENSAVECLIPBOARD,

        #ifndef MACPORT
        typedef struct _enoleopfailed
        {
            NMHDR nmhdr,
            LONG iob,
            LONG lOper,
            HRESULT hr,
        } ENOLEOPFAILED,
        #endif

        #define	OLEOP_DOVERB	1

        typedef struct _objectpositions
        {
            NMHDR nmhdr,
            LONG cObjectCount,
            LONG *pcpPositions,
        } OBJECTPOSITIONS,

        typedef struct _enlink
        {
            NMHDR nmhdr,
            UINT msg,
            _WPAD   _wPad1,
            WPARAM wParam,
            _WPAD   _wPad2,
            LPARAM lParam,
            CHARRANGE chrg,
        } ENLINK,

        // PenWin specific 
        typedef struct _encorrecttext
        {
            NMHDR nmhdr,
            CHARRANGE chrg,
            WORD seltyp,
        } ENCORRECTTEXT,

        // Far East specific 
        typedef struct _punctuation
        {
            UINT	iSize,
            LPSTR	szPunctuation,
        } PUNCTUATION,

        // Far East specific 
        typedef struct _compcolor
        {
            COLORREF crText,
            COLORREF crBackground,
            DWORD dwEffects,
        }COMPCOLOR,


        // clipboard formats - use as parameter to RegisterClipboardFormat() 
        #define CF_RTF 			TEXT("Rich Text Format")
        #define CF_RTFNOOBJS 	TEXT("Rich Text Format Without Objects")
        #define CF_RETEXTOBJ 	TEXT("RichEdit Text and Objects")

        // Paste Special 
        typedef struct _repastespecial
        {
            DWORD	dwAspect,
            DWORD	dwParam,
        } REPASTESPECIAL,

        //	UndoName info 
        typedef enum _undonameid
        {
            UID_UNKNOWN     = 0,
            UID_TYPING		= 1,
            UID_DELETE 		= 2,
            UID_DRAGDROP	= 3,
            UID_CUT			= 4,
            UID_PASTE		= 5
        } UNDONAMEID,

        // flags for the GETEXTEX data structure 
        #define GT_DEFAULT		0
        #define GT_USECRLF		1

        // EM_GETTEXTEX info, this struct is passed in the wparam of the message 
        typedef struct _gettextex
        {
            DWORD	cb,				// count of bytes in the string				
            DWORD	flags,			// flags (see the GT_XXX defines			
            UINT	codepage,		// code page for translation (CP_ACP for default,
                                       1200 for Unicode							
            LPCSTR	lpDefaultChar,	// replacement for unmappable chars			
            LPBOOL	lpUsedDefChar,	// pointer to flag set when def char used	
        } GETTEXTEX,

        // flags for the GETTEXTLENGTHEX data structure							
        #define GTL_DEFAULT		0	// do the default (return # of chars)		
        #define GTL_USECRLF		1	// compute answer using CRLFs for paragraphs
        #define GTL_PRECISE		2	// compute a precise answer					
        #define GTL_CLOSE		4	// fast computation of a "close" answer		
        #define GTL_NUMCHARS	8	// return the number of characters			
        #define GTL_NUMBYTES	16	// return the number of _bytes_				

        // EM_GETTEXTLENGTHEX info, this struct is passed in the wparam of the msg 
        typedef struct _gettextlengthex
        {
            DWORD	flags,			// flags (see GTL_XXX defines)				
            UINT	codepage,		// code page for translation (CP_ACP for default,
                                       1200 for Unicode							
        } GETTEXTLENGTHEX,
	
        // BiDi specific features 
        typedef struct _bidioptions
        {
            UINT	cbSize,
            _WPAD	_wPad1,
            WORD	wMask,
            WORD	wEffects, 
        } BIDIOPTIONS,

        // BIDIOPTIONS masks 
        #if (_RICHEDIT_VER == 0x0100)
        #define BOM_DEFPARADIR			0x0001	// Default paragraph direction (implies alignment) (obsolete) 
        #define BOM_PLAINTEXT			0x0002	// Use plain text layout (obsolete) 
        #define BOM_NEUTRALOVERRIDE		0x0004	// Override neutral layout (obsolete) 
        #endif // _RICHEDIT_VER == 0x0100 
        #define BOM_CONTEXTREADING		0x0008	// Context reading order 
        #define BOM_CONTEXTALIGNMENT	0x0010	// Context alignment 

        // BIDIOPTIONS effects 
        #if (_RICHEDIT_VER == 0x0100)
        #define BOE_RTLDIR				0x0001	// Default paragraph direction (implies alignment) (obsolete) 
        #define BOE_PLAINTEXT			0x0002	// Use plain text layout (obsolete) 
        #define BOE_NEUTRALOVERRIDE		0x0004	// Override neutral layout (obsolete) 
        #endif // _RICHEDIT_VER == 0x0100 
        #define BOE_CONTEXTREADING		0x0008	// Context reading order 
        #define BOE_CONTEXTALIGNMENT	0x0010	// Context alignment 

        // Additional EM_FINDTEXT[EX] flags 
        #define FR_MATCHDIAC                    0x20000000
        #define FR_MATCHKASHIDA                 0x40000000
        #define FR_MATCHALEFHAMZA               0x80000000

        // UNICODE embedding character 
        #ifndef WCH_EMBEDDING
        #define WCH_EMBEDDING (WCHAR)0xFFFC
        #endif // WCH_EMBEDDING 
        */
    }
    // ReSharper restore InconsistentNaming
}
