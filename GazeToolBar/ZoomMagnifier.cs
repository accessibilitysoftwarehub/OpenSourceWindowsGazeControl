﻿using System;
using System.Windows.Forms;
using Karna.Magnification;
using System.Drawing;
using EyeXFramework.Forms;
using EyeXFramework;
using Tobii.EyeX.Framework;

namespace GazeToolBar
{
    public class ZoomMagnifier
    {
        protected const int UPDATE_SPEED = 100; //how fast the lens will update

        //TODO: Move these to settings json
        public static bool DO_ZOOM = true;         //Zoom enabled
        public static float ZOOM_SPEED = 0.005F;//005F;    //Amount zoom will increment
        public static float ZOOM_MAX = Program.readSettings.maxZoom;          //Max zoom amount

        public Point FixationPoint { get; set; }
        public Point Offset { get; set; }  //Offset is the amount of pixels moved when repositioning the form if it is offscreen. It's used to reposition the Fixation point.
        //public Point SecondaryOffset { get; set; }  //Used for the Centered zoom offset from the sides..
        protected Form form;
        protected Timer updateTimer;
        protected RECT magWindowRect = new RECT();
        protected IntPtr hwndMag;
        public RECT sourceRect;
        FormsEyeXHost eyeXHost;
        GazePointDataStream gazeStream;
        protected bool zooming;
        public Point CurrentLook { get; set; }
        public float MaxZoom { get; set; } //Max zoom amount

        public Timer Timer { get { return updateTimer; } }

        protected bool hasInitialized;
        protected float magnification;

        protected FixationDetection fixationWorker;

        public ZoomMagnifier(Form displayform, FixationDetection fixationWorker)//, Point fixationPoint)
        {
            ZOOM_MAX = Program.readSettings.maxZoom;          //Max zoom amount
            Magnification = DO_ZOOM ? 1 : Program.readSettings.maxZoom; //Set magnification to the max if not zooming
            form = displayform;
            form.TopMost = true;
            updateTimer = new Timer();

            this.fixationWorker = fixationWorker;

            FixationPoint = new System.Drawing.Point();//fixationPoint;
            InitLens();
            sourceRect = new RECT();

            //Event handlers
            form.Resize += new EventHandler(form_Resize);
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            updateTimer.Tick += new EventHandler(timer_Tick);

            updateTimer.Interval = UPDATE_SPEED;
            updateTimer.Enabled = false;
            Offset = new Point(0, 0);
            //SecondaryOffset = new Point(0, 0);

            eyeXHost = new FormsEyeXHost();
            eyeXHost.Start();
            gazeStream = eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);
            gazeStream.Next += (s, e) => SetLook(e.X, e.Y);

            form.Left = -4000;
            form.Top = -5000;
            form.Width = 1;
            form.Height = 1;
            zooming = true;

        }

        private void SetLook(double x, double y)
        {
            CurrentLook = new Point((int)x, (int)y);
        }

        public void InitLens()
        {
            hasInitialized = NativeMethods.MagInitialize();
            if (hasInitialized)
            {
                IntPtr hInst = NativeMethods.GetModuleHandle(null);

                // Create a magnifier control that fills the client area.
                NativeMethods.GetClientRect(form.Handle, ref magWindowRect);
                hwndMag = NativeMethods.CreateWindow((int)ExtendedWindowStyles.WS_EX_CLIENTEDGE,
                    NativeMethods.WC_MAGNIFIER, "Zoom Lens",
                    (int)WindowStyles.WS_CHILD | (int)WindowStyles.WS_VISIBLE,
                    magWindowRect.left, magWindowRect.top, magWindowRect.right, magWindowRect.bottom,
                    form.Handle, IntPtr.Zero, hInst, IntPtr.Zero);

                // Set the magnification factor.
                Transformation matrix = new Transformation(Magnification);
                NativeMethods.MagSetWindowTransform(hwndMag, ref matrix);
            }

        }

        public virtual void UpdatePosition(Point fixationPoint)
        {
            Point zoomPosition = fixationPoint;
            Rectangle screenBounds = Screen.FromControl(form).Bounds;

            form.Left = zoomPosition.X - (form.Width / 2);
            form.Top = zoomPosition.Y - (form.Height / 2);

            int initLeft = form.Left;
            int initTop = form.Top;

            form.Left = Clamp(form.Left, 0, screenBounds.Width - form.Width);
            form.Top = Clamp(form.Top, 0, screenBounds.Height - form.Height);

            int finalLeft = form.Left;
            int finalTop = form.Top;

            Utils.Print("Position-", initLeft, initTop, finalLeft, finalTop);
            int offsetX = finalLeft - initLeft;
            int offsetY = finalTop - initTop;

            //Offset = new Point(offsetX, offsetY);
            Utils.Print("Offset-", offsetX, offsetY);
        }

