using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionPlugin;
using System.Runtime.InteropServices;


namespace BSInputLib
{
    public class Plugin : IPlugin
    {
        public static string PluginName = "BSInputLib";
        public string Name => PluginName;
        public string Version => "0.2.0";

        public void OnApplicationStart()
        {
            Util.Logger.LogLevel = Util.LogLevel.Debug;

            //SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            //SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            OpenVRInput.RightController.UpdateState();
            OpenVRInput.LeftController.UpdateState();
            // For testing purposes, use LogLevel.Debug
            OpenVRInput.RightController.ButtonPressChanged += OnRightButton;
            OpenVRInput.LeftController.ButtonPressChanged += OnLeftButton;

            OpenVRInput.RightController.ButtonTouchChanged += OnRightButtonTouched;
            OpenVRInput.LeftController.ButtonTouchChanged += OnLeftButtonTouched;


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

        public void OnRightButton(Button button, bool newState)
        {
            Console.WriteLine($"{button.ToString()} is now {newState}");
            try
            {
                string state = newState ? "Pressed" : "Released";
                switch (button)
                {
                    case Button.A:
                        Util.Logger.Info($"Right Button A {state}");
                        break;
                    case Button.AppMenu:
                        Util.Logger.Info($"Right Button AppMenu {state}");
                        break;
                    case Button.Grip:
                        Util.Logger.Info($"Right Button Grip {state}");
                        break;
                    case Button.Touchpad:
                        Util.Logger.Info($"Right Button Joystick {state}");
                        break;
                    case Button.Trigger:
                        Util.Logger.Info($"Right Button Trigger {state}");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Util.Logger.Debug("", ex);
            }
        }

        public void OnLeftButton(Button button, bool newState)
        {
            try
            {
                string state = newState ? "Pressed" : "Released";
                switch (button)
                {
                    case Button.A:
                        Util.Logger.Info($"Left Button A {state}");
                        break;
                    case Button.AppMenu:
                        Util.Logger.Info($"Left Button AppMenu {state}");
                        break;
                    case Button.Grip:
                        Util.Logger.Info($"Left Button Grip {state}");
                        break;
                    case Button.Touchpad:
                        Util.Logger.Info($"Left Button Joystick {state}");
                        break;
                    case Button.Trigger:
                        Util.Logger.Info($"Left Button Trigger {state}");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Util.Logger.Debug("", ex);
            }
        }

        public void OnRightButtonTouched(Button button, bool newState)
        {
            try
            {
                string state = newState ? "Touched" : "Released";
                switch (button)
                {
                    case Button.A:
                        Util.Logger.Info($"Right Button A {state}");
                        break;
                    case Button.AppMenu:
                        Util.Logger.Info($"Right Button AppMenu {state}");
                        break;
                    case Button.Grip:
                        Util.Logger.Info($"Right Button Grip {state}");
                        break;
                    case Button.Touchpad:
                        Util.Logger.Info($"Right Button Joystick {state}");
                        break;
                    case Button.Trigger:
                        Util.Logger.Info($"Right Button Trigger {state}");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Util.Logger.Debug("", ex);
            }
        }

        public void OnLeftButtonTouched(Button button, bool newState)
        {
            try
            {
                string state = newState ? "Touched" : "Released";
                switch (button)
                {
                    case Button.A:
                        Util.Logger.Info($"Left Button A {state}");
                        break;
                    case Button.AppMenu:
                        Util.Logger.Info($"Left Button AppMenu {state}");
                        break;
                    case Button.Grip:
                        Util.Logger.Info($"Left Button Grip {state}");
                        break;
                    case Button.Touchpad:
                        Util.Logger.Info($"Left Button Joystick {state}");
                        break;
                    case Button.Trigger:
                        Util.Logger.Info($"Left Button Trigger {state}");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Util.Logger.Debug("", ex);
            }
        }


        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            //Create GameplayOptions/SettingsUI if using either
            //if (scene.name == "Menu")
            //UI.BasicUI.CreateUI();
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            OpenVRInput.RightController.ButtonPressChanged -= OnRightButton;
            OpenVRInput.LeftController.ButtonPressChanged -= OnLeftButton;
            OpenVRInput.RightController.ButtonTouchChanged -= OnRightButtonTouched;
            OpenVRInput.LeftController.ButtonTouchChanged -= OnLeftButtonTouched;
        }

        public void OnLevelWasLoaded(int level)
        {

        }

        public void OnLevelWasInitialized(int level)
        {
        }
        
        public void OnUpdate()
        {
            //OpenVRInput.LeftController.UpdateState();
            //OpenVRInput.RightController.UpdateState();
            OpenVRInput.CheckEvents();
            
        }

        public void OnFixedUpdate()
        {
        }
    }
}
