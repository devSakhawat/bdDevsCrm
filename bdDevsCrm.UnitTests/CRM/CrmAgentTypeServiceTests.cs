using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Services.CRM;
using Domain.Exceptions;
using Moq;
using Xunit;

namespace bdDevsCrm.UnitTests.CRM;

/// <summary>
/// Unit tests for CrmAgentTypeService covering all CRUD operations.
/// </summary>
public class CrmAgentTypeServiceTests
{
    private readonly Mock<ICrmAgentTypeService> _mockService;

    public CrmAgentTypeServiceTests()
    {
        _mockService = new Mock<ICrmAgentTypeService>();
    }

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_WithValidRecord_ReturnsCreatedDto()
    {
        // Arrange
        var record = new CreateCrmAgentTypeRecord(
            AgentTypeName: "Direct",
            Description: "Direct referral agent",
            IsActive: true,
            CreatedDate: DateTime.UtcNow,
            CreatedBy: 1,
            UpdatedDate: null,
            UpdatedBy: null);

        var expected = new CrmAgentTypeDto
        {
            AgentTypeId = 1,
            AgentTypeName = "Direct",
            Description = "Direct referral agent",
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
        Assert.Equal(1, result.AgentTypeId);
        Assert.Equal("Direct", result.AgentTypeName);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task CreateAsync_WhenDuplicateName_ThrowsDuplicateRecordException()
    {
        // Arrange
        var record = new CreateCrmAgentTypeRecord(
            AgentTypeName: "Direct",
            Description: null,
            IsActive: true,
            CreatedDate: DateTime.UtcNow,
            CreatedBy: 1,
            UpdatedDate: null,
            UpdatedBy: null);

        _mockService.Setup(s => s.CreateAsync(record, It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new DuplicateRecordException("CrmAgentType", "AgentTypeName"));

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateRecordException>(
            () => _mockService.Object.CreateAsync(record));
    }

    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_WithValidRecord_ReturnsUpdatedDto()
    {
        // Arrange
        var record = new UpdateCrmAgentTypeRecord(
            AgentTypeId: 1,
            AgentTypeName: "Indirect",
            Description: "Updated description",
            IsActive: true,
            CreatedDate: DateTime.UtcNow.AddDays(-1),
            CreatedBy: 1,
            UpdatedDate: DateTime.UtcNow,
            UpdatedBy: 1);

        var expected = new CrmAgentTypeDto
        {
            AgentTypeId = 1,
            AgentTypeName = "Indirect",
            IsActive = true
        };

        _mockService.Setup(s => s.UpdateAsync(record, true, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expected);

        // Act
        var result = await _mockService.Object.UpdateAsync(record, trackChanges: true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.AgentTypeId);
        Assert.Equal("Indirect", result.AgentTypeName);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var record = new UpdateCrmAgentTypeRecord(
            AgentTypeId: 9999,
            AgentTypeName: "NonExistent",
            Description: null,
            IsActive: true,
            CreatedDate: DateTime.UtcNow,
            CreatedBy: 1,
            UpdatedDate: DateTime.UtcNow,
            UpdatedBy: 1);

        _mockService.Setup(s => s.UpdateAsync(record, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new NotFoundException("CrmAgentType", "AgentTypeId", "9999"));

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _mockService.Object.UpdateAsync(record, trackChanges: true));
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_WithValidId_CompletesSuccessfully()
    {
        // Arrange
        var record = new DeleteCrmAgentTypeRecord(AgentTypeId: 1);

        _mockService.Setup(s => s.DeleteAsync(record, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

        // Act
        await _mockService.Object.DeleteAsync(record, trackChanges: true);

        // Assert
        _mockService.Verify(s => s.DeleteAsync(record, true, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var record = new DeleteCrmAgentTypeRecord(AgentTypeId: 9999);

        _mockService.Setup(s => s.DeleteAsync(record, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new NotFoundException("CrmAgentType", "AgentTypeId", "9999"));

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _mockService.Object.DeleteAsync(record, trackChanges: true));
    }

    #endregion

    #region Read operations

    [Fact]
    public async Task AgentTypeAsync_WithValidId_ReturnsDto()
    {
        // Arrange
        var expected = new CrmAgentTypeDto { AgentTypeId = 1, AgentTypeName = "Direct", IsActive = true };

        _mockService.Setup(s => s.AgentTypeAsync(1, false, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expected);

        // Act
        var result = await _mockService.Object.AgentTypeAsync(1, trackChanges: false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.AgentTypeId);
    }

    [Fact]
    public async Task AgentTypeAsync_WhenNotFound_ThrowsNotFoundException()
    {
        // Arrange
        _mockService.Setup(s => s.AgentTypeAsync(9999, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new NotFoundException("CrmAgentType", "AgentTypeId", "9999"));

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _mockService.Object.AgentTypeAsync(9999, trackChanges: false));
    }

    [Fact]
    public async Task AgentTypesAsync_ReturnsAllRecords()
    {
        // Arrange
        var expected = new List<CrmAgentTypeDto>
        {
            new() { AgentTypeId = 1, AgentTypeName = "Direct", IsActive = true },
            new() { AgentTypeId = 2, AgentTypeName = "Indirect", IsActive = true }
        };

        _mockService.Setup(s => s.AgentTypesAsync(false, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expected);

        // Act
        var result = await _mockService.Object.AgentTypesAsync(trackChanges: false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AgentTypeForDDLAsync_ReturnsLightweightList()
    {
        // Arrange
        var expected = new List<CrmAgentTypeDto>
        {
            new() { AgentTypeId = 1, AgentTypeName = "Direct" },
            new() { AgentTypeId = 2, AgentTypeName = "Indirect" }
        };

        _mockService.Setup(s => s.AgentTypeForDDLAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expected);

        // Act
        var result = await _mockService.Object.AgentTypeForDDLAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AgentTypesSummaryAsync_WithGridOptions_ReturnsPaginatedResult()
    {
        // Arrange
        var options = new GridOptions { page = 1, pageSize = 10 };
        var expected = new GridEntity<CrmAgentTypeDto>
        {
            Items = new List<CrmAgentTypeDto>
            {
                new() { AgentTypeId = 1, AgentTypeName = "Direct", IsActive = true }
            },
            TotalCount = 1
        };

        _mockService.Setup(s => s.AgentTypesSummaryAsync(options, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expected);

        // Act
        var result = await _mockService.Object.AgentTypesSummaryAsync(options);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(1, result.TotalCount);
    }

    #endregion
}
