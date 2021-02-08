using System;
using System.Collections.Generic;
using System.Text;

using eliteKit.eliteEnums;
using System.Collections.ObjectModel;
using eliteKit.eliteElements;
using Xamarin.Forms;

namespace eliteKitDevelopment.appPages
{
    class pageTestChris : ContentPage
    {

        eliteColorCollection eliteColorCollection;
        eliteCircleImages eliteCircleImages;

        StackLayout stacklayoutContent;

        public pageTestChris()
        {
            this.stacklayoutContent = new StackLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            this.eliteColorCollection = new eliteColorCollection()
            {
                ColorShapes = new ObservableCollection<string>()
                {
                    "32a852",
                    "4db0a8",
                    "3250a8",
                    "dbdb37",
                    "e35b2d",
                    "e35b3d",
                    "e35b4d",
                    "e35b5d",
                    "e35b6d"
                }
            };

            this.eliteCircleImages = new eliteCircleImages()
            {
                CircleImages = new ObservableCollection<ImageSource>()
                {
                    ImageSource.FromResource("eliteKitDevelopment.demoImages.imageChris.jpg"),
                    ImageSource.FromResource("eliteKitDevelopment.demoImages.imageDominik.jpg"),
                    ImageSource.FromResource("eliteKitDevelopment.demoImages.imageDominik1.jpg"),
                    ImageSource.FromResource("eliteKitDevelopment.demoImages.imageHenrik.jpg"),
                    ImageSource.FromResource("eliteKitDevelopment.demoImages.imageMichi.jpg")
                },
            };

            this.Content = this.stacklayoutContent;

            this.stacklayoutContent.Children.Add(this.eliteColorCollection);
            this.stacklayoutContent.Children.Add(this.eliteCircleImages);
        }
    }
}
