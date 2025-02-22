﻿/*
 * Project	    : Outlook 2003 Style Navigation Pane
 *
 * Author       : Muhammed ŞAHİN
 * eMail        : muhammed.sahin@gmail.com
 *
 * Description  : NavigateBar collapsible screen text
 * 
 */

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProgrammersInc.Windows.Forms
{
    /// <summary>
    /// if show collapsible screen then display caption text on this control
    /// </summary>
    class NavigateBarCollapsibleText : UserControl
    {

        #region NavigateBar
        NavigateBar navigateBar;
        public NavigateBar NavigateBar
        {
            set
            {
                navigateBar = value;
                Invalidate();
            }
        }
        #endregion

        #region CaptionBand
        NavigateBarCaption captionBand;
        public NavigateBarCaption CaptionBand
        {
            get { return captionBand; }
        }
        #endregion

        #region ContentText
        string contentText = "";
        public string ContentText
        {
            get { return contentText; }
            set
            {
                contentText = value;
                Invalidate();
            }
        }
        #endregion

        #region IsSelected

        bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;

                if (isSelected == false)
                    isMouseOver = false;

                Invalidate();
            }
        }
        #endregion

        #region Constructor Method
        public NavigateBarCollapsibleText(NavigateBar tNavigateBar)
        {

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            navigateBar = tNavigateBar;

            this.Cursor = Cursors.Hand;
            this.TabStop = true;

            captionBand = new NavigateBarCaption(navigateBar);
            captionBand.CollapseMode = false;
            captionBand.Cursor = Cursors.Default;
            captionBand.CollapseButton.ToolTipText = Resources.TEXT_COLLAPSE_BUTTON_EXPAND;

            this.Controls.Add(captionBand);

        }


        #endregion

        bool isMouseOver = false;

        #region Overrided Methods

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            captionBand.Invalidate();
            base.OnInvalidated(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (navigateBar.SelectedButton != null &&
                navigateBar.SelectedButton.RelatedControl != null)
                Cursor = Cursors.Hand;
            else
                Cursor = Cursors.Default;

            isMouseOver = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isMouseOver = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            Invalidate();
            base.OnResize(eventargs);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            isMouseOver = true;
            isSelected = true;
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            isMouseOver = false;
            isSelected = false;
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {

            base.OnPaintBackground(e);

            // Paint

            if (IsSelected)
                NavigateBarHelper.PaintGradientControl(this, e.Graphics, navigateBar.NavigateBarColorTable.ButtonSelectedBegin, navigateBar.NavigateBarColorTable.ButtonSelectedBegin, navigateBar.NavigateBarColorTable.PaintAngle);
            else if (isMouseOver && !IsSelected)
                NavigateBarHelper.PaintGradientControl(this, e.Graphics, navigateBar.NavigateBarColorTable.ButtonMouseOverBegin, navigateBar.NavigateBarColorTable.ButtonMouseOverBegin, navigateBar.NavigateBarColorTable.PaintAngle);
            else
                NavigateBarHelper.PaintGradientControl(this, e.Graphics, navigateBar.NavigateBarColorTable.ButtonNormalBegin, navigateBar.NavigateBarColorTable.ButtonNormalBegin, navigateBar.NavigateBarColorTable.PaintAngle);

            //if (!(this.IsSelected || isMouseOver))
            //{
            //    Pen pen = new Pen(Color.White);
            //    // |
            //    e.Graphics.DrawLine(pen, new Point(0, 0), new Point(0, this.Height));
            //    // ___
            //    e.Graphics.DrawLine(pen, new Point(0, this.CaptionBand.Height), new Point(this.Width - 1, this.CaptionBand.Height));
            //}

            float leftPoss = 0, rightPoss = 0;

            Font font = new Font(SystemFonts.CaptionFont.FontFamily, 16, FontStyle.Bold);

            // Vertical Text 

            e.Graphics.TranslateTransform(0, this.ClientSize.Height);
            e.Graphics.RotateTransform(270F);

            SizeF sf = e.Graphics.MeasureString(this.ContentText, font);
            leftPoss = (this.ClientSize.Height - sf.Width - CaptionBand.Height) / 2;
            rightPoss = (this.ClientSize.Width - sf.Height) / 2;

            e.Graphics.DrawString(this.ContentText, font,
                new SolidBrush(navigateBar.NavigateBarColorTable.CaptionTextColor),
                leftPoss, rightPoss);

            //

        }
        #endregion

    }
}
