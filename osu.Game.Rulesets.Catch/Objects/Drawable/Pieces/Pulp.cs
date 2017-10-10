﻿using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using OpenTK;
using OpenTK.Graphics;

namespace osu.Game.Rulesets.Catch.Objects.Drawable.Pieces
{
    public class Pulp : Circle, IHasAccentColour
    {
        public const float PULP_SIZE = 20;

        public Pulp()
        {
            Size = new Vector2(PULP_SIZE);

            Blending = BlendingMode.Additive;
            Colour = Color4.White.Opacity(0.9f);
        }

        private Color4 accentColour;
        public Color4 AccentColour
        {
            get { return accentColour; }
            set
            {
                accentColour = value;

                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Glow,
                    Radius = 5,
                    Colour = accentColour.Lighten(100),
                };
            }
        }
    }
}
