﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.ObjectExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;

namespace osu.Game.Screens.Play.HUD
{
    /// <summary>
    /// A container for components displaying the current player health.
    /// Gets bound automatically to the <see cref="Rulesets.Scoring.HealthProcessor"/> when inserted to <see cref="DrawableRuleset.Overlays"/> hierarchy.
    /// </summary>
    public abstract partial class HealthDisplay : CompositeDrawable
    {
        private readonly Bindable<bool> showHealthBar = new Bindable<bool>(true);

        [Resolved]
        protected HealthProcessor HealthProcessor { get; private set; } = null!;

        public Bindable<double> Current { get; } = new BindableDouble(1)
        {
            MinValue = 0,
            MaxValue = 1
        };

        /// <summary>
        /// Triggered when a <see cref="Judgement"/> is a successful hit, signaling the health display to perform a flash animation (if designed to do so).
        /// </summary>
        /// <param name="result">The judgement result.</param>
        protected virtual void Flash(JudgementResult result)
        {
        }

        /// <summary>
        /// Triggered when a <see cref="Judgement"/> resulted in the player losing health.
        /// </summary>
        /// <param name="result">The judgement result.</param>
        protected virtual void Miss(JudgementResult result)
        {
        }

        [Resolved]
        private HUDOverlay? hudOverlay { get; set; }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Current.BindTo(HealthProcessor.Health);
            HealthProcessor.NewJudgement += onNewJudgement;

            if (hudOverlay != null)
                showHealthBar.BindTo(hudOverlay.ShowHealthBar);

            // this probably shouldn't be operating on `this.`
            showHealthBar.BindValueChanged(healthBar => this.FadeTo(healthBar.NewValue ? 1 : 0, HUDOverlay.FADE_DURATION, HUDOverlay.FADE_EASING), true);
        }

        private void onNewJudgement(JudgementResult judgement)
        {
            if (judgement.IsHit && judgement.Type != HitResult.IgnoreHit)
                Flash(judgement);
            else if (judgement.Judgement.HealthIncreaseFor(judgement) < 0)
                Miss(judgement);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (HealthProcessor.IsNotNull())
                HealthProcessor.NewJudgement -= onNewJudgement;
        }
    }
}
