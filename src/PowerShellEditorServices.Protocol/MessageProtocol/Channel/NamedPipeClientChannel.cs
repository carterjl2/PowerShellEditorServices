using System;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace Microsoft.PowerShell.EditorServices.Protocol.MessageProtocol.Channel
{
    public class NamedPipeClientChannel : ChannelBase
    {
        private string pipeName;
        private bool isClientConnected;
        private NamedPipeClientStream pipeClient;

        public NamedPipeClientChannel(string pipeName)
        {
            this.pipeName = pipeName;
        }

        protected override void Initialize(IMessageSerializer messageSerializer)
        {
            this.pipeClient =
                new NamedPipeClientStream(
                    ".",
                    this.pipeName,
                    PipeDirection.InOut,
                    PipeOptions.Asynchronous);

            this.MessageReader =
                new MessageReader(
                    new NamedPipeStreamReader(this, this.pipeClient),
                    messageSerializer);

            this.MessageWriter =
                new MessageWriter(
                    new NamedPipeStreamWriter(this, this.pipeClient),
                    messageSerializer);
        }

        protected override void Shutdown()
        {
            if (this.pipeClient != null)
            {
                this.pipeClient.Close();
            }
        }

        #region Private Classes

        private class NamedPipeStreamReader : IStreamReader
        {
            private NamedPipeClientChannel clientChannel;
            private NamedPipeClientStream namedPipeStream;

            public NamedPipeStreamReader(
                NamedPipeClientChannel clientChannel,
                NamedPipeClientStream namedPipeStream)
            {
                this.clientChannel = clientChannel;
                this.namedPipeStream = namedPipeStream;
            }

            public async Task<int> ReadAsync(byte[] buffer, int offset, int count)
            {
                if (!this.clientChannel.isClientConnected)
                {
                    await this.namedPipeStream.ConnectAsync();
                    this.clientChannel.isClientConnected = true;
                }

                return await this.namedPipeStream.ReadAsync(buffer, offset, count);
            }
        }

        private class NamedPipeStreamWriter : IStreamWriter
        {
            private NamedPipeClientChannel clientChannel;
            private NamedPipeClientStream namedPipeStream;

            public NamedPipeStreamWriter(
                NamedPipeClientChannel clientChannel,
                NamedPipeClientStream namedPipeStream)
            {
                this.clientChannel = clientChannel;
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
