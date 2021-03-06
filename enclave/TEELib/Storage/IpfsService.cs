﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ipfs.Http;

namespace TEELib.Storage
{
    public class IpfsService : IIpfsService
    {
        private IpfsClient GetClient()
        {
            return new IpfsClient("https://ipfs.infura.io:5001");
        }

        public async Task<string> UploadFileAsync(string path,
            IDictionary<string, string> metadata = null)
        {
            var ipfs = GetClient();

            var node = await ipfs.FileSystem.AddFileAsync(path);
            return node.Id;
        }

        public async Task<string> UploadStreamAsync(Stream stream, IDictionary<string, string> metadata = null)
        {
            var ipfs = GetClient();

            var node = await ipfs.FileSystem.AddAsync(stream);
            return node.Id;
        }

        public async Task DownloadFile(string hash, string targetLocation)
        {
            var ipfs = GetClient();

            using (var downloadStream = await ipfs.FileSystem.ReadFileAsync(hash))
            {
                using (var fileStream = new FileStream(targetLocation, FileMode.Create))
                {
                    downloadStream.CopyTo(fileStream);
                }
            }
        }

        public async Task<Stream> DownloadContentAsync(string ipfsHash)
        {
            var ipfs = GetClient();
            var outputStream = new MemoryStream();

            using (var downloadStream = await ipfs.FileSystem.ReadFileAsync(ipfsHash))
            {                
                downloadStream.CopyTo(outputStream);                
            }

            return outputStream;
        }        
    }
}
