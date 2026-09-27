using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingsApi.Data;
using MeetingsApi.DTOs;
using MeetingsApi.Models;

namespace MeetingsApi.Controllers;

[ApiController, Route("participants")]
public class ParticipantsController : ControllerBase
{
    readonly AppDbContext db; readonly IMapper mapper;
    public ParticipantsController(AppDbContext db,IMapper mapper){this.db=db;this.mapper=mapper;}

    [HttpGet]
    public async Task<ActionResult<List<ParticipantDto>>> Get()
    {
        var x=await db.Participants.OrderBy(p=>p.Name).ToListAsync();
        return Ok(mapper.Map<List<ParticipantDto>>(x));
    }

    [HttpPost]
    public async Task<ActionResult<ParticipantDto>> Create(ParticipantDto dto)
    {
        if(string.IsNullOrWhiteSpace(dto.Name)||string.IsNullOrWhiteSpace(dto.Email))return BadRequest();
        var p=new Participant{Name=dto.Name,Email=dto.Email}; db.Participants.Add(p); await db.SaveChangesAsync();
        return Ok(mapper.Map<ParticipantDto>(p));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var p=await db.Participants.Include(x=>x.Meetings).FirstOrDefaultAsync(x=>x.Id==id);
        if(p==null)return NotFound(); p.Meetings.Clear(); db.Participants.Remove(p); await db.SaveChangesAsync(); return NoContent();
    }
}
