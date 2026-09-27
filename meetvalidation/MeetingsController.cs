using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/meetings")]
public class MeetingsController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(MeetingCreateDto dto)
    {
        return Ok(dto);
    }
}
