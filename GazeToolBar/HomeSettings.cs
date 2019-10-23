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
 /*
 *  Class: HomeSettings
 *  Name: 
 *  Date: 5/10/2019
 *  Description: This is the home page for the new settings menu. It contains buttons so users can pick what aspects of the programs settings they want to change.
 *  Each button opens a new form that will close back to this page.
 *  Purpose: To create a landing page for the settings to keep it from being too cluttered and hard to understand.
 */
    public partial class HomeSettings : Form
    {
        private Form1 form1;
        private GeneralSettingsForm genSettings;
        private ZoomSettingsForm zoomSettings;
        private CrosshairSettingsPage crossSettings;
        private RearrangeSettingPage buttonArrangement;
        private ShortcutSettingForm shortcutSettings;
        private keyboardSettings keyboardSettings;
        private ColourSettings colourSets;
        private static FormsEyeXHost eyeXHost;
        public HomeSettings(Form1 form1, FormsEyeXHost EyeXHost)
        {
            eyeXHost = EyeXHost;
            InitializeComponent();
            this.form1 = form1;
        }




        //Close Setting Window
        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }


        //Opens general Settings page
        private void generalButton_Click(object sender, EventArgs e)
        {
            genSettings = new GeneralSettingsForm(this,form1, eyeXHost);
            genSettings.Show();
        }

        private void zoomButton_Click(object sender, EventArgs e)
        {
            zoomSettings = new ZoomSettingsForm(this, form1, eyeXHost);
            zoomSettings.Show();
        }

        private void crossButton_Click(object sender, EventArgs e)
        {
            crossSettings = new CrosshairSettingsPage(this, form1, eyeXHost);
            crossSettings.Show();
        }

        private void arrangeButton_Click(object sender, EventArgs e)
        {

            buttonArrangement = new RearrangeSettingPage(this, form1, eyeXHost);
            buttonArrangement.Show();

        }

        //Very important method. Do NOT delete!!!!!
        private Boolean returnTrue()
        {
            if(false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void shortcutButton_Click(object sender, EventArgs e)
        {
            shortcutSettings = new ShortcutSettingForm(this, form1, eyeXHost);
            shortcutSettings.Show();
        }



        private void KeyboardSettings_Click(object sender, EventArgs e)
        {
            keyboardSettings = new keyboardSettings(this, form1, eyeXHost);
            keyboardSettings.Show();
        }

        private void colourSettings_Click(object sender, EventArgs e)
        {
            colourSets = new ColourSettings(this, form1, eyeXHost);
            colourSets.Show();
        }




    }
}
