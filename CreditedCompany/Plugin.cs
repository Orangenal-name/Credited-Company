using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CreditedCompany
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string GUID = "CreditedCompany";
        private const string NAME = "Credited Company";
        private const string VERSION = "1.0.1";

        private readonly Harmony harmony = new Harmony(GUID);

        public static Plugin instance;

        public static new ManualLogSource Logger;

        public static List<string> credits = new List<string>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            harmony.PatchAll(typeof(Plugin));

            Logger = BepInEx.Logging.Logger.CreateLogSource(GUID);
            Logger.LogInfo($"Plugin {GUID} is loaded!");
        }

        [HarmonyPatch(typeof(MenuManager), "Start")]
        [HarmonyPostfix]
        private static void PutMeInTheCreditsImImportantISwear(ref MenuManager __instance)
        {
            var creditsTextContainer = __instance.transform.parent.Find("MenuContainer/CreditsPanel/Panel/Scroll View/Viewport/Content/CreditsText");
            if (creditsTextContainer == null) return;
            var scrollBar = __instance.transform.parent.Find("MenuContainer/CreditsPanel/Panel/Scroll View").GetComponent<ScrollRect>();
            var rect = scrollBar.content.GetComponent<RectTransform>();
            var sizeToAdd = 50 + (100 / 6) * credits.Count;
            var separator = "----------------------------------";
            var text = "";
            text += $"\n\n{separator}\nMod developers\n\nOrangenal - Credited Company\n";
            foreach (var credit in credits)
            {
                text += credit + "\n";
                if (credit.Contains("\n"))
                {
                    string[] substrings = credit.Split('\n');

                    int newlineCount = substrings.Length - 1;

                    sizeToAdd += (100 / 6)*newlineCount;
                } 
            }

            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + sizeToAdd);
            creditsTextContainer.GetComponent<TextMeshProUGUI>().text += text;
        }
    }
}