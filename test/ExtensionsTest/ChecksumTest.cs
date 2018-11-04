using System;
using Extensions.Encryption;
using System.Net;
using Xunit;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExtensionsTest
{
    public class ChecksumTest
    {
        [Fact]
        public async Task Should_Match()
        {
            var url = "https://www.anne-sophie-pic.com/sites/default/files/styles/block_square/public/media/2017/d%C3%A9c/k17a5299.jpg?itok=w8vQDqGi";
            var correctChecksum = "822bc42daad156f1e8ae15f41f67c672";
            
            var response = await new HttpClient().GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();
            var checksum = await stream.GetMd5Async();

            Assert.True(correctChecksum == checksum);
        }

        [Fact]
        public async Task Should_Not_Close_Stream()
        {
            var url = "https://www.anne-sophie-pic.com/sites/default/files/styles/block_square/public/media/2017/d%C3%A9c/k17a5299.jpg?itok=w8vQDqGi";
            var correctChecksum = "822bc42daad156f1e8ae15f41f67c672";

            var response = await new HttpClient().GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();
            var checksum = await stream.GetMd5Async();

            Assert.True(correctChecksum == checksum);
            Assert.True(stream.CanRead);
        }

        [Fact]
        public async Task Should_ThrowException_Null_Stream()
        {
            Stream stream = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => stream.GetMd5Async());
        }

        [Fact]
        public async Task Should_ThrowException_Closed_Stream()
        {
            var url = "https://www.anne-sophie-pic.com/sites/default/files/styles/block_square/public/media/2017/d%C3%A9c/k17a5299.jpg?itok=w8vQDqGi";
            var response = await new HttpClient().GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();
            stream.Close();
            await Assert.ThrowsAsync<InvalidOperationException>(() => stream.GetMd5Async());
        }

    }
}
