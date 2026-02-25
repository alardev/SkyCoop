using Il2Cpp;
using SkyCoop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SkyCoopClient
{
    public class GameModeHUD
    {
        public static GameObject s_DarkwalkerHUDClone;
        static HUDNowhereToHide s_HUD;

        public static List<UISprite> s_SideIcons = new List<UISprite>();
        public static List<UILabel> s_SideLabels = new List<UILabel>();
        public static List<string> s_SideLabelsPrefix = new List<string>() { "", "", "", "" };
        public static string s_TimerPrefix = "Time Remaining";
        public static UILabel s_BottomLabel = null;

        public static void Reinitialize()
        {
            if (s_DarkwalkerHUDClone == null)
            {
                s_SideIcons.Clear();
                s_SideLabels.Clear();
                s_SideLabelsPrefix = new List<string>() { "", "", "", "" };
                Panel_HUD Panel = null;
                if (InterfaceManager.TryGetPanel<Panel_HUD>(out Panel))
                {
                    s_DarkwalkerHUDClone = UnityEngine.Object.Instantiate(Panel.m_NowhereToHide.gameObject, Panel.m_NowhereToHide.transform.parent);
                    s_DarkwalkerHUDClone.SetActive(true);
                    s_HUD = s_DarkwalkerHUDClone.GetComponent<HUDNowhereToHide>();
                    s_HUD.m_AfflictionRoot.SetActive(false);
                    s_HUD.m_EntityDistanceRoot.SetActive(false);
                    s_HUD.m_StartCountdownRoot.SetActive(false);
                    s_HUD.m_LureGlyphRoot.SetActive(false);
                    s_HUD.m_WardGlyphRoot.SetActive(false);
                    s_HUD.m_ToxicFogWarningRoot.SetActive(false);
                    s_HUD.transform.GetChild(4).gameObject.SetActive(false);
                    s_HUD.m_ToxicFogIndicatorLabel.gameObject.SetActive(false);
                    s_HUD.m_ToxicFogIndicatorLabel.text = "";
                    s_BottomLabel = s_HUD.m_ToxicFogIndicatorLabel;


                    s_SideIcons.Add(FixSideIcon(s_HUD.m_WardGlyphRoot.transform));
                    s_SideIcons.Add(FixSideIcon(s_HUD.m_LureGlyphRoot.transform));
                    s_SideIcons.Add(FixSideIcon(s_HUD.transform.GetChild(4)));

                    s_SideLabels.Add(GetSideLabel(s_HUD.m_WardGlyphRoot.transform));
                    s_SideLabels.Add(GetSideLabel(s_HUD.m_LureGlyphRoot.transform));
                    s_SideLabels.Add(GetSideLabel(s_HUD.transform.GetChild(4)));
                    s_SideLabels.Add(s_BottomLabel);

                }
            }
        }

        public static UISprite FixSideIcon(Transform Root)
        {
            UITexture Tex = Root.GetChild(0).GetComponent<UITexture>();
            Root.GetChild(0).GetComponent<TweenAlpha>().enabled = false;
            if (Tex)
            {
                Tex.enabled = false; // Remove Icon
            }
            else
            {
                Root.GetChild(0).GetComponent<UISprite>().enabled = false;
            }
            
            Root.GetChild(2).gameObject.SetActive(false); // Hide progress bar

            Transform BGIcon = Root.GetChild(0).GetChild(0);
            UISprite Sprite = BGIcon.GetComponent<UISprite>();
            BGIcon.transform.rotation = Quaternion.identity;
            BGIcon.GetComponent<TweenAlpha>().enabled = false;
            Sprite.alpha = 1f;
            Sprite.color = Color.white;
            return Sprite;
        }

        public static void SetSideIcon(int SideIconIndex, string Icon)
        {
            if(SideIconIndex < 0 || SideIconIndex > s_SideIcons.Count-1)
            {
                return;
            }
            s_SideIcons[SideIconIndex].transform.parent.parent.gameObject.SetActive(true);
            s_SideIcons[SideIconIndex].gameObject.SetActive(true);
            s_SideIcons[SideIconIndex].spriteName = Icon;
        }

        public static UILabel GetSideLabel(Transform Root)
        {
            UILabel Label = Root.GetChild(1).GetComponent<UILabel>();
            UILocalize Loca = Label.GetComponent<UILocalize>();
            if (Loca)
            {
                UnityEngine.Object.Destroy(Loca);
            }

            return Label;
        }

        public static void SetSideLabel(int SideLabelIndex, string Text)
        {
            if (SideLabelIndex < 0 || SideLabelIndex > s_SideLabels.Count - 1)
            {
                return;
            }
            s_SideLabels[SideLabelIndex].gameObject.SetActive(true);
            s_SideLabels[SideLabelIndex].text = s_SideLabelsPrefix[SideLabelIndex]+Text;
        }
        public static void SetSideLabelPrefix(int SideLabelIndex, string Text)
        {
            s_SideLabelsPrefix[SideLabelIndex] = Text;
        }

        public static void SetBottomLabel(string Text)
        {
            s_BottomLabel.gameObject.SetActive(true);
            s_BottomLabel.text = Text;
        }

        public static void SetTimerPrefix(string Text)
        {
            s_TimerPrefix = Text;
        }
        public static void UpdateGameModeTimer(float TimeLeft)
        {
            if (s_HUD)
            {
                int Minutes = (int)TimeLeft / 60;
                int Seconds = (int)TimeLeft % 60;
                s_HUD.m_StartCountdownRoot.SetActive(true);
                UILabel Label = s_HUD.m_StartCountdownRoot.transform.GetChild(0).GetComponent<UILabel>();
                UILocalize Loca = Label.GetComponent<UILocalize>();
                if (Loca)
                {
                    UnityEngine.Object.Destroy(Loca);
                }

                Label.text = s_TimerPrefix;

                s_HUD.m_StartCountdownLabel.text = string.Format("{0:0}:{1:00}", Minutes, Seconds);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Panel_HUD), "Enable")]
        private static class Panel_HUD_Enable
        {
            private static void Postfix(Panel_HUD __instance)
            {
                if (__instance.m_ExperimentalBuildLabel)
                {
                    __instance.m_ExperimentalBuildLabel.gameObject.SetActive(true);
                    __instance.m_ExperimentalBuildLabel.text = $"{BuildInfo.ModName} {BuildInfo.ModVersion}";
                }
            }
        }
    }
}
