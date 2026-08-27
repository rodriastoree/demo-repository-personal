using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SanSaludAPI.Shared;
using Xunit;

namespace SanSaludAPI.Tests.IntegrationTests
{
    public class MedicosControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public MedicosControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetMedicos_ReturnsSuccessAndJsonArray()
        {
            // Act
            var response = await _client.GetAsync("/api/medicos");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var medicos = await response.Content.ReadFromJsonAsync<List<MedicoResponseDTO>>();
            Assert.NotNull(medicos);
        }

        [Fact]
        public async Task CreateMedico_ReturnsSuccessAndCreatedMedico()
        {
            // Arrange
            var newMedico = new MedicoCreateDTO
            {
                Nombre = "Dr. Roberto Gómez",
                Especialidad = "Neurología",
                Matricula = "MN9988"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/medicos", newMedico);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created);
            var createdMedico = await response.Content.ReadFromJsonAsync<MedicoResponseDTO>();
            Assert.NotNull(createdMedico);
            Assert.NotEqual(Guid.Empty, createdMedico.Id);
            Assert.Equal("Dr. Roberto Gómez", createdMedico.Nombre);
            Assert.Equal("Neurología", createdMedico.Especialidad);
            Assert.Equal("MN9988", createdMedico.Matricula);
        }

        [Fact]
        public async Task GetMedicoById_WhenNonExistent_ReturnsNotFound()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/api/medicos/{randomId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
