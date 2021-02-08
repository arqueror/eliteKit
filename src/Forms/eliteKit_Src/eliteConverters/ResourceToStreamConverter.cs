using System;
using eliteKit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eliteKit.eliteConverters
{
    [ContentProperty("ResourceId")]
    public class ResourceToStreamConverter : IMarkupExtension
    {
        public object Type { get; set; }

        public string ResourceId { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Type?.GetAssemblyResourceStream(ResourceId);
        }
    }
}
