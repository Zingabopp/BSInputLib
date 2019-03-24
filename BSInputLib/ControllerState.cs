using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSInputLib
{

    public class ControllerState
    {
        private uint _controllerID;
        private uint ControllerID
        {
            get
            {
                return _controllerID;
            }
            set
            {
                _controllerID = value;
                if (_controllerID < 50)
                {
                    SetControllerModel();
                }
            }
        }
        public readonly DeviceRole Controller;
        private string _controllerModel = "Unknown";
        public string ControllerModel
        {
            get
            {
                return _controllerModel;
            }
        }
        private StringBuilder str = new StringBuilder();
        private bool _Button_A_Pressed;
        private bool _Button_AppMenu_Pressed;
        private bool _Grip_Pressed;
        private bool _Touchpad_Pressed;
        private bool _Trigger_Pressed;

        private bool _Button_A_Touched;
        private bool _Button_AppMenu_Touched;
        private bool _Grip_Touched;
        private bool _Touchpad_Touched;
        private bool _Trigger_Touched;

        public ControllerState(DeviceRole ctlr)
        {
            _controllerID = OpenVRInput.GetControllerID(ctlr);
            Controller = ctlr;
            SetControllerModel();
            Util.Logger.Debug($"New ControllerState for {Controller.ToString()}, device ID {_controllerID}");
        }

        private bool SetControllerModel()
        {
            bool success = false;
            str.Clear();
            Valve.VR.ETrackedPropertyError etpErr = new Valve.VR.ETrackedPropertyError();
            OpenVRInput.OpenVR.GetStringTrackedDeviceProperty(_controllerID, Valve.VR.ETrackedDeviceProperty.Prop_ModelNumber_String, str, 65535, ref etpErr);
            if (etpErr == Valve.VR.ETrackedPropertyError.TrackedProp_Success)
            {
                _controllerModel = str.ToString();
                Util.Logger.Debug($"Controller for {Controller.ToString()} model: {ControllerModel}");
                success = true;
            }
            else
                Util.Logger.Warning($"Error getting controller model name: {etpErr.ToString()}");
            str.Clear();
            return success;
        }

        /// <summary>
        /// Checks if the specified Button is being pressed.
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns></returns>
        public bool GetButtonPressed(Button button)
        {
            bool val = OpenVRInput.GetIsPressed(_controllerID, button);

            if (val != GetLastState(button, InputType.Press))
            {
                switch (button)
                {
                    case Button.A:
                        _Button_A_Pressed = val;
                        break;
                    case Button.AppMenu:
                        _Button_AppMenu_Pressed = val;
                        break;
                    case Button.Grip:
                        _Grip_Pressed = val;
                        break;
                    case Button.Touchpad:
                        _Touchpad_Pressed = val;
                        break;
                    case Button.Trigger:
                        _Trigger_Pressed = val;
                        break;
                    default:
                        break;
                }
                ButtonPressChanged(button, val);
            }
            return val;
        }

        /// <summary>
        /// Checks if the specified Button is being touched (Oculus)
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns></returns>
        public bool GetButtonTouched(Button button)
        {
            bool val = OpenVRInput.GetIsTouched(_controllerID, button);

            if (val != GetLastState(button, InputType.Touch))
            {
                switch (button)
                {
                    case Button.A:
                        _Button_A_Touched = val;
                        break;
                    case Button.AppMenu:
                        _Button_AppMenu_Touched = val;
                        break;
                    case Button.Grip:
                        _Grip_Touched = val;
                        break;
                    case Button.Touchpad:
                        _Touchpad_Touched = val;
                        break;
                    case Button.Trigger:
                        _Trigger_Touched = val;
                        break;
                    default:
                        break;
                }
                ButtonTouchChanged(button, val);
            }
            return val;
        }

        /// <summary>
        /// Gets the AxisValue of the specified Axis.
        /// </summary>
        /// <param name="axis">Axis to check</param>
        /// <returns></returns>
        public AxisValue GetAxisValue(Axis axis)
        {
            return OpenVRInput.GetAxisValue(_controllerID, axis);
        }

        /// <summary>
        /// Returns the last known state of the specified button without checking for the current state.
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <param name="_type">State type: Press or Touch</param>
        /// <returns></returns>
        public bool GetLastState(Button button, InputType _type = InputType.Press)
        {
            bool val = false;
            if (_type == InputType.Press)
            {
                switch (button)
                {
                    case Button.A:
                        val = _Button_A_Pressed;
                        break;
                    case Button.AppMenu:
                        val = _Button_AppMenu_Pressed;
                        break;
                    case Button.Grip:
                        val = _Grip_Pressed;
                        break;
                    case Button.Touchpad:
                        val = _Touchpad_Pressed;
                        break;
                    case Button.Trigger:
                        val = _Trigger_Pressed;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (button)
                {
                    case Button.A:
                        val = _Button_A_Touched;
                        break;
                    case Button.AppMenu:
                        val = _Button_AppMenu_Touched;
                        break;
                    case Button.Grip:
                        val = _Grip_Touched;
                        break;
                    case Button.Touchpad:
                        val = _Touchpad_Touched;
                        break;
                    case Button.Trigger:
                        val = _Trigger_Touched;
                        break;
                    default:
                        break;
                }
            }

            return val;
        }


        /// <summary>
        /// Check the state of all buttons if there is at least one listener in the Changed events.
        /// </summary>
        public void UpdateState()
        {
            if (_controllerID > 20)
            {
                _controllerID = OpenVRInput.GetControllerID(Controller);
                return;
            }
            if (true)//ButtonPressChanged != null)
            {
                GetButtonPressed(Button.A);
                GetButtonPressed(Button.AppMenu);
                GetButtonPressed(Button.Grip);
                GetButtonPressed(Button.Touchpad);
                GetButtonPressed(Button.Trigger);
            }
            if (ButtonTouchChanged != null)
            {
                GetButtonTouched(Button.A);
                GetButtonTouched(Button.AppMenu);
                GetButtonTouched(Button.Grip);
                GetButtonTouched(Button.Touchpad);
                GetButtonTouched(Button.Trigger);
            }
            //Console.WriteLine(this.ToString());
        }

        public override string ToString()
        {
            str.Clear();
            str.Append("Controller: ");
            str.AppendLine((Controller == DeviceRole.RightHand) ? "Right" : "Left");
            str.AppendLine($"Button A Pressed: {_Button_A_Pressed}");
            str.AppendLine($"Button AppMenu Pressed: {_Button_AppMenu_Pressed}");
            str.AppendLine($"Button Grip Pressed: {_Grip_Pressed}");
            str.AppendLine($"Button Touchpad Pressed: {_Touchpad_Pressed}");
            str.AppendLine($"Button Trigger Pressed: {_Trigger_Pressed}");
            if (ButtonTouchChanged != null)
            {
                str.AppendLine($"Button A Touched: {_Button_A_Touched}");
                str.AppendLine($"Button AppMenu Touched: {_Button_AppMenu_Touched}");
                str.AppendLine($"Button Grip Touched: {_Grip_Touched}");
                str.AppendLine($"Button Touchpad Touched: {_Touchpad_Touched}");
                str.AppendLine($"Button Trigger Touched: {_Trigger_Touched}");
            }

            return str.ToString();
        }

        public event Action<Button, bool> ButtonPressChanged;
        public event Action<Button, bool> ButtonTouchChanged;
    }

    public enum Axis
    {
        Axis0, // Vive touchpad, Oculus joystick, WMR touchpad, Axis0
        Axis1, // Vive/Oculus/WMR trigger, Axis1
        Axis2,  // Vive?, Oculus grip, WMR joystick, Axis2
        Axis3,
        Axis4
    }

    public enum Button
    {
        A, // Does not exist on WMR or Vive?
        AppMenu, // Application menu button
        Grip,
        Touchpad, // Maybe doesn't work on WMR, brings up menu?
        Trigger
    }

    public enum DeviceRole
    {
        HMD = 0,
        LeftHand = 1,
        RightHand = 2,
        Unknown = 99
    }

    public enum InputType
    {
        Press,
        Touch
    }

    public struct AxisValue
    {
        public float x;
        public float y;

        public AxisValue(float _x, float _y)
        {
            x = _x;
            y = _y;
        }

        public AxisValue(Valve.VR.VRControllerAxis_t axis)
        {
            x = axis.x;
            y = axis.y;
            if (Math.Abs(x) < OpenVRInput.AxisDeadzone)
                x = 0;
            if (Math.Abs(y) < OpenVRInput.AxisDeadzone)
                y = 0;
        }

        public override string ToString()
        {
            return $"X: {x}, Y: {y}";
        }
    }
}
