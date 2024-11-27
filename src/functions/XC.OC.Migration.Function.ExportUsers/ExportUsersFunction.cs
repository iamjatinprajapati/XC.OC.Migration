using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using XC.OC.Migration.Core.Application.Abstractions;
using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;
using XC.OC.Migration.Core.Application.Features.Users.Commands.ExportUsersCommand;

namespace XC.OC.Migration.Function.ExportUsers
{
    public class ExportUsersFunction
    {
        private readonly ILogger<ExportUsersFunction> _logger;
        private readonly IUsersRepository _usersRepository;
        private readonly IFileStorageService _fileStorageService;

        public ExportUsersFunction(ILogger<ExportUsersFunction> logger, IUsersRepository usersRepository, IFileStorageService fileStorageService)
        {
            _logger = logger;
            _usersRepository = usersRepository;
            _fileStorageService = fileStorageService;
        }

        [Function(nameof(ExportUsersFunction))]
        public async Task RunAsync([QueueTrigger("%ExportUsersQueueName%", Connection = "%queueConnectionString%")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
            var expoertUsersRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<ExportUsersRequest>(message.MessageText);
            if (expoertUsersRequest != null)
            {
                var totalUsers = await _usersRepository.GetUsersCount(expoertUsersRequest.UserNamePrefix, expoertUsersRequest.StartDate, 
                    expoertUsersRequest.EndDate, 1, expoertUsersRequest.PageSize);
                var totalPages = (int)Math.Ceiling((double)(totalUsers / expoertUsersRequest.PageSize));
                for(int page = 0; page < totalPages; page++)
                {
                    var users = await _usersRepository.GetUsers(expoertUsersRequest.UserNamePrefix, expoertUsersRequest.StartDate, expoertUsersRequest.EndDate,
                        pageIndex: page, pageSize: expoertUsersRequest.PageSize, applicationName: expoertUsersRequest.ApplicationName);
                    string fileName = $"{expoertUsersRequest.FileNamePrefix}/file-{page + 1}.json";
                    await _fileStorageService.SaveFileAsync(Newtonsoft.Json.JsonConvert.SerializeObject(users), fileName);
                }
            }
        }
    }
}
