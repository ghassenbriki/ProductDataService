using System;
using System.Collections.Generic;

namespace Leoni.Domain.Entities;

public partial class Tasks
{
    public int TaskId { get; set; }

    public string? LeoniPn { get; set; }

    public string? ActivePart { get; set; }

    public string? CustomerPn { get; set; }

    public string? Type { get; set; }

    public string? TerminalReelDirectionWireSizes { get; set; }

    public string? Description { get; set; }

    public string? ComponentGroup { get; set; }

    public int? TimeSmv { get; set; }

    public int? CompletedMinutesSmv { get; set; }

    public int? ActualMinutes { get; set; }

    public bool? NeededForClassification { get; set; }

    public bool? NeededForValidation { get; set; }

    public string? ResponsibleComponentEngineerId { get; set; }
    
    public string? AssignedByFk { get; set; }


    public DateOnly? CleansingDate { get; set; }

    public int StatusId { get; set; }


    public required string  ComponentName {  get; set; }    

    public string? InternalStatus { get; set; }

    public string? ReleasedPartNumber { get; set; }

    public string? PartnerDrawing { get; set; }

    public string? DocumentComments { get; set; }

    public string? DevExXmllink { get; set; }

    public string? IBomxmllink { get; set; }

    public string? FailureCategory { get; set; }

    public string? FailureDetails { get; set; }

    public virtual Employee? ResponsibleComponentEngineer { get; set; }

    public virtual Employee? AssignedBy { get; set; }

    public virtual Component Component { get; set; } = null!;


    public virtual Status Status { get; set; } = null!;
}
