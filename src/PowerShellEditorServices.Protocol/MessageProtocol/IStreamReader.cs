using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PowerShell.EditorServices.Protocol.MessageProtocol
{
    public interface IStreamReader
    {
        Task<int> ReadAsync(byte[] buffer, int offset, int count);
    }
}
