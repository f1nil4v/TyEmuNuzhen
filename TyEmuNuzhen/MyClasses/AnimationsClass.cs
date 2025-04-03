using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class AnimationsClass
    {
        public static void ShakeElement(UIElement element)
        {
            TranslateTransform trans = new TranslateTransform();
            element.RenderTransform = trans;

            DoubleAnimationUsingKeyFrames anim = new DoubleAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromMilliseconds(200);

            anim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(-10, KeyTime.FromPercent(0.1)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(10, KeyTime.FromPercent(0.3)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(-10, KeyTime.FromPercent(0.5)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(10, KeyTime.FromPercent(0.7)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(-5, KeyTime.FromPercent(0.9)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1)));

            trans.BeginAnimation(TranslateTransform.XProperty, anim);
        }

    }
}
