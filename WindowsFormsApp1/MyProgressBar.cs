using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class MyProgressBar : UserControl, IDisposable
    {
        public enum EnProgressDirection
        {
            Horizontal, VerticalUpLeft, VerticalUpRight,
        }
        #region variables
        private int _iMin = 0;
        private int _iMax = 100;
        private int _iValue = 0;
        private bool _bLockUpdate = false;

        private readonly SolidBrush _fillBrush = null;
        private readonly SolidBrush _textBrush = null;
        private EnProgressDirection _enProgressDirection = EnProgressDirection.Horizontal;

        // String Format
        private StringFormat _format = new StringFormat();
        #endregion
        public MyProgressBar()
        {
            InitializeComponent();
            //this.DoubleBuffered = true;
            _fillBrush = new SolidBrush(ForeColor);
            _textBrush = new SolidBrush(Color.Black);
            _format.Alignment = StringAlignment.Center;
            _format.LineAlignment = StringAlignment.Center;
        }

        public new void Dispose()
        {
            base.Dispose();
            _fillBrush.Dispose();
            _textBrush.Dispose();
            _format.Dispose(); 
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            if (_fillBrush != null) _fillBrush.Color = ForeColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Drawing(e.Graphics);
        }

        private void Drawing(Graphics graphics)
        {
            if (_bLockUpdate) return;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Rectangle rc = ClientRectangle;
            float left = 0;
            float width = rc.Width;
            float top = 0;
            float height = rc.Height;
            string text = "100%";
            if (_iMin == _iMax)
            {
                if (_iValue < _iMin)
                {
                    text = "0%";
                    width = 0;
                }
            }
            else 
            {
                if (_enProgressDirection == EnProgressDirection.Horizontal)
                {
                    if (_iMin < _iMax)
                    {
                        int value = _iValue;
                        if (value < _iMin) value = _iMin;
                        else if (value > _iMax) value = _iMax;

                        float percent = (float)(value - _iMin) / (_iMax - _iMin);
                        text = (int)(percent * 100) + "%";
                        width = percent * rc.Width;
                    }
                    else
                    {
                        int value = _iValue;
                        if (value < _iMax) value = _iMax;
                        else if (value > _iMin) value = _iMin;
                        int range = _iMin - _iMax;
                        value = Math.Abs(range - value);
                        float percent = (float)value / range;
                        text = (int)(percent * 100) + "%";
                        width = percent * rc.Width;
                        left = rc.Width - width;
                    }
                }
                else
                {
                    if (_iMin < _iMax)
                    {
                        int value = _iValue;
                        if (value < _iMin) value = _iMin;
                        else if (value > _iMax) value = _iMax;

                        float percent = (float)(value - _iMin) / (_iMax - _iMin);
                        text = (int)(percent * 100) + "%";
                        height = percent * rc.Height;
                        top = rc.Height - height;
                    }
                    else
                    {
                        int value = _iValue;
                        if (value < _iMax) value = _iMax;
                        else if (value > _iMin) value = _iMin;
                        int range = _iMin - _iMax;
                        value = range - value;
                        value = Math.Abs(value);
                        float percent = (float)(value) / range;
                        text = (int)(percent * 100) + "%";
                        height = percent * rc.Height;
                    }

                }
            }
            graphics.FillRectangle(_fillBrush, left, top, width, height);
            if (_enProgressDirection == EnProgressDirection.Horizontal)
                graphics.DrawString(text, this.Font, _textBrush, rc, _format);
            else
            {
                float angle = 90;
                if (_enProgressDirection == EnProgressDirection.VerticalUpRight) angle = -90;
                //ternary operation
                //float angle = _enProgressDirection == EnProgressDirection.VerticalUpRight ? -90 : 90;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddString(text, Font.FontFamily, (int)Font.Style, Font.Size, rc, _format);
                    using (Matrix matrix = new Matrix())
                    {
                        matrix.RotateAt(angle, new Point(rc.Width / 2, rc.Height / 2));
                        path.Transform(matrix);
                    }
                    graphics.FillPath(_textBrush, path);
                }
            }
        }
        [Category("ProgressValues")]
        public EnProgressDirection ProgressDirection
        {
            get => _enProgressDirection;
            set 
            {
                if (_enProgressDirection != value)
                {

                    //if (value == EnProgressDirection.Horizontal) _format.FormatFlags = 0;
                    //else _format.FormatFlags = StringFormatFlags.DirectionVertical;

                    if (_enProgressDirection == EnProgressDirection.Horizontal || value == EnProgressDirection.Horizontal)
                    {

                        bool isLock = _bLockUpdate;
                        _bLockUpdate = true;
                        int temp = Width;
                        Width = Height;
                        Height = temp;
                        _bLockUpdate = isLock;
                    }

                    _enProgressDirection = value;
                    if (!_bLockUpdate)
                    {
                        Refresh();
                    }
                }
            }
        }

        [Browsable(false)]
        public bool LockUpdate
        {
            get => _bLockUpdate;
            set => SetProperty(ref _bLockUpdate, value);
        }
        /// <summary>
        /// Get or Set Minimum
        /// </summary>
        [Category("ProgressValues"), Browsable(true)]
        public int Minimum 
        {
            get => _iMin;
            set => SetProperty(ref _iMin, value);
        }
        [Category("ProgressValues")]
        public int Maximum
        {
            get => _iMax;
            set => SetProperty(ref _iMax, value);
        }
        [Category("ProgressValues"), Editor(typeof(TrackBarEditor), typeof(UITypeEditor))]
        public int Value
        {
            get => _iValue;
            set => SetProperty(ref _iValue, value);
        }


        [Category("ProgressValues")]
        public Color TextColor
        {
            get => _textBrush.Color;
            set
            {
                if (_textBrush.Color != value)
                {
                    _textBrush.Color = value;
                    if (!_bLockUpdate) Refresh();
                }
            }
        }

        //template class T
        private void SetProperty<T>(ref T variable, T value)
        {
            if (!variable.Equals(value))
            {
                variable = value;
                if (!_bLockUpdate) Refresh();
            }
        }

        private class TrackBarEditor: UITypeEditor
        {
            TrackBar _bar;
            MyProgressBar _progressBar;
            bool _bLockUpdate = false;
            public TrackBarEditor()
            {
                _bar = new TrackBar();
                _bar.TickFrequency = 10;
                _bar.ValueChanged += TrackBar_ValueChanged;
            }

            private void TrackBar_ValueChanged(object sender, EventArgs e)
            {
                if (!_bLockUpdate)
                {
                    _bLockUpdate = true;
                    _progressBar.Value = _bar.Value;
                    _bLockUpdate = false;
                }
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (!_bLockUpdate)
                {
                    _progressBar = context.Instance as MyProgressBar;
                    int iMinimum = _progressBar.Minimum;
                    int iMaximum = _progressBar.Maximum;
                    if (iMinimum > iMaximum) Extension.Swap(ref iMinimum, ref iMaximum);
                    int iValue = _progressBar.Value;
                    if (iValue < iMinimum) iValue = iMinimum;
                    else if (iValue > iMaximum) iValue = iMaximum;
                    _bar.Minimum = iMinimum;
                    _bar.Maximum = iMaximum;
                    _bLockUpdate = true;
                    _bar.Value = iValue;
                    _bLockUpdate = false;
                    IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

                    service.DropDownControl(_bar);
                }
                return _bar.Value;
            }
        }
    }
}
