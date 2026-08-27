using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanSaludAPI.BusinessLogic;
using SanSaludAPI.Shared;

namespace SanSaludAPI.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicosController(IMedicoService medicoService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicoResponseDTO>>> GetMedicos()
        {
            var medicos = await medicoService.GetAllMedicosAsync();
            return Ok(medicos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicoResponseDTO>> GetMedico(Guid id)
        {
            var medico = await medicoService.GetMedicoByIdAsync(id);
            if (medico == null)
            {
                return NotFound();
            }
            return Ok(medico);
        }

        [HttpPost]
        public async Task<ActionResult<MedicoResponseDTO>> CreateMedico([FromBody] MedicoCreateDTO medicoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medico = await medicoService.CreateMedicoAsync(medicoDto);
            return CreatedAtAction(nameof(GetMedico), new { id = medico.Id }, medico);
        }
    }
}
