using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Il2CppRewired.ComponentControls.Data;
using ModSettings;
using MelonLoader;
using UnityEngine;
using System.Reflection;
using Il2Cpp;

namespace SkyCoopClient
{
    public class Settings : JsonModSettings
    {
        internal static Settings m_Options = new Settings();

        [Section("Generic Settings")]

        [Name("Use Steam UserName")]
        [Description("Enable Steam username visibility for other players. Disable to display a generic name.")]
        public bool m_SteamUserName = true;

        [Section("Voice Chat")]

        [Name("Push To Talk")]
        [Description("Enable to hold down a defined key for voice activation.")]
        public bool m_PushToTalk = false;

        [Name("Push To Talk Button")]
        [Description("Configure the key for Push To Talk activation.")]
        public KeyCode m_VoiceButton = KeyCode.V;

        [Name("Received Volume")]
        [Description("Volume of incoming voice.")]
        [Slider(0, 5)]
        public float m_ReceivedVoiceVolume = 1;

        [Name("Microphone Volume")]
        [Description("Volume of your outgoing voice.")]
        [Slider(0, 5)]
        public float m_MicrophoneVoice = 1;

        [Name("Use Noise Suppression")]
        [Description("Enable to reduce microphone noise..")]
        public bool m_NoiseSuppression = true;

        [Name("Speaking Indicator")]
        [Description("Show icon during speech.")]
        public bool m_DisplayMicrophoneIcon = true;

        public static void Init()
        {
            m_Options = new Settings();
            m_Options.RefreshGUI();
            m_Options.AddToModSettings("Sky Co-op: Reborn");
        }

        protected override void OnChange(FieldInfo field, object? oldValue, object? newValue)
        {
            base.OnChange(field, oldValue, newValue);
            ClientVoice.OnNoiseSuppressionChanged();
        }

        public static void BackFromForcedMenu()
        {
            Panel_OptionsMenu Options = InterfaceManager.GetPanel<Panel_OptionsMenu>();

            if (Options)
            {
                Transform Pages = Options.transform.FindChild("Pages");
                if (Pages)
                {
                    Transform ModSettings = Pages.FindChild("ModSettings");

                    ConsoleComboBox box = ModSettings.GetChild(1).GetChild(0).GetComponent<ConsoleComboBox>();
                    Transform SubMenuDisplay = ModSettings.GetChild(0);
                    if (SubMenuDisplay)
                    {
                        SubMenuDisplay.gameObject.SetActive(true);
                    }

                    if (box)
                    {
                        box.gameObject.SetActive(true);
                    }
                }
            }
        }

        public static void ForceToShow()
        {
            Panel_OptionsMenu Options = InterfaceManager.GetPanel<Panel_OptionsMenu>();

            if (Options)
            {
                Options.m_MainTab.SetActive(false);

                Transform Pages = Options.transform.FindChild("Pages");
                if (Pages)
                {
                    Transform ModSettings = Pages.FindChild("ModSettings");

                    ModSettings.gameObject.SetActive(true);

                    ConsoleComboBox box = ModSettings.GetChild(1).GetChild(0).GetComponent<ConsoleComboBox>();
                    Transform SubMenuDisplay = ModSettings.GetChild(0);
                    if (SubMenuDisplay)
                    {
                        SubMenuDisplay.gameObject.SetActive(false);
                    }

                    if (box)
                    {
                        box.m_CurrentIndex = box.items.IndexOf("Sky Co-op: Reborn");
                        box.m_SelectedItem = box.items[box.m_CurrentIndex];
                        box.Refresh();
                        if (EventDelegate.IsValid(box.onChange))
                        {
                            EventDelegate.Execute(box.onChange);
                        }
                        box.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
