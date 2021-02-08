using System;
using eliteKit.eliteElements.BaseElements;
using eliteKit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eliteKit.eliteConverters
{
    [ContentProperty("ResourceId")]
    public class ResourceToIconSourceConverter : IMarkupExtension
    {
        public object Type { get; set; }

        public string ResourceId { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            using (var stream = Type?.GetAssemblyResourceStream(ResourceId))
            {
                return new IconSource
                {
                    Stream = stream,
                    FileName = ResourceId
                };
            }
        }
    }
}
