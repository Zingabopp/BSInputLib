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

        private static Valve.VR.VRControllerState_t state = new Valve.VR.VRControllerState_t();
        private static ControllerState _hmd;
        private static ControllerState _RightController;
        private static ControllerState _LeftController;

        public static ControllerState HMD
        {
            get
            {
                if (_hmd == null)
                    _hmd = new ControllerState(DeviceRole.HMD);
                return _hmd;
            }
        }
        public static ControllerState RightController
        {
            get
            {
                if (_RightController == null)
                    _RightController = new ControllerState(DeviceRole.RightHand);
                return _RightController;
            }
        }
        public static ControllerState LeftController
        {
            get
            {
                if (_LeftController == null)
                    _LeftController = new ControllerState(DeviceRole.LeftHand);
                return _LeftController;
            }
        }

        private static uint _leftID = 0;
        /// <summary>
        /// Device ID for the left controller.
        /// </summary>
        public static uint LeftControllerID
        {
            get
            {
                if (_leftID == 0)
                    _leftID = OpenVR.GetTrackedDeviceIndexForControllerRole(Valve.VR.ETrackedControllerRole.LeftHand);
                if (_leftID > 20)
                    _leftID = 0;
                return _leftID;
            }

        }

        private static uint _rightID = 0;
        /// <summary>
        /// Device ID for the right controller.
        /// </summary>
        public static uint RightControllerID
        {
            get
            {
                if (_rightID == 0)
                    _rightID = OpenVR.GetTrackedDeviceIndexForControllerRole(Valve.VR.ETrackedControllerRole.RightHand);
                if (_rightID > 20)
                    _rightID = 0;
                return _rightID;
            }

        }

        /// <summary>
        /// Below this amount, axis will read as 0.
        /// </summary>
        public static float AxisDeadzone = 0.0001f;

        /// <summary>
        /// Returns the controller ID for the specified controller.
        /// </summary>
        /// <param name="_controller"></param>
        /// <returns></returns>
        public static uint GetControllerID(DeviceRole _controller)
        {
            switch (_controller)
            {
                case DeviceRole.HMD:
                    return 0;
                case DeviceRole.LeftHand:
                    return LeftControllerID;
                case DeviceRole.RightHand:
                    return RightControllerID;
                default:
                    return 99;
            }
        }

        /// <summary>
        /// Returns the button mask for the specified Button.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static ulong GetButtonMask(Button button)
        {
            switch (button)
            {
                case Button.A:
                    return (1UL << (int) Valve.VR.EVRButtonId.k_EButton_A);
                case Button.AppMenu:
                    return (1UL << (int) Valve.VR.EVRButtonId.k_EButton_ApplicationMenu);
                case Button.Grip:
                    return (1UL << (int) Valve.VR.EVRButtonId.k_EButton_Grip);
                case Button.Touchpad:
                    return (1UL << (int) Valve.VR.EVRButtonId.k_EButton_Axis0);
                case Button.Trigger:
                    return (1UL << (int) Valve.VR.EVRButtonId.k_EButton_Axis1);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Returns true if the button is currently being pressed.
        /// </summary>
        /// <param name="controller">Device ID of the controller</param>
        /// <param name="button">Button mask of the button to check</param>
        /// <returns></returns>
        public static bool GetIsPressed(uint controller, ulong button)
        {
            if (controller == 0)
                return false;
            //var state = new Valve.VR.VRControllerState_t();
            var success = OpenVR.GetControllerState(controller, ref state, (uint) Marshal.SizeOf(typeof(Valve.VR.VRControllerState_t)));
            if (!success)
            {
                //Util.Logger.Error($"Couldn't get controller state: {controller}");
                return false;
            }
            return (state.ulButtonPressed & button) != 0;
        }

        /// <summary>
        /// Returns true if the Button is currently being pressed.
        /// </summary>
        /// <param name="controller">Device ID of the controller</param>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetIsPressed(uint controller, Button button)
        {
            return GetIsPressed(controller, GetButtonMask(button));
        }

        /// <summary>
        /// Returns true if the button is being touched (Oculus).
        /// </summary>
        /// <param name="controller">Device ID of the controller</param>
        /// <param name="button">Buttom mask of the button to check</param>
        /// <returns></returns>
        public static bool GetIsTouched(uint controller, ulong button)
        {
            if (controller == 0)
                return false;
            var locState = new Valve.VR.VRControllerState_t();
            var success = OpenVR.GetControllerState(controller, ref locState, (uint) Marshal.SizeOf(typeof(Valve.VR.VRControllerState_t)));
            if (!success)
                return false;
            return (state.ulButtonTouched & button) != 0;
        }

        /// <summary>
        /// Returns true if the button is being touched (Oculus).
        /// </summary>
        /// <param name="controller">Device ID of the controller.</param>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetIsTouched(uint controller, Button button)
        {
            return GetIsTouched(controller, GetButtonMask(button));
        }

        /// <summary>
        /// Returns an AxisValue with the X and Y values of the specified Axis.
        /// </summary>
        /// <param name="controller">Device ID of the controller</param>
        /// <param name="axis">Axis to check.</param>
        /// <returns></returns>
        public static AxisValue GetAxisValue(uint controller, Axis axis)
        {
            //var state = new Valve.VR.VRControllerState_t();
            var success = OpenVR.GetControllerState(controller, ref state, (uint) Marshal.SizeOf(typeof(Valve.VR.VRControllerState_t)));
            switch (axis)
            {
                case Axis.Axis0:
                    return new AxisValue(state.rAxis0);
                case Axis.Axis1:
                    return new AxisValue(state.rAxis1);
                case Axis.Axis2:
                    return new AxisValue(state.rAxis2);
                case Axis.Axis3:
                    return new AxisValue(state.rAxis3);
                case Axis.Axis4:
                    return new AxisValue(state.rAxis4);
                default:
                    return new AxisValue(0, 0);
            }
        }

        private static Valve.VR.VREvent_t vrEvent;
        private static uint vrEventSize = (uint) Marshal.SizeOf(typeof(Valve.VR.VREvent_t));
        private static Valve.VR.ETrackedPropertyError etpErr;
        public static void CheckEvents()
        {
            try
            {
                while (OpenVRInput.OpenVR.PollNextEvent(ref vrEvent, vrEventSize))
                {
                    switch (vrEvent.eventType)
                    {
                        case (uint) Valve.VR.EVREventType.VREvent_ButtonPress:
                            HandleButtonPress(vrEvent);
                            break;
                        case (uint) Valve.VR.EVREventType.VREvent_ButtonUnpress:
                            HandleButtonUnpress(vrEvent);
                            break;
                        case (uint) Valve.VR.EVREventType.VREvent_ButtonTouch:
                            HandleButtonTouch(vrEvent);
                            break;
                        case (uint) Valve.VR.EVREventType.VREvent_ButtonUntouch:
                            HandleButtonUntouch(vrEvent);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Logger.Exception("Exception getting event", ex);
            }
        }

        private static void HandleButtonPress(Valve.VR.VREvent_t vrEvt)
        {
            Util.Logger.Debug($"Button pressed on device {vrEvt.trackedDeviceIndex} ({GetDeviceRoleFromDevIndex(vrEvt.trackedDeviceIndex).ToString()}): {vrEvt.data.controller.button}");
            StringBuilder serialBuffer = new StringBuilder();
            OpenVR.GetStringTrackedDeviceProperty(vrEvt.trackedDeviceIndex, Valve.VR.ETrackedDeviceProperty.Prop_ModelNumber_String, serialBuffer, 65535, ref etpErr);
            //Console.WriteLine($"Error State: {etpErr.ToString()}");
            Util.Logger.Debug($"Model: {serialBuffer}");
        }

        private static void HandleButtonUnpress(Valve.VR.VREvent_t vrEvt)
        {
            Util.Logger.Debug($"Button released on device {vrEvt.trackedDeviceIndex} ({GetDeviceRoleFromDevIndex(vrEvt.trackedDeviceIndex).ToString()}): {vrEvt.data.controller.button}");
        }

        private static void HandleButtonTouch(Valve.VR.VREvent_t vrEvt)
        {
            Util.Logger.Debug($"Button touched on device {vrEvt.trackedDeviceIndex} ({GetDeviceRoleFromDevIndex(vrEvt.trackedDeviceIndex).ToString()}): {vrEvt.data.controller.button}");
        }

        private static void HandleButtonUntouch(Valve.VR.VREvent_t vrEvt)
        {
            Util.Logger.Debug($"Button untouched on device {vrEvt.trackedDeviceIndex} ({GetDeviceRoleFromDevIndex(vrEvt.trackedDeviceIndex).ToString()}): {vrEvt.data.controller.button}");
        }

        private static DeviceRole GetDeviceRoleFromDevIndex(uint devIndex)
        {
            return GetDeviceRoleFromRoleHint(SteamVR.instance.hmd.GetInt32TrackedDeviceProperty(devIndex, Valve.VR.ETrackedDeviceProperty.Prop_ControllerRoleHint_Int32, ref etpErr));
        }

        private static DeviceRole GetDeviceRoleFromRoleHint(int role)
        {
            DeviceRole ctrl = DeviceRole.Unknown;
            switch (role)
            {
                case 0:
                    ctrl = DeviceRole.HMD;
                    break;
                case 1:
                    ctrl = DeviceRole.LeftHand;
                    break;
                case 2:
                    ctrl = DeviceRole.RightHand;
                    break;
                default:
                    break;
            }
            return ctrl;
        }
    }
}
