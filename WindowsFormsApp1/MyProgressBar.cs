using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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
    [Designer(typeof(MyProgressDesigner))]
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

        //private int Abc(int a, ref string s, out DateTime dt)
        //{
        //    s = "Phuc";
        //    dt = DateTime.Now;
        //    return 100;
        //}

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
                int value = _iValue;
                if (value < Math.Min(_iMin, _iMax)) value = Math.Min(_iMin, _iMax);
                else if (value > Math.Max(_iMin, _iMax))
                {
                    value = Math.Max(_iMin, _iMax);
                }

                int range = Math.Abs(_iMin - _iMax);
                float percent;
                if (_iMin < _iMax)
                {
                    percent = (float)(value - _iMin) / range;
                }
                else
                {
                    percent = (float)(Math.Abs(range - value)) / range;
                }
                text = (int)(percent * 100) + "%";
                if (_enProgressDirection == EnProgressDirection.Horizontal)
                {
                    width = percent * rc.Width;
                    if (_iMin > _iMax) left = rc.Width - width;
                }
                else
                {
                    height = percent * rc.Height;
                    if (_iMin > _iMax) top = rc.Height - height;
                }
                //if (_iMin < _iMax)
                //{
                //    float percent = (float)(value - _iMin) / range;
                //    text = (int)(percent * 100) + "%";
                //    if (_enProgressDirection == EnProgressDirection.Horizontal)
                //    {
                //        width = percent * rc.Width;
                //    }
                //    else
                //    {
                //        height = percent * rc.Height;
                //        top = rc.Height - height;
                //    }
                //}
                //else
                //{

                //    value = Math.Abs(range - value);
                //    float percent = (float)value / range;
                //    text = (int)(percent * 100) + "%";
                //    if (_enProgressDirection == EnProgressDirection.Horizontal)
                //    {
                //        width = percent * rc.Width;
                //        left = rc.Width - width;
                //    }
                //    else
                //    {
                //        height = percent * rc.Height;
                //    }
                //}
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

        private int GetValue(int m)
        {
            return m + 1;
        }

        private class MyProgressDesigner: ControlDesigner
        {
            private DesignerActionListCollection _actions;
            public MyProgressDesigner()
            {
                _actions = new DesignerActionListCollection();
                _actions.Add(new MyAction (this));
            }
            public override DesignerActionListCollection ActionLists => _actions;
            public override void Initialize(IComponent component)
            {
                base.Initialize(component);
                //MessageBox.Show("Initialize " + component);
            }
            protected override void PreFilterProperties(IDictionary properties)
            {
                //MessageBox.Show("PreFilterAttributes " );
                RemoveProp(properties, "AccessibleName");
                RemoveProp(properties, "AccessibleRole");
                //properties.Remove("Minimum");
                //base.PreFilterAttributes(attributes);
            }
            private void RemoveProp(IDictionary properties, string key)
            {
                if (properties.Contains(key)) properties.Remove(key);
            }
        }
        private class MyAction: DesignerActionList
        {
            MyProgressBar _progressBar;
            DesignerActionItemCollection _collectionItems;
            public MyAction(MyProgressDesigner designer) : base(designer.Component)
            {
                _progressBar = designer.Component as MyProgressBar;
                _collectionItems = new DesignerActionItemCollection();
                _collectionItems.Add(new DesignerActionMethodItem(this, "Setup", "Setup", true));

                _collectionItems.Add(new DesignerActionMethodItem(this, "Venha", "Go Home"));
            }

            private void Setup()
            {
                MessageBox.Show("Setup");
            }
            private void Venha()
            {
                MessageBox.Show("Welcome home");
            }


            public override DesignerActionItemCollection GetSortedActionItems() => _collectionItems;
        }


    }
    public interface ITrackBarEditor
    {
        int GetMin();
        int GetMax();
        int GetValue();
        void SetValue(int value);
    }
    public class TrackBarEditor : UITypeEditor
    {
        TrackBar _bar;
        ITrackBarEditor _control;
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
                _control.SetValue(_bar.Value);
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
                try
                {
                    if (!(context.Instance is ITrackBarEditor))
                    {
                        MessageBox.Show(context.Instance.GetType().Name + " must implement interface ITrackBarEditor");
                        return value;
                    }
                    _control = context.Instance as ITrackBarEditor;
                    int iMinimum = _control.GetMin();
                    int iMaximum = _control.GetMax();
                    if (iMinimum > iMaximum) Extension.Swap(ref iMinimum, ref iMaximum);
                    int iValue = (int)value;
                    if (iValue < iMinimum) iValue = iMinimum;
                    else if (iValue > iMaximum) iValue = iMaximum;
                    _bar.Minimum = iMinimum;
                    _bar.Maximum = iMaximum;
                    _bLockUpdate = true;
                    _bar.Value = iValue;
                    _bLockUpdate = false;
                    IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

                    service.DropDownControl(_bar);
                    return _bar.Value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return value;
        }


    }
    public class TrackBarEditor1 : UITypeEditor
    {
        TrackBar _bar;
        object _control;
        bool _bLockUpdate = false;
        public TrackBarEditor1()
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
                _control.SetPropertyValue("Value", _bar.Value);
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
                try
                {
                    _control = context.Instance as MyProgressBar;
                    int iMinimum = (int)_control.GetPropertyValue("Minimum");
                    int iMaximum = (int)_control.GetPropertyValue("Maximum");
                    if (iMinimum > iMaximum) Extension.Swap(ref iMinimum, ref iMaximum);
                    int iValue = (int)value;
                    if (iValue < iMinimum) iValue = iMinimum;
                    else if (iValue > iMaximum) iValue = iMaximum;
                    _bar.Minimum = iMinimum;
                    _bar.Maximum = iMaximum;
                    _bLockUpdate = true;
                    _bar.Value = iValue;
                    _bLockUpdate = false;
                    IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

                    service.DropDownControl(_bar);
                    return _bar.Value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return value;
        }


    }
}

