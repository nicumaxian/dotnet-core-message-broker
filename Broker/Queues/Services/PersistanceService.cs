using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Broker.Queues.Entities;
using Microsoft.Extensions.Logging;
using Utils.Extensions;

namespace Broker.Queues.Services
{
    public class PersistanceService : IPersistanceService
    {
        private const string PersistancePath = "/home/nicu/MessageBroker/";
        private readonly ILogger<PersistanceService> _logger;

        public PersistanceService(ILogger<PersistanceService> logger)
        {
            _logger = logger;
        }

        public void Store(MbMessage message)
        {
            var fileName = GetFileName(message);
            Directory.CreateDirectory(new FileInfo(fileName).DirectoryName);
            var fileStream = File.Create(fileName);
            var bytes = Encoding.ASCII.GetBytes(message.Content);
            fileStream.Write(bytes,0, bytes.Length);
            fileStream.Close();
            
            _logger.LogInformation("Created new message \"{0}\"",fileName);
        }

        public void Remove(MbMessage message)
        {
            var fileName = GetFileName(message);
            var fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
                _logger.LogInformation("Removed message \"{0}\"",fileName);
            }
        }

        public ICollection<MbMessage> RestoreAllMessages()
        {
            _logger.LogDebug("Restoring all messages");
            return Directory.EnumerateDirectories(PersistancePath)
                .Select(it => new DirectoryInfo(it))
                .SelectMany(RestoreQueue)
                .ToList();
        }

        private ICollection<MbMessage> RestoreQueue(DirectoryInfo queueDirectoryInfo)
        {
            return queueDirectoryInfo.EnumerateFiles()
                .OrderBy(file => Convert.ToInt64(file.Name))
                .Select(file => GetMessage(file, queueDirectoryInfo.Name))
                .ToList();
        }

        private MbMessage GetMessage(FileInfo fileInfo, string queueIdentifier)
        {
            var bytes = File.ReadAllBytes(fileInfo.FullName);
            var content = Encoding.ASCII.GetString(bytes);
            var ticks = Convert.ToInt64(fileInfo.Name);
            _logger.LogDebug("Recreated message \"{0}\"",fileInfo.FullName);
            
            return new MbMessage(queueIdentifier,content,new DateTime(ticks,DateTimeKind.Utc));
        }

        private string GetFileName(MbMessage message)
        {
            return Path.Combine(PersistancePath, message.QueueIdentifier, message.CreatedDateTime.Ticks.ToString());
        }
    }
}