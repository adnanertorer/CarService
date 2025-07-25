﻿using Adoroid.CarService.Application.Features.AccountTransactions.Abstracts;
using Adoroid.CarService.Application.Features.Cities.Abstracts;
using Adoroid.CarService.Application.Features.Companies.Abstracts;
using Adoroid.CarService.Application.Features.CompanyMasterServices.Abstracts;
using Adoroid.CarService.Application.Features.Customers.Abstracts;
using Adoroid.CarService.Application.Features.Districts.Abstracts;
using Adoroid.CarService.Application.Features.Employees.Abstracts;
using Adoroid.CarService.Application.Features.MainServices.Abstracts;
using Adoroid.CarService.Application.Features.MasterServices.Abstracts;
using Adoroid.CarService.Application.Features.MobileUsers.Abstracts;
using Adoroid.CarService.Application.Features.SubServices.Abstracts;
using Adoroid.CarService.Application.Features.Suppliers.Abstracts;
using Adoroid.CarService.Application.Features.Users.Abtracts.Repositories;
using Adoroid.CarService.Application.Features.UserToCompanies.Abstracts;
using Adoroid.CarService.Application.Features.Vehicles.Abstracts;

namespace Adoroid.CarService.Application.Common.Abstractions;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    ICompanyReposiyory Companies { get; }
    IAccountTransactionRepository AccountTransactions { get; }
    ICustomerRepository Customers { get; set; }
    IMainServiceRepository MainServices { get; }
    ISubServiceRepository SubServices { get; }
    IEmployeeRepository Employees { get; }
    IMobileUserRepository MobileUsers { get; }
    IVehicleRepository Vehicles { get; }
    IVehicleUserRepository VehicleUsers { get; }
    ICityRepository Cities { get; }
    IDistrictRepository Districts { get; }
    ICompanyServiceRepository CompanyServices { get; }
    IMasterServiceRepository MasterServices { get; }
    ISupplierRepository Suppliers { get; }
    IUserToCompanyRepository UserToCompanies { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}
