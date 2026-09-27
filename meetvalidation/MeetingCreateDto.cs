using FluentValidation;

public class MeetingCreateDto
{
    public string Title { get; set; } = "";
    public DateTime Date { get; set; }
}

public class MeetingCreateDtoValidator : AbstractValidator<MeetingCreateDto>
{
    public MeetingCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Date)
            .NotEmpty();
    }
}
