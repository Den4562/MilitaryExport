using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;
using WpfAppMilitaryExport.Navigator;

namespace WpfAppMilitaryExport.Icons
{
    public partial class MainWindow : UserControl
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            InitializeImageAnimation("image1", "textBlock", TextBlock.HeightProperty, TimeSpan.FromSeconds(0.3));
            InitializeImageAnimation("image2", "textBlock2", TextBlock.HeightProperty, TimeSpan.FromSeconds(0.3));

            ImageBehavior.SetAnimatedSource(gifImage, new BitmapImage(new Uri("pack://application:,,,/Assets/Image/flag.gif")));
            ImageBehavior.SetRepeatBehavior(gifImage, new RepeatBehavior(0));
            image2.MouseLeftButtonDown += (sender, e) =>
            {
                // Переключитесь на страницу Autorized
                var authorizedPage = new Autorizeds(); // Предполагается, что Autorized - это UserControl для новой страницы
                NavigatorObject.Switch(authorizedPage);
            };
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Создаем анимацию для линии
            Storyboard lineAnimation = (Storyboard)FindResource("LineAnimation");
            lineAnimation.Completed += lineAnim;
            lineAnimation.Begin();
        }

      

        private void lineAnim(object sender, EventArgs e)
        {
            // Анимация линии завершилась, можно запустить анимацию появления изображений
            Storyboard imageAnimation = (Storyboard)FindResource("ImageAnimation");
            if (imageAnimation != null)
            {
                imageAnimation.Begin();
            }
        }




        private void InitializeImageAnimation(string imageName, string textBlockName, DependencyProperty propertyToAnimate, TimeSpan duration)
        {
            Image image = FindName(imageName) as Image;
            TextBlock textBlock = FindName(textBlockName) as TextBlock;

            DoubleAnimation showAnimation = new DoubleAnimation
            {
                To = 30,
                Duration = duration
            };

            DoubleAnimation hideAnimation = new DoubleAnimation
            {
                To = 0,
                Duration = duration
            };

            image.MouseEnter += (sender, e) =>
            {
                textBlock.BeginAnimation(propertyToAnimate, showAnimation);
            };

            image.MouseLeave += (sender, e) =>
            {
                textBlock.BeginAnimation(propertyToAnimate, hideAnimation);
            };
        }
    }
}