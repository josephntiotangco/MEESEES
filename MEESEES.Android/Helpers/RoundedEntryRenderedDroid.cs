using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MEESEES.Droid.Helpers;
using MEESEES.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderedDroid))]
namespace MEESEES.Droid.Helpers
{
    public class RoundedEntryRenderedDroid : EntryRenderer
    {
        public RoundedEntryRenderedDroid(Context context) : base(context) { }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if(e.OldElement == null)
            {
                //use uncomment if will be using android layout
                //Control.SetBackgroundResource(Resource.Layout.rounded_shape);
                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetCornerRadius(30f);
                gradientDrawable.SetStroke(5, Android.Graphics.Color.ParseColor("#F1F8E9"));
                gradientDrawable.SetColor(Android.Graphics.Color.ParseColor("#e9f8f0"));
                Control.SetBackground(gradientDrawable);
                Control.SetPadding(10, Control.PaddingTop, Control.PaddingRight, Control.PaddingBottom);
            }
        }
    }
}