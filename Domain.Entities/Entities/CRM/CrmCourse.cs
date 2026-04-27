using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Entities.CRM;

public partial class CrmCourse
{
  [Key]
    public int CourseId { get; set; }

    public int InstituteId { get; set; }

    public string? CourseTitle { get; set; }

    public string? CourseLevel { get; set; }

    public decimal? CourseFee { get; set; }

    public decimal? ApplicationFee { get; set; }

    public int? CurrencyId { get; set; }

    public decimal? MonthlyLivingCost { get; set; }

    public string? PartTimeWorkDetails { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? CourseBenefits { get; set; }

    public string? LanguagesRequirement { get; set; }

    public string? CourseDuration { get; set; }

    public string? CourseCategory { get; set; }

    public string? AwardingBody { get; set; }

    public string? AdditionalInformationOfCourse { get; set; }

    public string? GeneralEligibility { get; set; }

    public string? FundsRequirementforVisa { get; set; }

    public string? InstitutionalBenefits { get; set; }

    public string? VisaRequirement { get; set; }

    public string? CountryBenefits { get; set; }

    public string? KeyModules { get; set; }

    public bool? Status { get; set; }

    public string? After2YearsPswcompletingCourse { get; set; }

    public string? DocumentId { get; set; }

    // Phase 1 upgrade — academic structure
    public int? FacultyId { get; set; }
    public int? DegreeLevelId { get; set; }
    public int? Duration { get; set; }
    /// <summary>DurationUnit: 1=Years, 2=Months, 3=Weeks</summary>
    public byte? DurationUnit { get; set; }
    /// <summary>StudyMode: 1=FullTime, 2=PartTime, 3=Online, 4=Blended</summary>
    public byte? StudyMode { get; set; }
    public string? LanguageOfInstruction { get; set; }

    // Phase 1 upgrade — per-course entry requirement overrides
    public decimal? OverrideMinIelts { get; set; }
    public decimal? OverrideMinToefl { get; set; }
    public decimal? OverrideMinAcademic { get; set; }
}
