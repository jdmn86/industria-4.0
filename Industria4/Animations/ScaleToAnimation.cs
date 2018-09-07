﻿using System;
using System.Threading.Tasks;
using Industria4.Animations.Base;
using Industria4.Helpers;
using Xamarin.Forms;

namespace Industria4.Animations
{
    public class ScaleToAnimation : AnimationBase
    {
        public static readonly BindableProperty ScaleProperty =
            BindableProperty.Create("Scale", typeof(double), typeof(RotateToAnimation), 0.0d,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ScaleToAnimation)bindable).Scale = (double)newValue);

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.ScaleTo(Scale, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }

        protected override Task ResetAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.ScaleTo(Scale, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }

    public class RelScaleToAnimation : AnimationBase
    {
        public static readonly BindableProperty ScaleProperty =
            BindableProperty.Create("Scale", typeof(double), typeof(RotateToAnimation), 0.0d,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((RelScaleToAnimation)bindable).Scale = (double)newValue);

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.RelScaleTo(Scale, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }

        protected override Task ResetAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.RelScaleTo(Scale, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }
}