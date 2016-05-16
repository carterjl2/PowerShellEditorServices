using System.IO.Pipes;
using System.Threading.Tasks;

namespace Microsoft.PowerShell.EditorServices.Protocol.MessageProtocol.Channel
{
    public class NamedPipeServerChannel : ChannelBase
    {
        private string pipeName;
        private bool isClientConnected;
        private NamedPipeServerStream pipeServer;

        public NamedPipeServerChannel(string pipeName)
        {
            this.pipeName = pipeName;
        }

        protected override void Initialize(IMessageSerializer messageSerializer)
        {
            this.pipeServer =
                new NamedPipeServerStream(
                    pipeName,
                    PipeDirection.InOut,
                    1,
                    PipeTransmissionMode.Byte,
                    PipeOptions.Asynchronous);

            this.MessageReader =
                new MessageReader(
                    new NamedPipeStreamReader(this, this.pipeServer),
                    messageSerializer);

            this.MessageWriter =
                new MessageWriter(
                    new NamedPipeStreamWriter(this, this.pipeServer),
                    messageSerializer);
        }

        protected override void Shutdown()
        {
            if (this.pipeServer != null)
            {
                this.pipeServer.Close();
            }
        }

        #region Private Classes

        private class NamedPipeStreamReader : IStreamReader
        {
            private NamedPipeServerChannel serverChannel;
            private NamedPipeServerStream namedPipeStream;

            public NamedPipeStreamReader(
                NamedPipeServerChannel serverChannel,
                NamedPipeServerStream namedPipeStream)
            {
                this.serverChannel = serverChannel;
                this.namedPipeStream = namedPipeStream;
            }

            public async Task<int> ReadAsync(byte[] buffer, int offset, int count)
            {
                if (!this.serverChannel.isClientConnected)
                {
                    await this.namedPipeStream.WaitForConnectionAsync();
                    this.serverChannel.isClientConnected = true;
                }

                return await this.namedPipeStream.ReadAsync(buffer, offset, count);
            }
        }

        private class NamedPipeStreamWriter : IStreamWriter
        {
            private NamedPipeServerChannel serverChannel;
            private NamedPipeServerStream namedPipeStream;

            public NamedPipeStreamWriter(
                NamedPipeServerChannel serverChannel,
                NamedPipeServerStream namedPipeStream)
            {
                this.serverChannel = serverChannel;
                this.namedPipeStream = namedPipeStream;
            }

            public Task WriteAsync(byte[] buffer, int offset, int count)
            {
                // TODO: Connected?
                return this.namedPipeStream.WriteAsync(buffer, offset, count);
            }

            public Task FlushAsync()
            {
                // TODO: Connected?
                return this.namedPipeStream.FlushAsync();
            }
        }

        #endregion
    }
}
