using System;
using System.Drawing;
using System.Windows.Forms;
using CCSWE.Native;

namespace CC.Utilities.Interop
{
    /// <summary>
    /// A collection of conversion methods for native interop methods.
    /// </summary>
    public static class InteropConvert
    {
        #region Public Methods
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Converts a <see cref="CFE"/> constant to a <see cref="FontStyle"/>
        /// </summary>
        /// <param name="dwEffects">The <see cref="CFE"/> to convert</param>
        /// <returns>A <see cref="FontStyle"/></returns>
        public static FontStyle CFE_ToFontStyle(CFE dwEffects)
        {
            return CFM_ToFontStyle((CFM)dwEffects);
        }

        /// <summary>
        /// Converts a <see cref="CFM"/> constant to a <see cref="FontStyle"/>
        /// </summary>
        /// <param name="dwMask">The <see cref="CFM"/> to convert</param>
        /// <returns>A <see cref="FontStyle"/></returns>
        public static FontStyle CFM_ToFontStyle(CFM dwMask)
        {
            FontStyle fontStyle = 0;

            if ((dwMask & CFM.BOLD) == CFM.BOLD)
            {
                fontStyle |= FontStyle.Bold;
            }

            if ((dwMask & CFM.ITALIC) == CFM.ITALIC)
            {
                fontStyle |= FontStyle.Italic;
            }

            if ((dwMask & CFM.STRIKEOUT) == CFM.STRIKEOUT)
            {
                fontStyle |= FontStyle.Strikeout;
            }

            if ((dwMask & CFM.UNDERLINE) == CFM.UNDERLINE)
            {
                fontStyle |= FontStyle.Underline;
            }

            return fontStyle;
        }

        /// <summary>
        /// Converts a <see cref="FontStyle"/> to <see cref="CFE"/> constant.
        /// </summary>
        /// <param name="fontStyle">The <see cref="FontStyle"/> to convert.</param>
        /// <returns>A <see cref="CFE"/> constant</returns>
        public static CFM FontStyleTo_CFE(FontStyle fontStyle)
        {
            return FontStyleTo_CFM(fontStyle);
        }

        /// <summary>
        /// Converts a <see cref="FontStyle"/> to <see cref="CFM"/> constant.
        /// </summary>
        /// <param name="fontStyle">The <see cref="FontStyle"/> to convert.</param>
        /// <returns>A <see cref="CFM"/> constant</returns>
        public static CFM FontStyleTo_CFM(FontStyle fontStyle)
        {
            CFM cfm_ = 0;

            if ((fontStyle & FontStyle.Bold) == FontStyle.Bold)
            {
                cfm_ |= CFM.BOLD;
            }

            if ((fontStyle & FontStyle.Italic) == FontStyle.Italic)
            {
                cfm_ |= CFM.ITALIC;
            }

            if ((fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
            {
                cfm_ |= CFM.STRIKEOUT;
            }

            if ((fontStyle & FontStyle.Underline) == FontStyle.Underline)
            {
                cfm_ |= CFM.UNDERLINE;
            }

            return cfm_;
        }

        /// <summary>
        /// Converts a <see cref="HorizontalAlignment"/> to <see cref="PFA"/> constant.
        /// </summary>
        /// <param name="alignment">The <see cref="HorizontalAlignment"/> to convert.</param>
        /// <returns>A <see cref="PFA"/> constant.</returns>
        public static PFA HorizontalAlignmentTo_PFA(HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    {
                        return PFA.CENTER;
                    }
                case HorizontalAlignment.Left:
                    {
                        return PFA.LEFT;
                    }
                case HorizontalAlignment.Right:
                    {
                        return PFA.RIGHT;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException("alignment", alignment, "Invalid 'alignment' value.");
                    }
            }
        }

        /// <summary>
        /// Converts a <see cref="PFA"/> constant to a <see cref="HorizontalAlignment"/>.
        /// </summary>
        /// <param name="pfa">The <see cref="PFA"/> to convert.</param>
        /// <returns>A <see cref="HorizontalAlignment"/></returns>
        public static HorizontalAlignment PFA_ToHorizontalAlignment(PFA pfa)
        {
            switch (pfa)
            {
                case PFA.CENTER:
                    {
                        return HorizontalAlignment.Center;
                    }
                case PFA.LEFT:
                    {
                        return HorizontalAlignment.Left;
                    }
                case PFA.RIGHT:
                    {
                        return HorizontalAlignment.Right;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException("pfa", pfa, "Invalid PFA value.");
                    }
            }
        }
        // ReSharper restore InconsistentNaming
        #endregion
    }
}
