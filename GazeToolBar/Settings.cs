﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
namespace GazeToolBar
{
    class Settings
    {
        public int fixationTimeLength { get; set; }
        public int fixationTimeOut { get; set; }
        public string leftClick { get; set; }
        public string doubleClick { get; set; }
        public string rightClick { get; set; }
        public string scroll { get; set; }
        public string micInput { get; set; }
        public string micInputOff { get; set; }
        public string[] sidebar { get; set; }
        public int maxZoom { get; set; }
        public int Crosshair { get; set; }
        public int zoomWindowSize { get; set; }
        public bool stickyLeftClick { get; set; }
        public bool selectionFeedback { get; set; }
        public bool dynamicZoom { get; set; }

        public bool centerZoom { get; set; }

        public bool eng { get; set; }

        public bool k123 { get; set; }
        public bool kacc { get; set; }

        public bool autocomplete { get; set; }

        public bool spanish { get; set; }

        SettingJSON settingsJson;
        public Settings(string path)
        {
            if (!File.Exists(path))
            {
                defaultSettings();
            }
            else
            {
                string s = File.ReadAllText(path);
                settingsJson = JsonConvert.DeserializeObject<SettingJSON>(s);
                fixationTimeLength = settingsJson.fixationTimeLength;
                fixationTimeOut = settingsJson.fixationTimeOut;
                leftClick = settingsJson.leftClick;
                doubleClick = settingsJson.doubleClick;
                rightClick = settingsJson.rightClick;
                scroll = settingsJson.scroll;
                micInput = settingsJson.micInput;
                micInputOff = settingsJson.micInputOff;
                sidebar = settingsJson.sidebar;
                maxZoom = settingsJson.maxZoom;
                Crosshair = settingsJson.Crosshair;
                zoomWindowSize = settingsJson.zoomWindowSize;
                stickyLeftClick = settingsJson.stickyLeftClick;
                selectionFeedback = settingsJson.selectionFeedback;
                dynamicZoom = settingsJson.dynamicZoom;                                  //Changed for TESTINGVARIABLE
                centerZoom = settingsJson.centerZoom;
                eng = true;
                k123 = true;
                kacc = true;
                spanish = true;
                autocomplete = true;
                //dynamicZoom = false;
                //centerZoom = false;
                Console.WriteLine("Settings loaded");
            }
        }


        public void createJSON(string[] side)
        {
            SettingJSON setting = new SettingJSON();

            setting.fixationTimeLength = fixationTimeLength;
            setting.fixationTimeOut = fixationTimeOut;
            setting.leftClick = leftClick;
            setting.doubleClick = doubleClick;
            setting.rightClick = rightClick;
            setting.scroll = scroll;
            setting.sidebar = sidebar;
            setting.Crosshair = Crosshair;
            setting.maxZoom = maxZoom;
            setting.zoomWindowSize = zoomWindowSize;
            setting.stickyLeftClick = stickyLeftClick;
            setting.selectionFeedback = selectionFeedback;
            setting.dynamicZoom = dynamicZoom;                                  //Changed for TESTINGVARIABLE
            setting.centerZoom = centerZoom;
            setting.eng = eng;
            setting.k123 = k123;
            setting.kacc = kacc;
            setting.spanish = spanish;
            setting.autocomplete = autocomplete;
            //setting.dynamicZoom = false;
            //centerZoom = false;
            string settings = JsonConvert.SerializeObject(setting);
            File.WriteAllText(Program.path, settings);
        }


        public void defaultSettings()
        {
            fixationTimeLength = Constants.DEFAULT_TIME_LENGTH;
            fixationTimeOut = Constants.DEFAULT_TIME_OUT;
            leftClick = Constants.KEY_FUNCTION_UNASSIGNED_MESSAGE;
            doubleClick = Constants.KEY_FUNCTION_UNASSIGNED_MESSAGE;
            rightClick = Constants.KEY_FUNCTION_UNASSIGNED_MESSAGE;
            scroll = Constants.KEY_FUNCTION_UNASSIGNED_MESSAGE;
            micInput = Constants.KEY_FUNCTION_UNASSIGNED_MESSAGE;
            micInputOff = Constants.KEY_FUNCTION_UNASSIGNED_MESSAGE;
            sidebar = new string[] { "right_click", "left_click", "double_left_click", "scroll", "keyboard", "settings" };
            maxZoom = 3;
            Crosshair = 3;
            zoomWindowSize = 10;
            stickyLeftClick = false;
            selectionFeedback = true;
            dynamicZoom = false;                                  //Changed for TESTINGVARIABLE
            centerZoom = false;
        }



    }
}
