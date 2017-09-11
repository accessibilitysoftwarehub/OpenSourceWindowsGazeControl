﻿using System.Drawing;
using System.Windows.Forms;

namespace GazeToolBar
{
    /*
        Date: 17/05/2016
        Name: Derek Dai
        Description: Put all the constant value or method in here

        Updated 28/8/17 - Sam Medlock
        Changed name from "ValueNeverChange" to "Constants", removed unnessasary/unused values
    */
    public static class Constants
    {
        public static readonly int MIN_TIME_LENGTH = 500;
        public static readonly int GAP_TIME_LENGTH = 200;
        public static readonly int DEFAULT_TIME_LENGTH = 1500;
        public static readonly int MIN_TIME_OUT = 4500;
        public static readonly int GAP_TIME_OUT = 500;
        public static readonly int DEFAULT_TIME_OUT = 7000;
        public static readonly string RES_NAME = "GazeToolBar";
        public static readonly string AUTO_START_ON = "Auto Start Is On";
        public static readonly string AUTO_START_OFF = "Auto Start Is OFF";
        public static readonly string KEY_FUNCTION_UNASSIGNED_MESSAGE = "N/A";
        public static readonly Size SCREEN_SIZE = Screen.PrimaryScreen.WorkingArea.Size;
        public static readonly Rectangle PRIMARY_SCREEN = Screen.PrimaryScreen.Bounds;
        public static readonly Color SelectedColor = Color.White;
        public static readonly Color SettingButtonColor = Color.Black;
    }
}