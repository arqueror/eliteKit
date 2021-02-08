using System;
using Xamarin.Forms;

namespace eliteKit.eliteEventArgs
{
    public class EventArgsColorChanged : EventArgs
    {

        public EventArgsColorChanged(Color color)
        {
            this.Color = color;
        }

        public Color Color { get; private set; }
    }
}
