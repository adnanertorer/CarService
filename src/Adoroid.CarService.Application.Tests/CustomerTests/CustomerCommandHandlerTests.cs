using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.Customers.Abstracts;
using Adoroid.CarService.Application.Features.Customers.Commands.Create;
using Adoroid.CarService.Application.Features.Customers.Commands.Delete;
using Adoroid.CarService.Application.Features.Customers.Commands.Update;
using Adoroid.CarService.Application.Features.Customers.Queries.GetById;
using Adoroid.CarService.Application.Features.Customers.Queries.GetList;
using Adoroid.CarService.Application.Tests.Data;
using Adoroid.CarService.Application.Tests.Helpers;
using Adoroid.CarService.Persistence;
using Adoroid.CarService.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Application.Tests.CustomerTests;

public class CustomerCommandHandlerTests : IDisposable
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerRepository _customerRepository;
    private readonly CreateCustomerCommandHandler _handler;
    private readonly UpdateCustomerCommandHandler _updateHandler;
    private readonly DeleteCustomerCommandHandler _deleteHandler;
    private readonly CustomerGetByIdQueryHandler _getByIdHandler;
    private readonly GetCustomerListQueryHandler _getListHandler;
    private readonly CarServiceDbContext _dbContext;

    public CustomerCommandHandlerTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<CarServiceDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

        _dbContext = new CarServiceDbContext(optionsBuilder.Options);

        _unitOfWork = new UnitOfWork(_dbContext);
        _customerRepository = new CustomerRepository(_dbContext);
        _unitOfWork.Customers = _customerRepository;
        
        var userId = UserData.User.Id.ToString();
        var companyId = CompanyData.Company.Id.ToString();
        var currentUser = CurrentUserMockHelper.Create(userId, companyId).Object;
        _handler = new CreateCustomerCommandHandler(_unitOfWork, currentUser);
        _updateHandler = new UpdateCustomerCommandHandler(_unitOfWork, currentUser);
        _deleteHandler = new DeleteCustomerCommandHandler(_unitOfWork, currentUser);
        _getByIdHandler = new CustomerGetByIdQueryHandler(_unitOfWork);
        _getListHandler = new GetCustomerListQueryHandler(_unitOfWork, currentUser);
    }

    [Fact]
    public async Task Handle_Should_Create_Customer_When_Valid_Data_Is_Provided()
    {
        //Arrange
        
        var command = new CreateCustomerCommand(
            Name: "John",
            Surname: "Doe",
            Email: "test@test.com",
            Phone: "+90555555555",
            Address: "123 Main St",
            TaxNumber: "1234567890",
            TaxOffice: "Main Tax Office",
            IsActive: true,
            CityId: 41,
            DistrictId: 409);

        //Act
        
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue(); 
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_Should_Update_Existing_Customer_When_Valid_Data()
    {
        //Arrange
        
        var command = new CreateCustomerCommand(
            Name: "John",
            Surname: "Doe",
            Email: "test@test.com",
            Phone: "+90555555555",
            Address: "123 Main St",
            TaxNumber: "1234567890",
            TaxOffice: "Main Tax Office",
            IsActive: true,
            CityId: 41,
            DistrictId: 409);

        
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();

        var updateCommand = new UpdateCustomerCommand(
            result.Data.Id, command.Name, command.Surname, command.Email, command.Phone, command.Address, command.TaxNumber, command.TaxOffice, false, command.CityId, command.DistrictId);

        //Act

        var updateResult = await _updateHandler.Handle(updateCommand, CancellationToken.None);

        //Assert

        updateResult.Should().NotBeNull();
        updateResult.Succeeded.Should().BeTrue();
        updateResult.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_Should_Remove_Customer_When_Data_Is_Valid()
    {
        //Arrange

        var command = new CreateCustomerCommand(
            Name: "John",
            Surname: "Doe",
            Email: "test@test.com",
            Phone: "+90555555555",
            Address: "123 Main St",
            TaxNumber: "1234567890",
            TaxOffice: "Main Tax Office",
            IsActive: true,
            CityId: 41,
            DistrictId: 409);


        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();

        var deleteCommand = new DeleteCustomerCommand(
            result.Data.Id);

        //Act

        var deleteResult = await _deleteHandler.Handle(deleteCommand, CancellationToken.None);

        //Assert

        deleteResult.Should().NotBeNull();
        deleteResult.Succeeded.Should().BeTrue();
        deleteResult.Data.Should().Be(result.Data.Id);
    }

    [Fact]
    public async Task Handle_Should_Get_Customer_By_Id_When_Data_Is_Valid()
    {
        //Arrange

        var command = new CreateCustomerCommand(
            Name: "John",
            Surname: "Doe",
            Email: "test@test.com",
            Phone: "+90555555555",
            Address: "123 Main St",
            TaxNumber: "1234567890",
            TaxOffice: "Main Tax Office",
            IsActive: true,
            CityId: 41,
            DistrictId: 409);


        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();

        var getByIdCommand = new CustomerGetByIdQuery(
            result.Data.Id);

        //Act

        var customerResult = await _getByIdHandler.Handle(getByIdCommand, CancellationToken.None);

        //Assert

        customerResult.Should().NotBeNull();
        customerResult.Succeeded.Should().BeTrue();
        customerResult.Data.Should().NotBeNull();
        customerResult.Data.Id.Should().Be(result.Data.Id);
    }

    [Fact]
    public async Task Handle_Should_Get_Customer_By_Filter_When_Data_Is_Valid()
    {
        //Arrange

        var command = new CreateCustomerCommand(
            Name: "John",
            Surname: "Doe",
            Email: "test@test.com",
            Phone: "+90555555555",
            Address: "123 Main St",
            TaxNumber: "1234567890",
            TaxOffice: "Main Tax Office",
            IsActive: true,
            CityId: 41,
            DistrictId: 409);


        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();

        var getListCommand = new GetCustomerListQuery(new Core.Application.Requests.PageRequest { IsAllItems = false,
        PageIndex = 0, PageSize = 10}, "John");

        //Act

        var customerResult = await _getListHandler.Handle(getListCommand, CancellationToken.None);

        //Assert

        customerResult.Should().NotBeNull();
        customerResult.Succeeded.Should().BeTrue();
        customerResult.Data.Should().NotBeNull();
        customerResult.Data.Count.Should().BeGreaterThan(0);
        customerResult.Data.Items.Should().ContainSingle(c => c.Name == result.Data.Name);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
    }

}
