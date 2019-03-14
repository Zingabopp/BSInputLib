using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionPlugin;
using VRUIControls;
using UnityEngine.XR;


namespace BSInputLib
{
    public class Plugin : IPlugin
    {
        public static string PluginName = "BSInputLib";
        public string Name => PluginName;
        public string Version => "0.0.1";

        public void OnApplicationStart()
        {
            Logger.LogLevel = LogLevel.Debug;

            //SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            //SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            OpenVRInput.RightController.ButtonPressChanged += onRightButton;
            OpenVRInput.LeftController.ButtonPressChanged += onLeftButton;

            OpenVRInput.RightController.ButtonTouchChanged += onRightButtonTouched;
            OpenVRInput.LeftController.ButtonTouchChanged += onLeftButtonTouched;
            

        }

        private void SceneManagerOnActiveSceneChanged(Scene oldScene, Scene newScene)
        {

            if (newScene.name == "Menu")
            {
                //Code to execute when entering The Menu

            }

            if (newScene.name == "GameCore")
            {
                //Code to execute when entering actual gameplay


            }


        }

        public void onRightButton(Button button, bool newState)
        {
            try
            {
                string state = newState ? "Pressed" : "Released";
                switch (button)
                {
                    case Button.A:
                        Logger.Debug($"Right Button A {state}");
                        break;
                    case Button.B:
                        Logger.Debug($"Right Button B {state}");
                        break;
                    case Button.Grip:
                        Logger.Debug($"Right Button Grip {state}");
                        break;
                    case Button.Joystick:
                        Logger.Debug($"Right Button Joystick {state}");
                        break;
                    case Button.Trigger:
                        Logger.Debug($"Right Button Trigger {state}");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("", ex);
            }
        }

        public void onLeftButton(Button button, bool newState)
        {
            try
            {
                string state = newState ? "Pressed" : "Released";
                switch (button)
                {
                    case Button.A:
                        Logger.Debug($"Left Button A {state}");
                        break;
                    case Button.B:
                        Logger.Debug($"Left Button B {state}");
                        break;
                    case Button.Grip:
                        Logger.Debug($"Left Button Grip {state}");
                        break;
                    case Button.Joystick:
                        Logger.Debug($"Left Button Joystick {state}");
                        break;
                    case Button.Trigger:
                        Logger.Debug($"Left Button Trigger {state}");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("", ex);
            }
        }

        public void onRightButtonTouched(Button button, bool newState)
        {
            try
            {
                string state = newState ? "Touched" : "Released";
                switch (button)
                {
                    case Button.A:
                        Logger.Debug($"Right Button A {state}");
                        break;
                    case Button.B:
                        Logger.Debug($"Right Button B {state}");
                        break;
                    case Button.Grip:
                        Logger.Debug($"Right Button Grip {state}");
                        break;
                    case Button.Joystick:
                        Logger.Debug($"Right Button Joystick {state}");
                        break;
                    case Button.Trigger:
                        Logger.Debug($"Right Button Trigger {state}");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("", ex);
            }
        }

        public void onLeftButtonTouched(Button button, bool newState)
        {
            try
            {
                string state = newState ? "Touched" : "Released";
                switch (button)
                {
                    case Button.A:
                        Logger.Debug($"Left Button A {state}");
                        break;
                    case Button.B:
                        Logger.Debug($"Left Button B {state}");
                        break;
                    case Button.Grip:
                        Logger.Debug($"Left Button Grip {state}");
                        break;
                    case Button.Joystick:
                        Logger.Debug($"Left Button Joystick {state}");
                        break;
                    case Button.Trigger:
                        Logger.Debug($"Left Button Trigger {state}");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("", ex);
            }
        }


        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            //Create GameplayOptions/SettingsUI if using either
            if (scene.name == "Menu")
                UI.BasicUI.CreateUI();

        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            OpenVRInput.RightController.ButtonPressChanged -= onRightButton;
            OpenVRInput.LeftController.ButtonPressChanged -= onLeftButton;
        }

        public void OnLevelWasLoaded(int level)
        {

        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {
            try
            {
                OpenVRInput.LeftController.UpdateState();
                OpenVRInput.RightController.UpdateState();
            }
            catch (Exception ex)
            {
                Logger.Exception("", ex);
            }

        }

        public void OnFixedUpdate()
        {
        }
    }
}
