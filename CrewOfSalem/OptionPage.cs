using System.Collections.Generic;
using System.Reflection;
using Essentials.Options;

namespace CrewOfSalem
{
    public class OptionPage
    {
        // Fields
        private static readonly List<OptionPage> OptionPages = new List<OptionPage>();
        private static          int              pageIndex   = 0;

        private readonly List<CustomOption> options = new List<CustomOption>();

        private static readonly FieldInfo OptionsFieldInfo =
            typeof(CustomOption).GetField("Options", BindingFlags.Static | BindingFlags.NonPublic);

        // Properties
        private bool Enabled
        {
            set
            {
                foreach (CustomOption option in options)
                {
                    option.HudVisible = value;
                    option.MenuVisible = value;
                }
            }
            /*
            set
            {
                var currentOptions = (List<CustomOption>) OptionsFieldInfo.GetValue(null);
                if (value)
                {
                    currentOptions.AddRange(options);
                } else
                {
                    currentOptions.RemoveAll(option => options.Contains(option));
                }

                OptionsFieldInfo.SetValue(null, currentOptions);

                foreach (CustomOption option in currentOptions)
                {
                    option.HudVisible = value;
                    option.MenuVisible = value;
                }
            }
            */
        }

        // Constructors
        private OptionPage() { }

        // Methods
        public static void CreateOptionPage(IEnumerable<CustomOption> options)
        {
            var optionPage = new OptionPage();
            OptionPages.Add(optionPage);
            optionPage.options.AddRange(options);
            optionPage.Enabled = OptionPages.Count == 1;
        }

        public static void TurnPage()
        {
            OptionPages[pageIndex].Enabled = false;
            pageIndex = ++pageIndex % OptionPages.Count;
            OptionPages[pageIndex].Enabled = true;

            // Object.FindObjectOfType<GameOptionsMenu>()?.Start();
        }

        /*
        public void AddOption(CustomOption option)
        {
            if (!options.Contains(option))
            {
                options.Add(option);
            }
        }

        public void RemoveOption(CustomOption option)
        {
            if (options.Contains(option))
            {
                options.Remove(option);
            }
        }
        */
    }
}