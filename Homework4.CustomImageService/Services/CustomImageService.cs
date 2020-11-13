using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Homework4.CustomImageService.Clients;
using Homework4.CustomImageService.Interfaces;
using Homework4.CustomImageService.Models;
using Newtonsoft.Json;

namespace Homework4.CustomImageService.Services
{
    public class CustomImageService : ICustomImageService
    {
        private readonly ICustomImageClient _customImageClient;

        public CustomImageService(ICustomImageClient customImageClient)
        {
            _customImageClient = customImageClient ?? throw new ArgumentException(nameof(customImageClient));
        }

        public async Task<IEnumerable<ImageModel>> GetAll()
        {
            try
            {
                var data = JsonConvert.DeserializeObject<CustomImageModel>(await _customImageClient.GetAll());
                return data.Images;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task Post()
        {
            try
            {
                await _customImageClient.Post();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}