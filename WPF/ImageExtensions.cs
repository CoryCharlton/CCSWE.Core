using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CCSWE
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Provides an animation while changing the source of an <see cref="Image"/> control.
        /// </summary>
        /// <param name="image">The target <see cref="Image"/> control.</param>
        /// <param name="source">The new <see cref="ImageSource"/> value.</param>
        /// <param name="fadeDuration">The duration in milliseconds to run the animations for.</param>
        public static void AnimateSourceChange(this Image image, ImageSource source, double fadeDuration)
        {
            AnimateSourceChange(image, source, TimeSpan.FromMilliseconds(fadeDuration));
        }

        /// <summary>
        /// Provides an animation while changing the source of an <see cref="Image"/> control.
        /// </summary>
        /// <param name="image">The target <see cref="Image"/> control.</param>
        /// <param name="source">The new <see cref="ImageSource"/> value.</param>
        /// <param name="fadeDuration">The duration to run the animations for.</param>
        public static void AnimateSourceChange(this Image image, ImageSource source, TimeSpan fadeDuration)
        {
            AnimateSourceChange(image, source, fadeDuration, fadeDuration);
        }

        /// <summary>
        /// Provides an animation while changing the source of an <see cref="Image"/> control.
        /// </summary>
        /// <param name="image">The target <see cref="Image"/> control.</param>
        /// <param name="source">The new <see cref="ImageSource"/> value.</param>
        /// <param name="fadeOutDuration">The duration in milliseconds to run the fade out animation for.</param>
        /// <param name="fadeInDuration">The duration in milliseconds to run the fade out animation for.</param>
        public static void AnimateSourceChange(this Image image, ImageSource source, double fadeOutDuration, double fadeInDuration)
        {
            AnimateSourceChange(image, source, TimeSpan.FromMilliseconds(fadeOutDuration), TimeSpan.FromMilliseconds(fadeInDuration));
        }

        /// <summary>
        /// Provides an animation while changing the source of an <see cref="Image"/> control.
        /// </summary>
        /// <param name="image">The target <see cref="Image"/> control.</param>
        /// <param name="source">The new <see cref="ImageSource"/> value.</param>
        /// <param name="fadeOutDuration">The duration to run the fade out animation for.</param>
        /// <param name="fadeInDuration">The duration to run the fade out animation for.</param>
        public static void AnimateSourceChange(this Image image, ImageSource source, TimeSpan fadeOutDuration, TimeSpan fadeInDuration)
        {
            var fadeInAnimation = new DoubleAnimation(1d, fadeInDuration);
            
            if (image.Source != null && fadeOutDuration > TimeSpan.Zero)
            {
                var fadeOutAnimation = new DoubleAnimation(0d, fadeOutDuration);

                fadeOutAnimation.Completed += (o, e) =>
                {
                    image.Source = source;
                    image.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                };

                image.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
            }
            else
            {
                image.Opacity = 0d;
                image.Source = source;
                image.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            }
        }
    }
}
