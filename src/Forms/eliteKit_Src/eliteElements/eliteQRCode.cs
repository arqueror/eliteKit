using eliteKit.eliteCore;
using eliteKit.Infrastructure.eliteQRCode;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.IO;
using Xamarin.Forms;

namespace eliteKit.eliteElements
{
    public class eliteQRCode : SKCanvasView
    {
        private QRCodeData data;
        private byte[] snapshotArray = null;

        public eliteQRCode()
        {
            BackgroundColor = Color.Transparent;
        }


        #region Bindable Properties
        public static readonly BindableProperty HideBackgroundProperty = BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(eliteQRCode), false, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteQRCode)bindableObject).InvalidateSurface();

                }
            
        });
        public bool HideBackground
        {
            get
            {
                return (bool)GetValue(HideBackgroundProperty);
            }
            set
            {
                SetValue(HideBackgroundProperty, value);
                this.InvalidateSurface();
            }
        }

        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(eliteQRCode), true, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteQRCode)bindableObject).InvalidateSurface();

                }
            
        });
        public bool HasShadow
        {
            get
            {
                return (bool)GetValue(HasShadowProperty);
            }
            set
            {
                SetValue(HasShadowProperty, value);
                this.InvalidateSurface();
            }
        }

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteQRCode), true, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                    ((eliteQRCode)bindableObject).InvalidateSurface();
            
        });
        public bool IsGradient
        {
            get
            {
                return (bool)GetValue(IsGradientProperty);
            }
            set
            {
                SetValue(IsGradientProperty, value);
                this.InvalidateSurface();
            }
        }

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteQRCode), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                    ((eliteQRCode)bindableObject).InvalidateSurface();
            
        });
        public Color ColorPrimary
        {
            get
            {
                return (Color)GetValue(ColorPrimaryProperty);
            }
            set
            {
                SetValue(ColorPrimaryProperty, value);
                this.InvalidateSurface();
            }
        }

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteQRCode), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                    ((eliteQRCode)bindableObject).InvalidateSurface();
            
        });
        public Color ColorSecondary
        {
            get
            {
                return (Color)GetValue(ColorSecondaryProperty);
            }
            set
            {
                SetValue(ColorSecondaryProperty, value);
                this.InvalidateSurface();
            }
        }

        public static readonly BindableProperty LevelProperty = BindableProperty.Create(
            propertyName: nameof(ErrorCorrectionLevel),
            returnType: typeof(QRCodeGenerator.ECCLevel),
            declaringType: typeof(eliteQRCode),
            defaultValue: QRCodeGenerator.ECCLevel.H,
            propertyChanged: (bindable, old, newValue) => {
                if (bindable is eliteQRCode qrcode)
                {
                    qrcode.InvalidateSurface();
                }
            });

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(
            propertyName: nameof(Color),
            returnType: typeof(Color),
            declaringType: typeof(eliteQRCode),
            defaultValue: Color.Black,
            propertyChanged: (bindable, old, newValue) => {
                if (bindable is eliteQRCode qrcode)
                {
                    qrcode.InvalidateSurface();
                }
            });

        public static readonly BindableProperty QRContentProperty = BindableProperty.Create(
           propertyName: nameof(QRContent),
           returnType: typeof(string),
           declaringType: typeof(eliteQRCode),
           defaultValue: null,
           propertyChanged: (bindable, old, newValue) => {
               if (bindable is eliteQRCode qrcode)
               {
                   if (newValue is string content)
                   {
                       using (var generator = new QRCodeGenerator())
                       {
                           qrcode.data = generator.CreateQrCode(content, qrcode.ErrorCorrectionLevel);
                       }

                   }
                   else
                   {
                       qrcode.data = null;
                   }

                   qrcode.InvalidateSurface();
               }
           });

        public string QRContent
        {
            get { return (string)GetValue(QRContentProperty); }
            set { this.SetValue(QRContentProperty, value); }
        }

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { this.SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// Defines the tolerance level for how much of the code can be lost before the code cannot be recovered.
        /// </summary>
        public QRCodeGenerator.ECCLevel ErrorCorrectionLevel
        {
            get { return (QRCodeGenerator.ECCLevel)GetValue(LevelProperty); }
            set { this.SetValue(LevelProperty, value); }
        }
        #endregion

        /// <summary>
        /// Gets current QR Code bytes.
        /// </summary>
        /// <param name="compression"> [Optional] Compression strategy.</param>
        /// <returns></returns>
        public byte[] GetImageBytes()
        {

            return snapshotArray;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear();

            int canvasWidth = e.Info.Width;
            int canvasHeight = e.Info.Height;

            if (data != null)
            {

                float rectRadius = ((canvasHeight) / 10);

                var roundRectButton = new SKRoundRect(new SKRect(0, 0, canvasWidth, canvasHeight), rectRadius, rectRadius);
                var paintButton = new SKPaint()
                {
                    Color = ColorPrimary.ToSKColor(),
                    Style = SKPaintStyle.Fill,
                    IsAntialias = true
                };

                if (HasShadow)
                {
                    paintButton.ImageFilter = SKImageFilter.CreateDropShadow(
                        0,
                        8,
                        0,
                        5,
                        Color.FromRgba(0, 0, 0, 0.4).ToSKColor(),
                        SKDropShadowImageFilterShadowMode.DrawShadowAndForeground
                    );
                }

                if (IsGradient)
                    paintButton.Shader = SKShader.CreateLinearGradient(
                        new SKPoint(roundRectButton.Rect.Left, roundRectButton.Rect.Top),
                        new SKPoint(roundRectButton.Rect.Right, roundRectButton.Rect.Bottom),
                        new SKColor[] {
                        this.ColorPrimary.ToSKColor(),
                        this.ColorSecondary.ToSKColor()
                        },
                        new float[] {
                        0,
                        1
                        },
                        SKShaderTileMode.Repeat
                    );

                if (!HideBackground)
                    e.Surface.Canvas.DrawRoundRect(roundRectButton, paintButton);

                using (var renderer = new QRCodeRenderer())
                {
                    renderer.Paint.Color = Color.ToSKColor();
                    var area = SKRect.Create(0, 0, e.Info.Width, e.Info.Height);
                    renderer.Render(e.Surface.Canvas, area, data);
                    using (var image = e.Surface.Snapshot())
                    {
                        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                        using (var stream = new MemoryStream(new byte[data.Size]))
                        {
                            data.SaveTo(stream);
                            snapshotArray = stream.ToArray();
                        }
                    }
                }

            }
        }
    }
}
