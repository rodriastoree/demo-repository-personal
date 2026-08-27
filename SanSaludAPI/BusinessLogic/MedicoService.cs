using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SanSaludAPI.DataAccess;
using SanSaludAPI.Shared;

namespace SanSaludAPI.BusinessLogic
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _medicoRepository;

        public MedicoService(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        public async Task<IEnumerable<MedicoResponseDTO>> GetAllMedicosAsync()
        {
            var medicos = await _medicoRepository.GetAllAsync();
            return medicos.Select(m => new MedicoResponseDTO
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Especialidad = m.Especialidad,
                Matricula = m.Matricula
            });
        }

        public async Task<MedicoResponseDTO?> GetMedicoByIdAsync(Guid id)
        {
            var medico = await _medicoRepository.GetByIdAsync(id);
            if (medico == null) return null;

            return new MedicoResponseDTO
            {
                Id = medico.Id,
                Nombre = medico.Nombre,
                Especialidad = medico.Especialidad,
                Matricula = medico.Matricula
            };
        }

        public async Task<MedicoResponseDTO> CreateMedicoAsync(MedicoCreateDTO medicoDto)
        {
            var medico = new Medico
            {
                Nombre = medicoDto.Nombre,
                Especialidad = medicoDto.Especialidad,
                Matricula = medicoDto.Matricula
            };

            var createdMedico = await _medicoRepository.CreateAsync(medico);

            return new MedicoResponseDTO
            {
                Id = createdMedico.Id,
                Nombre = createdMedico.Nombre,
                Especialidad = createdMedico.Especialidad,
                Matricula = createdMedico.Matricula
            };
        }
    }
}
