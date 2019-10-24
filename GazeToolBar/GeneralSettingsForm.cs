﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EyeXFramework.Forms;

namespace GazeToolBar
{
    public partial class GeneralSettingsForm : Form
    {
        private SettingsHome home;
        private static FormsEyeXHost eyeXHost;
        private Form1 form1;
        public GeneralSettingsForm(SettingsHome home, Form1 form1, FormsEyeXHost EyeXHost)
        {
            eyeXHost = EyeXHost;
            InitializeComponent();
            this.home = home;
            this.form1 = form1;
            this.BackColor = Program.readSettings.mainColour;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Method to update a trackbar up or down in value. Used by both trackpads.
        private void changeTrackBarValue(TrackBar trackbar, String IncrementOrDecrement)
        {
            switch (IncrementOrDecrement)
            {
                case "I":
                    if (trackbar.Value != trackbar.Maximum) { trackbar.Value = ++trackbar.Value; }
                    break;
                case "D":
                    if (trackbar.Value != trackbar.Minimum) { trackbar.Value = --trackbar.Value; }
                    break;
            }
            trackbar.Update();
        }

        private void btnFixTimeLengthMins_Click(object sender, EventArgs e)
        {
            changeTrackBarValue(trackBarFixTimeLength, "D");
        }

        private void btnFixTimeLengthPlus_Click(object sender, EventArgs e)
        {
            changeTrackBarValue(trackBarFixTimeLength, "I");
        }

        private void btnFixTimeOutMins_Click(object sender, EventArgs e)
        {
            changeTrackBarValue(trackBarFixTimeOut, "D");
        }

        private void btnFixTimeOutPlus_Click(object sender, EventArgs e)
        {
            changeTrackBarValue(trackBarFixTimeOut, "I");
        }

        private void btnAutoStart_Click(object sender, EventArgs e)
        {
            if (Program.onStartUp)
            {
                ChangeButtonColor(btnAutoStart, false, false);
            }
            else
            {
                ChangeButtonColor(btnAutoStart, true, false);
            }

            form1.OnStartTextChange();
        }

        public void ChangeButtonColor(Button button, bool onOff, bool hasText)
        {

            button.BackColor = onOff ? Constants.SelectedColor : Constants.SettingButtonColor;
            if (hasText)
            {
                if (onOff)
                {
                    button.ForeColor = Color.Black;
                }
                else
                {
                    button.ForeColor = Color.White;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                Program.readSettings.fixationTimeLength = trackBarFixTimeLength.Value * Constants.GAP_TIME_LENGTH + Constants.MIN_TIME_LENGTH;
                Program.readSettings.fixationTimeOut = trackBarFixTimeOut.Value * Constants.GAP_TIME_OUT + Constants.MIN_TIME_OUT;
                if (Program.onStartUp)
                {
                    AutoStart.SetOff();
                    Program.onStartUp = !Program.onStartUp;
                }
                else
                {
                    if (AutoStart.SetOn())
                    {
                        Program.onStartUp = !Program.onStartUp;
                    }
                }

                form1.OnStartTextChange();

                Close();
                //form1.NotifyIcon.BalloonTipTitle = "Saving success";
                //form1.NotifyIcon.BalloonTipText = "Your settings are successfuly saved";
                this.Close();
                form1.stateManager.ResetMagnifier();
                //form1.NotifyIcon.ShowBalloonTip(2000);
            }
            catch (Exception exception)
            {
                //form1.NotifyIcon.BalloonTipTitle = "Saving error";
                //form1.NotifyIcon.BalloonTipText = "For some reason, your settings are not successfuly saved, click me to show error message";
                //form1.NotifyIcon.Tag = exception.Message;
                this.Close();
                //form1.NotifyIcon.BalloonTipClicked += NotifyIcon_BalloonTipClicked;
                //form1.NotifyIcon.ShowBalloonTip(5000);
            }
        }

        private void GeneralSettingsForm_Load(object sender, EventArgs e)
        {
            if (Program.onStartUp)
            {
                ChangeButtonColor(btnAutoStart, true, false);
            }
            else
            {
                ChangeButtonColor(btnAutoStart, false, false);
            }


            //TODO: Need to be replaced
            trackBarFixTimeLength.Value = (Program.readSettings.fixationTimeLength - Constants.MIN_TIME_LENGTH) / Constants.GAP_TIME_LENGTH;
            trackBarFixTimeOut.Value = (Program.readSettings.fixationTimeOut - Constants.MIN_TIME_OUT) / Constants.GAP_TIME_OUT;
        }
    }
}
