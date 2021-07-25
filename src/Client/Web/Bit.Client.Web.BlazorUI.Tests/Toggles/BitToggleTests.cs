﻿namespace Bit.Client.Web.BlazorUI.Tests.Toggles
{
    using System;
    using Bunit;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BitToggleTests : BunitTestContext
    {
        [DataTestMethod,
           DataRow(Visual.Fluent, true, true),
           DataRow(Visual.Fluent, true, false),
           DataRow(Visual.Fluent, false, true),
           DataRow(Visual.Fluent, false, false),

           DataRow(Visual.Cupertino, true, true),
           DataRow(Visual.Cupertino, true, false),
           DataRow(Visual.Cupertino, false, true),
           DataRow(Visual.Cupertino, false, false),

           DataRow(Visual.Material, true, true),
           DataRow(Visual.Material, true, false),
           DataRow(Visual.Material, false, true),
           DataRow(Visual.Material, false, false),
       ]
        public void BitToggleTest(Visual visual, bool isEnabled, bool isChecked)
        {
            var com = RenderComponent<BitToggleTest>(parameters =>
            {
                parameters.Add(p => p.Visual, visual);
                parameters.Add(p => p.IsEnabled, isEnabled);
                parameters.Add(p => p.IsChecked, isChecked);
            });

            var bitToggle = com.Find(".bit-tgl");

            var isEnabledClass = isEnabled ? "enabled" : "disabled";
            var visualClass = visual == Visual.Cupertino ? "cupertino" : visual == Visual.Material ? "material" : "fluent";
            var ischeckedClass = isChecked ? "checked" : "unchecked";

            Assert.IsTrue(bitToggle.ClassList.Contains($"bit-tgl-{isEnabledClass}-{ischeckedClass}-{visualClass}"));
        }

        [DataTestMethod,
            DataRow(Visual.Fluent, "", ""),
            DataRow(Visual.Fluent, "", null),
            DataRow(Visual.Fluent, null, ""),
            DataRow(Visual.Fluent, null, null),
            DataRow(Visual.Fluent, "On", "Off"),
            DataRow(Visual.Fluent, "On", ""),
            DataRow(Visual.Fluent, "On", null),
            DataRow(Visual.Fluent, "", "Off"),
            DataRow(Visual.Fluent, null, "Off"),

            DataRow(Visual.Cupertino, "", ""),
            DataRow(Visual.Cupertino, "", null),
            DataRow(Visual.Cupertino, null, ""),
            DataRow(Visual.Cupertino, null, null),
            DataRow(Visual.Cupertino, "On", "Off"),
            DataRow(Visual.Cupertino, "On", ""),
            DataRow(Visual.Cupertino, "On", null),
            DataRow(Visual.Cupertino, "", "Off"),
            DataRow(Visual.Cupertino, null, "Off"),

            DataRow(Visual.Material, "", ""),
            DataRow(Visual.Material, "", null),
            DataRow(Visual.Material, null, ""),
            DataRow(Visual.Material, null, null),
            DataRow(Visual.Material, "On", "Off"),
            DataRow(Visual.Material, "On", ""),
            DataRow(Visual.Material, "On", null),
            DataRow(Visual.Material, "", "Off"),
            DataRow(Visual.Material, null, "Off"),
        ]
        public void BitToggle_WithoutOnOffText_ShouldHaveClassName(Visual visual, string onText, string offText)
        {
            var com = RenderComponent<BitToggleTest>(parameters =>
            {
                parameters.Add(p => p.Visual, visual);
                parameters.Add(p => p.OnText, onText);
                parameters.Add(p => p.OffText, offText);
            });
            var bitToggle = com.Find(".bit-tgl");

            var visualClass = visual == Visual.Cupertino ? "cupertino" : visual == Visual.Material ? "material" : "fluent";

            if (onText.HasNoValue() || offText.HasNoValue())
            {
                Assert.IsTrue(bitToggle.ClassList.Contains($"bit-tgl-noonoff-{visualClass}"));
            }
        }

        [DataTestMethod,
          DataRow(Visual.Fluent, true),
          DataRow(Visual.Fluent, false),
          DataRow(Visual.Cupertino, true),
          DataRow(Visual.Cupertino, false),
          DataRow(Visual.Material, true),
          DataRow(Visual.Material, false),
        ]
        public void BitToggle_InlineLabrl_ShouldHaveClassName(Visual visual, bool isInlioneLabel)
        {
            var com = RenderComponent<BitToggleTest>(parameters =>
            {
                parameters.Add(p => p.Visual, visual);
                parameters.Add(p => p.IsInlineLabel, isInlioneLabel);
            });
            var bitToggle = com.Find(".bit-tgl");

            var visualClass = visual == Visual.Cupertino ? "cupertino" : visual == Visual.Material ? "material" : "fluent";

            if (isInlioneLabel)
            {
                Assert.IsTrue(bitToggle.ClassList.Contains($"bit-tgl-inline-{visualClass}"));
            }
        }

        [DataTestMethod, DataRow("Detailed AriaLabel")]
        public void BitToggleAriaLabelTest(string ariaLabel)
        {
            var com = RenderComponent<BitToggleTest>(parameters =>
            {
                parameters.Add(p => p.AriaLabel, ariaLabel);
            });

            var bitToggleButton = com.Find(".bit-tgl button");
            Assert.IsTrue(bitToggleButton.GetAttribute("aria-label").Equals(ariaLabel));
        }

        [DataTestMethod, 
            DataRow(true, "on", "off", "This is defaultText", "", "This is Label"),
            DataRow(false, "on", "off", "This is defaultText", "", "This is Label")
        ]
        public void BitToggleAriaLabelledyTest(bool isChecked, string onText, string offText, string defaultText, string ariaLabel, string label)
        {
            var com = RenderComponent<BitToggleTest>(parameters =>
            {
                parameters.Add(p => p.IsChecked, isChecked);
                parameters.Add(p => p.OnText, onText);
                parameters.Add(p => p.OffText, offText);
                parameters.Add(p => p.DefaultText, defaultText);
                parameters.Add(p => p.AriaLabel, ariaLabel);
                parameters.Add(p => p.Label, label);
            });

            var bitToggleButton = com.Find(".bit-tgl button");
            var bitToggleButtonId = bitToggleButton.Id;
            var labelId = bitToggleButtonId + "-label";
            var stateTextId = bitToggleButtonId + "-stateText";
            var ariaLabelledById = "";
            var stateText = (isChecked ? onText : offText) ?? defaultText ?? "";

            if (ariaLabel.HasNoValue())
            {
                if (label.HasValue())
                {
                    ariaLabelledById = labelId;
                }
                if (stateText.HasValue())
                {
                    ariaLabelledById = ariaLabelledById.HasValue() ? labelId + " " + stateTextId : stateTextId;
                }
            }

            Assert.IsTrue(bitToggleButton.GetAttribute("aria-labelledby").Equals(ariaLabelledById));
        }

        [DataTestMethod, 
            DataRow(true),
            DataRow(false)
        ]
        public void BitToggleAriaCheckedTest(bool isChecked)
        {
            var com = RenderComponent<BitToggleTest>(parameters =>
            {
                parameters.Add(p => p.IsChecked, isChecked);
            });

            var ariaChecked = isChecked ? "true" : "false";
            var bitToggleButton = com.Find(".bit-tgl button");
            Assert.IsTrue(bitToggleButton.GetAttribute("aria-checked").Equals(ariaChecked));
        }

        [DataTestMethod, DataRow("Switch")]
        public void BitToggleRoleTest(string role)
        {
            var com = RenderComponent<BitToggleTest>(parameters =>
            {
                parameters.Add(p => p.Role, role);
            });

            var bitToggleButton = com.Find(".bit-tgl button");
            Assert.IsTrue(bitToggleButton.GetAttribute("role").Equals(role));
        }

        [DataTestMethod, DataRow("This is label")]
        public void BitToggleLabel(string label)
        {
            var com = RenderComponent<BitToggleTest>(parameters =>
            {
                parameters.Add(p => p.Label, label);
            });

            var bitToggleLabel = com.Find(".bit-tgl > label");
            Assert.IsTrue(bitToggleLabel.TextContent.Equals(label));
        }

        [DataTestMethod, DataRow("<div>This is labelFragment</div>")]
        public void BitToggleMarkupLabelTest(string labelFragment)
        {
            var com = RenderComponent<BitToggleTest>(parameters =>
            {
                parameters.Add(p => p.LabelFragment, labelFragment);
            });

            var bitToggleLabelChild = com.Find(".bit-tgl > label").ChildNodes;
            bitToggleLabelChild.MarkupMatches(labelFragment);
        }
    }
}