        public virtual void UpdateMagnifier()
        {
            //If the magnifier is not setup correctly (will crash otherwise)
            if ((!hasInitialized) || (hwndMag == IntPtr.Zero) || !updateTimer.Enabled)
            {
                return;
            }
            PlaceZoomWindow();// FixationPoint);
        }

        public void PlaceZoomWindow()
        {
            Point fixationPoint = GetZoomPosition();
            Point zoomPosition =  fixationPoint;// Utils.SubtractPoints(GetZoomPosition(), Offset);
            Rectangle screenBounds = Screen.FromControl(form).Bounds;
            form.Width = 800;
            form.Height = 600;

            UpdatePosition(fixationPoint);

            int width = (int)(form.Width / Magnification);
            int height = (int)(form.Height / Magnification);

            //sourceRect.left = zoomPosition.X - (width / 2);
            //sourceRect.top = zoomPosition.Y - (height / 2);


            //if (Magnification < ZOOM_MAX + .1)
            //{
                sourceRect.left = fixationWorker.getXY().X - (width / 2);
                sourceRect.top = fixationWorker.getXY().Y - (width / 2);
            //}
            

            //int inLeft = sourceRect.left;
            //int inTop = sourceRect.top;

            sourceRect.left = Clamp(sourceRect.left, 0, screenBounds.Width - width);
            sourceRect.top = Clamp(sourceRect.top, 0, screenBounds.Height - height);

            //int fnLeft = sourceRect.left - inLeft;
            //int fnTop = sourceRect.top - inTop;

            /*
            if (SecondaryOffset.X == 0 && SecondaryOffset.Y == 0)
            {
                //     MessageBox.Show(SecondaryOffset.X + " " + SecondaryOffset.Y + " " + inLeft + " " + fnLeft);

                SecondaryOffset = new Point(fnLeft, fnTop);
            }
            */

            NativeMethods.MagSetWindowSource(hwndMag, sourceRect);  //Sets the source of the zoom
            NativeMethods.InvalidateRect(hwndMag, IntPtr.Zero, true); // Force redraw.
        }

        public void Zoom()
        {
            if (DO_ZOOM)
            {
                Magnification += ZOOM_SPEED;
            }
        }

        //Gets the position that the zoom will be centered on
        public Point GetZoomPosition()
        {
            return Utils.AddPoints(FixationPoint, Offset);
        }

        //TODO: move to utility class
        //Forces an int to be between two integers
        public int Clamp(int current, int min, int max)
        {
            return (current < min) ? min : (current > max) ? max : current;
        }

        public void ResetZoomValue()
        {
            Offset = new Point(0, 0);
            //SecondaryOffset = new Point(0, 0);
            Magnification = 1;// Program.readSettings.maxZoom;
            MaxZoom = magnification;
            Timer.Enabled = false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Zoom();
            UpdateMagnifier();
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            updateTimer.Enabled = false;
        }

        private void form_Resize(object sender, EventArgs e)
        {
            if (hasInitialized && (hwndMag != IntPtr.Zero))
            {
                NativeMethods.GetClientRect(form.Handle, ref magWindowRect);
                // Resize the control to fill the window.
                NativeMethods.SetWindowPos(hwndMag, IntPtr.Zero, magWindowRect.left,
                    magWindowRect.top, magWindowRect.right, magWindowRect.bottom, 0);
            }
        }

        //the magnification factor
        protected float Magnification
        {
            set
            {
                if (magnification != value)
                {
                    magnification = value;
                    if (magnification > ZOOM_MAX)
                    {
                        magnification = ZOOM_MAX;
                    }
                    // Set the magnification factor.
                    Transformation matrix = new Transformation(magnification);
                    NativeMethods.MagSetWindowTransform(hwndMag, ref matrix);
                }
            }
            get { return magnification; }
        }

        public void Stop()
        {
            updateTimer.Enabled = false;
            form.Left = -4000;
            form.Top = -5000;
            form.Width = 1;
            form.Height = 1;
            form.Refresh();
            form.Hide();
        }

        public virtual int MagnifierDivAmount()
        {
            return (int)ZOOM_MAX;
        }


        public Point GetLookPosition()
        {
            Point startPoint = new Point(sourceRect.left, sourceRect.top);
            Point actualLook = CurrentLook;
            Point formPos = new Point(form.Left, form.Top);
            Point adjustedPoint = Utils.SubtractPoints(actualLook, formPos);
            Point magAdjust = new Point((int)(adjustedPoint.X / ZOOM_MAX), (int)(adjustedPoint.Y / ZOOM_MAX));

            Point finalPoint = Utils.AddPoints(magAdjust, startPoint);

            //  Point finalPoint = adjustedPoint;//Utils.SubtractPoints(Utils.AddPoints(startPoint, adjustedPoint), 1);
            //  MessageBox.Show(adjustedPoint.X + " " + adjustedPoint.Y + " " + finalPoint.X + " " + finalPoint.Y);

            return finalPoint;
        }
    }
}

