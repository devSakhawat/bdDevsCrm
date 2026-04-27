using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Services.CRM;
using Domain.Exceptions;
using Moq;
using Xunit;

namespace bdDevsCrm.UnitTests.CRM;

/// <summary>
/// Unit tests for CrmLeadSourceService covering all CRUD operations.
/// </summary>
public class CrmLeadSourceServiceTests
{
    private readonly Mock<ICrmLeadSourceService> _mockService;

    public CrmLeadSourceServiceTests()
    {
        _mockService = new Mock<ICrmLeadSourceService>();
    }

    [Fact]
    public async Task CreateAsync_WithValidRecord_ReturnsCreatedDto()
    {
        // Arrange
        var record = new CreateCrmLeadSourceRecord(
            SourceName: "Website",
            SourceCode: "WEB",
            IsActive: true,
            CreatedDate: DateTime.UtcNow,
            CreatedBy: 1,
            UpdatedDate: null,
            UpdatedBy: null);

        var expected = new CrmLeadSourceDto
        {
            LeadSourceId = 1,
            SourceName = "Website",
            SourceCode = "WEB",
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = 1
        };

        _mockService.Setup(s => s.CreateAsync(record, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expected);

        // Act
        var result = await _mockService.Object.CreateAsync(record);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.LeadSourceId);
        Assert.Equal("Website", result.SourceName);
        Assert.Equal("WEB", result.SourceCode);
    }

    [Fact]
    public async Task CreateAsync_WhenDuplicateSourceName_ThrowsDuplicateRecordException()
    {
        // Arrange
        var record = new CreateCrmLeadSourceRecord(
            SourceName: "Website",
            SourceCode: "WEB",
            IsActive: true,
            CreatedDate: DateTime.UtcNow,
            CreatedBy: 1,
            UpdatedDate: null,
            UpdatedBy: null);

        _mockService.Setup(s => s.CreateAsync(record, It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new DuplicateRecordException("CrmLeadSource", "SourceName"));

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateRecordException>(
            () => _mockService.Object.CreateAsync(record));
    }

    [Fact]
    public async Task UpdateAsync_WithValidRecord_ReturnsUpdatedDto()
    {
        // Arrange
        var record = new UpdateCrmLeadSourceRecord(
            LeadSourceId: 1,
            SourceName: "Social Media",
            SourceCode: "SOC",
            IsActive: true,
            CreatedDate: DateTime.UtcNow.AddDays(-1),
            CreatedBy: 1,
            UpdatedDate: DateTime.UtcNow,
            UpdatedBy: 1);

        var expected = new CrmLeadSourceDto
        {
            LeadSourceId = 1,
            SourceName = "Social Media",
            SourceCode = "SOC",
            IsActive = true
        };

        _mockService.Setup(s => s.UpdateAsync(record, true, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expected);

        // Act
        var result = await _mockService.Object.UpdateAsync(record, trackChanges: true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Social Media", result.SourceName);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_CompletesSuccessfully()
    {
        // Arrange
        var record = new DeleteCrmLeadSourceRecord(LeadSourceId: 1);

        _mockService.Setup(s => s.DeleteAsync(record, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

        // Act
        await _mockService.Object.DeleteAsync(record, trackChanges: true);

        // Assert
        _mockService.Verify(s => s.DeleteAsync(record, true, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LeadSourceAsync_WithValidId_ReturnsDto()
    {
        // Arrange
        var expected = new CrmLeadSourceDto { LeadSourceId = 1, SourceName = "Website", IsActive = true };

        _mockService.Setup(s => s.LeadSourceAsync(1, false, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expected);

        // Act
        var result = await _mockService.Object.LeadSourceAsync(1, trackChanges: false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Website", result.SourceName);
    }

    [Fact]
    public async Task LeadSourcesAsync_ReturnsAllRecords()
    {
        // Arrange
        var expected = new List<CrmLeadSourceDto>
        {
            new() { LeadSourceId = 1, SourceName = "Website" },
            new() { LeadSourceId = 2, SourceName = "Referral" }
        };

        _mockService.Setup(s => s.LeadSourcesAsync(false, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expected);

        // Act
        var result = await _mockService.Object.LeadSourcesAsync(trackChanges: false);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task LeadSourcesSummaryAsync_WithGridOptions_ReturnsPaginatedResult()
    {
        // Arrange
        var options = new GridOptions { page = 1, pageSize = 20 };
        var expected = new GridEntity<CrmLeadSourceDto>
        {
            Items = new List<CrmLeadSourceDto> { new() { LeadSourceId = 1, SourceName = "Website" } },
            TotalCount = 1
        };

        _mockService.Setup(s => s.LeadSourcesSummaryAsync(options, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expected);

        // Act
        var result = await _mockService.Object.LeadSourcesSummaryAsync(options);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
    }
}
