using Microsoft.PowerShell.EditorServices.Protocol.LanguageServer;
using Microsoft.PowerShell.EditorServices.Protocol.MessageProtocol;
using Microsoft.PowerShell.EditorServices.Protocol.MessageProtocol.Channel;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.PowerShell.EditorServices.Test.Protocol.MessageProtocol
{
    public class NamedPipeChannelTests
    {
        [Fact]
        public async Task NamedPipeClientConnectsToServer()
        {
            const string pipeName = "PSES-Test-Pipe";

            //NamedPipeServerChannel serverChannel = new NamedPipeServerChannel(pipeName);
            //serverChannel.Start(MessageProtocolType.LanguageServer);

            NamedPipeClientChannel clientChannel = new NamedPipeClientChannel(pipeName);
            clientChannel.Start(MessageProtocolType.LanguageServer);

            await clientChannel.MessageWriter.WriteEvent(
                DidOpenTextDocumentNotification.Type,
                new DidOpenTextDocumentNotification { Uri = "test", Text = "test" });

            clientChannel.Stop();
            //serverChannel.Stop();
        }
    }
}
