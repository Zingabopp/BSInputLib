using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionPlugin;

namespace BSInputLib
{
    public static class OpenVRInput
    {
        public static Valve.VR.CVRSystem OpenVR
        {
            get
            {
                return SteamVR.instance.hmd;
            }
        }

        public readonly static ControllerState RightController = new ControllerState(Controller.RightHand);
        public readonly static ControllerState LeftController = new ControllerState(Controller.LeftHand);

        private static uint _leftID = 0;
        public static uint LeftControllerID
        {
            get
            {
                if (_leftID == 0)
                    _leftID = OpenVR.GetTrackedDeviceIndexForControllerRole(Valve.VR.ETrackedControllerRole.LeftHand);
                return _leftID;
            }

        }

        private static uint _rightID = 0;
        public static uint RightControllerID
        {
            get
            {
                if (_rightID == 0)
                    _rightID = OpenVR.GetTrackedDeviceIndexForControllerRole(Valve.VR.ETrackedControllerRole.RightHand);
                return _rightID;
            }

        }
        public static float AxisDeadzone = 0.0001f;

        public static uint GetControllerID(Controller _controller)
        {
            switch (_controller)
            {
                case Controller.LeftHand:
                    return LeftControllerID;
                case Controller.RightHand:
                    return RightControllerID;
                default:
                    return 0;
            }
        }

        public static ulong GetButtonMask(Button button)
        {
            ulong mask = 0;
            switch (button)
            {
                case Button.A:
                    mask = (1UL << (int) Valve.VR.EVRButtonId.k_EButton_A);
                    break;
                case Button.B:
                    mask = (1UL << (int) Valve.VR.EVRButtonId.k_EButton_ApplicationMenu);
                    break;
                case Button.Grip:
                    mask = (1UL << (int) Valve.VR.EVRButtonId.k_EButton_Grip);
                    break;
                case Button.Joystick:
                    mask = (1UL << (int) Valve.VR.EVRButtonId.k_EButton_Axis0);
                    break;
                case Button.Trigger:
                    mask = (1UL << (int) Valve.VR.EVRButtonId.k_EButton_Axis1);
                    break;
                default:
                    break;
            }
            return mask;
        }

        public static bool GetIsPressed(uint controller, ulong button)
        {
            if (controller == 0)
                return false;
            var state = new Valve.VR.VRControllerState_t();
            var success = OpenVR.GetControllerState(controller, ref state, (uint) Marshal.SizeOf(typeof(Valve.VR.VRControllerState_t)));
            return (state.ulButtonPressed & button) != 0;
        }

        public static bool GetIsPressed(uint controller, Button button)
        {
            return GetIsPressed(controller, GetButtonMask(button));
        }

        public static bool GetIsTouched(uint controller, ulong button)
        {
            if (controller == 0)
                return false;
            var state = new Valve.VR.VRControllerState_t();
            var success = OpenVR.GetControllerState(controller, ref state, (uint) Marshal.SizeOf(typeof(Valve.VR.VRControllerState_t)));
            return (state.ulButtonTouched & button) != 0;
        }

        public static bool GetIsTouched(uint controller, Button button)
        {
            return GetIsTouched(controller, GetButtonMask(button));
        }

        public static AxisValue GetAxisValue(uint controller, Axis axis)
        {
            var state = new Valve.VR.VRControllerState_t();
            var success = OpenVR.GetControllerState(controller, ref state, (uint) Marshal.SizeOf(typeof(Valve.VR.VRControllerState_t)));
            switch (axis)
            {
                case Axis.Joystick:
                    return new AxisValue(state.rAxis0);
                case Axis.Trigger:
                    return new AxisValue(state.rAxis1);
                case Axis.Grip:
                    return new AxisValue(state.rAxis2);
                case Axis.Axis3:
                    return new AxisValue(state.rAxis3);
                case Axis.Axis4:
                    return new AxisValue(state.rAxis4);
                default:
                    return new AxisValue(0, 0);
            }
        }
    }

    public class ControllerState
    {
        private readonly uint _controllerID;

        private bool _Button_A_Pressed;
        private bool _Button_B_Pressed;
        private bool _Grip_Pressed;
        private bool _Joystick_Pressed;
        private bool _Trigger_Pressed;

        private bool _Button_A_Touched;
        private bool _Button_B_Touched;
        private bool _Grip_Touched;
        private bool _Joystick_Touched;
        private bool _Trigger_Touched;

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
                    case Button.B:
                        _Button_B_Pressed = val;
                        break;
                    case Button.Grip:
                        _Grip_Pressed = val;
                        break;
                    case Button.Joystick:
                        _Joystick_Pressed = val;
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
                    case Button.B:
                        _Button_B_Touched = val;
                        break;
                    case Button.Grip:
                        _Grip_Touched = val;
                        break;
                    case Button.Joystick:
                        _Joystick_Touched = val;
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

        public AxisValue GetAxisValue(Axis axis)
        {
            return OpenVRInput.GetAxisValue(_controllerID, axis);
        }

        public bool GetLastState(Button button, InputType _type)
        {
            bool val = false;
            if (_type == InputType.Press)
            {
                switch (button)
                {
                    case Button.A:
                        val = _Button_A_Pressed;
                        break;
                    case Button.B:
                        val = _Button_B_Pressed;
                        break;
                    case Button.Grip:
                        val = _Grip_Pressed;
                        break;
                    case Button.Joystick:
                        val = _Joystick_Pressed;
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
                    case Button.B:
                        val = _Button_B_Touched;
                        break;
                    case Button.Grip:
                        val = _Grip_Touched;
                        break;
                    case Button.Joystick:
                        val = _Joystick_Touched;
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

        public ControllerState(Controller ctlr)
        {
            _controllerID = OpenVRInput.GetControllerID(ctlr);
        }

        public void UpdateState()
        {
            if (ButtonPressChanged != null)
            {
                GetButtonPressed(Button.A);
                GetButtonPressed(Button.B);
                GetButtonPressed(Button.Grip);
                GetButtonPressed(Button.Joystick);
                GetButtonPressed(Button.Trigger);
            }
            if (ButtonTouchChanged != null)
            {
                GetButtonTouched(Button.A);
                GetButtonTouched(Button.B);
                GetButtonTouched(Button.Grip);
                GetButtonTouched(Button.Joystick);
                GetButtonTouched(Button.Trigger);
            }
        }

        public event Action<Button, bool> ButtonPressChanged;
        public event Action<Button, bool> ButtonTouchChanged;
    }

    public enum Axis
    {
        Joystick, // Vive touchpad, Oculus joystick, Axis0
        Trigger, // Vive/Oculus trigger, Axis1
        Grip,  // Vive?, Oculus grip, Axis2
        Axis3,
        Axis4
    }

    public enum Button
    {
        A,
        B,
        Grip,
        Joystick,
        Trigger
    }

    public enum Controller
    {
        LeftHand,
        RightHand
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
