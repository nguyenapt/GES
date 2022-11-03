using System.IO;

namespace GES.Common.Services.Interface
{
    public interface IConverterProcessor
    {
        void Convert(Stream source, Stream destination);
    }
}
