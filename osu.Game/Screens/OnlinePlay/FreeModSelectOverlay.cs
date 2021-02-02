// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Mods;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Screens.OnlinePlay
{
    /// <summary>
    /// A <see cref="ModSelectOverlay"/> used for free-mod selection in online play.
    /// </summary>
    public class FreeModSelectOverlay : ModSelectOverlay
    {
        protected override bool Stacked => false;

        public FreeModSelectOverlay()
        {
            CustomiseButton.Alpha = 0;
            MultiplierSection.Alpha = 0;
            DeselectAllButton.Alpha = 0;

            Drawable selectAllButton;
            Drawable deselectAllButton;

            FooterContainer.AddRange(new[]
            {
                selectAllButton = new TriangleButton
                {
                    Origin = Anchor.CentreLeft,
                    Anchor = Anchor.CentreLeft,
                    Width = 180,
                    Text = "Select All",
                    Action = selectAll,
                },
                // Unlike the base mod select overlay, this button deselects mods instantaneously.
                deselectAllButton = new TriangleButton
                {
                    Origin = Anchor.CentreLeft,
                    Anchor = Anchor.CentreLeft,
                    Width = 180,
                    Text = "Deselect All",
                    Action = deselectAll,
                },
            });

            FooterContainer.SetLayoutPosition(selectAllButton, -2);
            FooterContainer.SetLayoutPosition(deselectAllButton, -1);
        }

        private void selectAll()
        {
            foreach (var section in ModSectionsContainer.Children)
                section.SelectAll();
        }

        private void deselectAll()
        {
            foreach (var section in ModSectionsContainer.Children)
                section.DeselectAll(true);
        }

        protected override ModSection CreateModSection(ModType type) => new FreeModSection(type);

        private class FreeModSection : ModSection
        {
            private HeaderCheckbox checkbox;

            public FreeModSection(ModType type)
                : base(type)
            {
            }

            protected override Drawable CreateHeader(string text) => new Container
            {
                AutoSizeAxes = Axes.Y,
                Width = 175,
                Child = checkbox = new HeaderCheckbox
                {
                    LabelText = text,
                    Changed = onCheckboxChanged
                }
            };

            private void onCheckboxChanged(bool value)
            {
                foreach (var button in ButtonsContainer.OfType<ModButton>())
                {
                    if (value)
                        button.SelectAt(0);
                    else
                        button.Deselect();
                }
            }

            protected override void Update()
            {
                base.Update();

                var validButtons = ButtonsContainer.OfType<ModButton>().Where(b => b.Mod.HasImplementation);
                checkbox.Current.Value = validButtons.All(b => b.Selected);
            }
        }

        private class HeaderCheckbox : OsuCheckbox
        {
            public Action<bool> Changed;

            protected override void OnUserChange(bool value)
            {
                base.OnUserChange(value);
                Changed?.Invoke(value);
            }
        }
    }
}
