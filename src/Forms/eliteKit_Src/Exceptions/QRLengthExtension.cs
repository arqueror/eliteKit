using System;
namespace eliteKit.Exceptions
{
    public class QRLengthExtension : Exception
    {
        public QRLengthExtension(string eccLevel, string encodingMode, int maxSizeByte) : base(
            $"The given payload exceeds the maximum size of the QR code standard. The maximum size allowed for the choosen paramters (ECC level={eccLevel}, EncodingMode={encodingMode}) is {maxSizeByte} byte."
        )
        { }
    }
}
