using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSInputLib
{
    public class ControllerMapping
    {
        private const string OculusTouchModel = "Oculus Rift CV1";
        private const string ViveWandModel = "ViveWand-NOTCORRECT";
        private const string ValveKnucklesModel = "Knuckles-NOTCORRECT";
        private const string WMRModel = "WMR-NOTCORRECT";
        public readonly string ControllerType;

        private Dictionary<int, ButtonMap> _buttons;
        public Dictionary<int, ButtonMap> Buttons
        {
            get
            {
                if (_buttons == null)
                    _buttons = new Dictionary<int, ButtonMap>();
                return _buttons;
            }
        }

        private Dictionary<int, string> _axis;
        public Dictionary<int, string> Axis
        {
            get
            {
                if (_axis == null)
                    _axis = new Dictionary<int, string>();
                return _axis;
            }
        }

        public ControllerMapping(string controllerModel)
        {
            ControllerType = controllerModel;
            switch (ControllerType)
            {
                case OculusTouchModel:
                    MapForOculusTouch();
                    break;
                case ViveWandModel:
                    MapForViveWand();
                    break;
                case WMRModel:
                    MapForWMR();
                    break;
                default:
                    break;
            }
        }

        public int GetButtonIdByName(string name)
        {
            int buttonID = -1;
            foreach (var key in Buttons.Keys)
            {
                if(Buttons[key].Name == name)
                {
                    buttonID = key;
                    break;
                }
            }
            return buttonID;
        }

        public ButtonMap GetButtonByName(string name)
        {
            return Buttons[GetButtonIdByName(name)];
        }

        private void MapForOculusTouch()
        {
            Util.Logger.Debug("Controller mapping for Oculus Touch");
            Buttons.Add(1, new ButtonMap("B"));
            Buttons.Add(2, new ButtonMap("Grip"));
            Buttons.Add(7, new ButtonMap("A"));
            Buttons.Add(31, new ButtonMap("ProximitySensor"));
            Buttons.Add(32, new ButtonMap("Joystick"));
            Buttons.Add(33, new ButtonMap("Trigger"));

            Axis.Add(0, "Joystick");
            Axis.Add(1, "Trigger");
            Axis.Add(2, "Grip");
        }

        private void MapForViveWand()
        {
            Util.Logger.Debug("Controller mapping for Vive Wand");
            Buttons.Add(1, new ButtonMap("ApplicationMenu"));
            Buttons.Add(2, new ButtonMap("Grip"));
            Buttons.Add(7, new ButtonMap("A"));
            Buttons.Add(31, new ButtonMap("ProximitySensor"));
            Buttons.Add(32, new ButtonMap("Touchpad"));
            Buttons.Add(33, new ButtonMap("Trigger"));
            
            Axis.Add(0, "Touchpad");
            Axis.Add(1, "Trigger");
        }

        private void MapForWMR()
        {
            Util.Logger.Debug("Controller mapping for WMR");
            Buttons.Add(1, new ButtonMap("ApplicationMenu"));
            Buttons.Add(2, new ButtonMap("Grip"));
            Buttons.Add(7, new ButtonMap("A"));
            Buttons.Add(31, new ButtonMap("ProximitySensor"));
            //Buttons.Add(32, "Touchpad"); // Doesn't work on WMR, brings up a menu?
            Buttons.Add(33, new ButtonMap("Trigger"));
            Buttons.Add(34, new ButtonMap("Joystick"));

            Axis.Add(0, "Touchpad");
            Axis.Add(1, "Trigger");
            Axis.Add(2, "Joystick");

        }
    }
}
