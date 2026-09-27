using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingsApi.Data;
using MeetingsApi.DTOs;
using MeetingsApi.Models;

namespace MeetingsApi.Controllers;

[ApiController, Route("meetings")]
public class MeetingsController : ControllerBase
{
    readonly AppDbContext db; readonly IMapper mapper;
    public MeetingsController(AppDbContext db,IMapper mapper){this.db=db;this.mapper=mapper;}

    [HttpGet]
    public async Task<ActionResult<List<MeetingDto>>> Get()
    {
        var x=await db.Meetings.Include(m=>m.Participants).Include(m=>m.Room).OrderBy(m=>m.StartTime).ToListAsync();
        return Ok(mapper.Map<List<MeetingDto>>(x));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MeetingDetailDto>> Get(int id)
    {
        var x=await db.Meetings.Include(m=>m.Participants).Include(m=>m.Room).FirstOrDefaultAsync(m=>m.Id==id);
        return x==null ? NotFound() : Ok(mapper.Map<MeetingDetailDto>(x));
    }

    [HttpPost]
    public async Task<ActionResult<MeetingDetailDto>> Create(MeetingCreateDto dto)
    {
        if(string.IsNullOrWhiteSpace(dto.Title)) return BadRequest("Title is required.");
        var ps=await db.Participants.Where(p=>dto.ParticipantIds.Contains(p.Id)).ToListAsync();
        if(ps.Count!=dto.ParticipantIds.Distinct().Count()) return BadRequest("One or more participants were not found.");
        if(dto.RoomId.HasValue && !await db.Rooms.AnyAsync(r=>r.Id==dto.RoomId.Value)) return BadRequest("Room was not found.");
        var m=mapper.Map<Meeting>(dto); m.Participants=ps; db.Meetings.Add(m); await db.SaveChangesAsync();
        await db.Entry(m).Reference(x=>x.Room).LoadAsync();
        return CreatedAtAction(nameof(Get),new{id=m.Id},mapper.Map<MeetingDetailDto>(m));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<MeetingDetailDto>> Update(int id,MeetingUpdateDto dto)
    {
        var m=await db.Meetings.Include(x=>x.Participants).Include(x=>x.Room).FirstOrDefaultAsync(x=>x.Id==id);
        if(m==null) return NotFound();
        var ps=await db.Participants.Where(p=>dto.ParticipantIds.Contains(p.Id)).ToListAsync();
        if(ps.Count!=dto.ParticipantIds.Distinct().Count()) return BadRequest("One or more participants were not found.");
        if(dto.RoomId.HasValue && !await db.Rooms.AnyAsync(r=>r.Id==dto.RoomId.Value)) return BadRequest("Room was not found.");
        m.Title=dto.Title; m.StartTime=dto.StartTime; m.RoomId=dto.RoomId; m.Participants=ps;
        await db.SaveChangesAsync(); await db.Entry(m).Reference(x=>x.Room).LoadAsync();
        return Ok(mapper.Map<MeetingDetailDto>(m));
    }

    [HttpGet("by-participant/{participantId:int}")]
    public async Task<ActionResult<List<MeetingDto>>> ByParticipant(int participantId)
    {
        if(!await db.Participants.AnyAsync(p=>p.Id==participantId)) return NotFound();
        var x=await db.Meetings.Include(m=>m.Participants).Where(m=>m.Participants.Any(p=>p.Id==participantId)).ToListAsync();
        return Ok(mapper.Map<List<MeetingDto>>(x));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var m=await db.Meetings.FindAsync(id); if(m==null)return NotFound();
        db.Meetings.Remove(m); await db.SaveChangesAsync(); return NoContent();
    }
}
