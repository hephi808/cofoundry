﻿using Cofoundry.Core;
using Cofoundry.Core.Validation;
using Cofoundry.Domain.Data;
using Cofoundry.Domain.Tests.Shared.Assertions;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cofoundry.Domain.Tests.Integration.Users.Commands
{
    [Collection(nameof(DbDependentFixtureCollection))]
    public class UpdateUserCommandHandlerTests
    {

        private const string UNIQUE_PREFIX = "UpdUsrCHT-";
        private const string PASSWORD = "neverbr3@kthechange";
        private static string EMAIL_DOMAIN = $"@{UNIQUE_PREFIX}.example.com";
        private readonly DbDependentTestApplicationFactory _appFactory;

        public UpdateUserCommandHandlerTests(
            DbDependentTestApplicationFactory appFactory
            )
        {
            _appFactory = appFactory;
        }

        [Fact]
        public async Task CanUpdateBasicProperties()
        {
            using var app = _appFactory.Create();
            var contentRepository = app.Services.GetContentRepositoryWithElevatedPermissions();
            var dbContext = app.Services.GetRequiredService<CofoundryDbContext>();
            var userArea = app.SeededEntities.TestUserArea1;

            var addCommand = new AddUserCommand()
            {
                Email = "reginald.dwight" + EMAIL_DOMAIN,
                Password = PASSWORD,
                FirstName = "Reginald",
                LastName = "Dwight",
                RoleCode = userArea.RoleA.RoleCode,
                UserAreaCode = userArea.UserAreaCode,
                RequirePasswordChange = false
            };

            var userId = await contentRepository
                .Users()
                .AddAsync(addCommand);

            var updateCommand = MapUpdateCommand(addCommand);
            updateCommand.Email = "E.john" + EMAIL_DOMAIN;
            updateCommand.FirstName = "Elton";
            updateCommand.LastName = "John";
            updateCommand.IsEmailConfirmed = true;
            updateCommand.RequirePasswordChange = true;

            await contentRepository
                .Users()
                .UpdateUserAsync(updateCommand);

            var user = await dbContext
                .Users
                .AsNoTracking()
                .FilterById(userId)
                .SingleOrDefaultAsync();

            using (new AssertionScope())
            {
                user.Should().NotBeNull();
                user.FirstName.Should().Be(updateCommand.FirstName);
                user.LastName.Should().Be(updateCommand.LastName);
                user.Email.Should().Be(updateCommand.Email);
                user.IsEmailConfirmed.Should().BeTrue();
                user.RequirePasswordChange.Should().BeTrue();
            }
        }

        [Fact]
        public async Task CanChangeRoleById()
        {
            var uniqueData = UNIQUE_PREFIX + nameof(CanChangeRoleById);

            using var app = _appFactory.Create();
            var contentRepository = app.Services.GetContentRepositoryWithElevatedPermissions();
            var dbContext = app.Services.GetRequiredService<CofoundryDbContext>();
            var loginService = app.Services.GetRequiredService<ILoginService>();
            var userArea = app.SeededEntities.TestUserArea1;
            var roleId = await app.TestData.Roles().AddAsync(uniqueData, userArea.UserAreaCode);

            var addCommand = new AddUserCommand()
            {
                Email = uniqueData + EMAIL_DOMAIN,
                Password = PASSWORD,
                RoleCode = userArea.RoleA.RoleCode,
                UserAreaCode = userArea.UserAreaCode
            };

            var userId = await contentRepository
                .Users()
                .AddAsync(addCommand);

            var updateCommand = MapUpdateCommand(addCommand);
            updateCommand.RoleCode = null;
            updateCommand.RoleId = roleId;

            await contentRepository
                .Users()
                .UpdateUserAsync(updateCommand);

            var user = await dbContext
                .Users
                .AsNoTracking()
                .FilterById(userId)
                .SingleOrDefaultAsync();

            using (new AssertionScope())
            {
                user.Should().NotBeNull();
                user.RoleId.Should().Be(updateCommand.RoleId);
            }
        }

        [Fact]
        public async Task CanChangeRoleByCode()
        {
            var uniqueData = UNIQUE_PREFIX + nameof(CanChangeRoleByCode);

            using var app = _appFactory.Create();
            var contentRepository = app.Services.GetContentRepositoryWithElevatedPermissions();
            var dbContext = app.Services.GetRequiredService<CofoundryDbContext>();
            var loginService = app.Services.GetRequiredService<ILoginService>();
            var userArea = app.SeededEntities.TestUserArea1;

            var addCommand = new AddUserCommand()
            {
                Email = uniqueData + EMAIL_DOMAIN,
                Password = PASSWORD,
                RoleCode = userArea.RoleA.RoleCode,
                UserAreaCode = userArea.UserAreaCode
            };

            var userId = await contentRepository
                .Users()
                .AddAsync(addCommand);

            var updateCommand = MapUpdateCommand(addCommand);
            updateCommand.RoleCode = userArea.RoleB.RoleCode;

            await contentRepository
                .Users()
                .UpdateUserAsync(updateCommand);

            var user = await dbContext
                .Users
                .AsNoTracking()
                .Include(u => u.Role)
                .FilterById(userId)
                .SingleOrDefaultAsync();

            using (new AssertionScope())
            {
                user.Should().NotBeNull();
                user.Role.RoleCode.Should().Be(updateCommand.RoleCode);
            }
        }

        [Fact]
        public async Task CantChangeUserAreaByRoleId()
        {
            var uniqueData = UNIQUE_PREFIX + nameof(CantChangeUserAreaByRoleId);

            using var app = _appFactory.Create();
            var contentRepository = app.Services.GetContentRepositoryWithElevatedPermissions();
            var dbContext = app.Services.GetRequiredService<CofoundryDbContext>();
            var loginService = app.Services.GetRequiredService<ILoginService>();
            var userArea1 = app.SeededEntities.TestUserArea1;
            var roleId = await app.TestData.Roles().AddAsync(uniqueData, app.SeededEntities.TestUserArea2.UserAreaCode);

            var addCommand = new AddUserCommand()
            {
                Email = uniqueData + EMAIL_DOMAIN,
                Password = PASSWORD,
                RoleCode = userArea1.RoleA.RoleCode,
                UserAreaCode = userArea1.UserAreaCode
            };

            var userId = await contentRepository
                .Users()
                .AddAsync(addCommand);

            var updateCommand = MapUpdateCommand(addCommand);
            updateCommand.RoleCode = null;
            updateCommand.RoleId = roleId;

            await contentRepository
                .Awaiting(r => r.Users().UpdateUserAsync(updateCommand))
                .Should()
                .ThrowAsync<ValidationErrorException>()
                .WithMessage("*user area*");
        }

        [Fact]
        public async Task CantChangeUserAreaByRoleCode()
        {
            var uniqueData = UNIQUE_PREFIX + nameof(CantChangeUserAreaByRoleCode);

            using var app = _appFactory.Create();
            var contentRepository = app.Services.GetContentRepositoryWithElevatedPermissions();
            var dbContext = app.Services.GetRequiredService<CofoundryDbContext>();
            var loginService = app.Services.GetRequiredService<ILoginService>();
            var userArea1 = app.SeededEntities.TestUserArea1;
            var userArea2 = app.SeededEntities.TestUserArea2;

            var addCommand = new AddUserCommand()
            {
                Email = uniqueData + EMAIL_DOMAIN,
                Password = PASSWORD,
                RoleCode = userArea1.RoleA.RoleCode,
                UserAreaCode = userArea1.UserAreaCode
            };

            var userId = await contentRepository
                .Users()
                .AddAsync(addCommand);

            var updateCommand = MapUpdateCommand(addCommand);
            updateCommand.RoleCode = userArea2.RoleB.RoleCode;

            await contentRepository
                .Awaiting(r => r.Users().UpdateUserAsync(updateCommand))
                .Should()
                .ThrowAsync<ValidationErrorException>()
                .WithMessage("*user area*");
        }

        [Fact]
        public async Task WhenUsernameNotUnique_Throws()
        {
            using var app = _appFactory.Create();
            var contentRepository = app.Services.GetContentRepositoryWithElevatedPermissions();
            var loginService = app.Services.GetRequiredService<ILoginService>();
            var userArea = app.SeededEntities.TestUserArea1;

            await contentRepository
                .Users()
                .AddAsync(new AddUserCommand()
                {
                    Email = $"djones" + EMAIL_DOMAIN,
                    Password = PASSWORD,
                    RoleCode = userArea.RoleA.RoleCode,
                    UserAreaCode = userArea.UserAreaCode
                });

            var addCommand = new AddUserCommand()
            {
                Email = $"dbowie" + EMAIL_DOMAIN,
                Password = PASSWORD,
                RoleCode = userArea.RoleA.RoleCode,
                UserAreaCode = userArea.UserAreaCode
            };

            var userId = await contentRepository
                .Users()
                .AddAsync(addCommand);

            var updateCommand = MapUpdateCommand(addCommand);
            updateCommand.Email = $"djones" + EMAIL_DOMAIN;

            await contentRepository
                .Awaiting(r => r.Users().UpdateUserAsync(updateCommand))
                .Should()
                .ThrowAsync<ValidationErrorException>()
                .WithMemberNames(nameof(updateCommand.Email))
                .WithMessage("*already registered*");
        }

        [Fact]
        public async Task WhenSystemUser_Throws()
        {
            using var app = _appFactory.Create();
            var contentRepository = app.Services.GetContentRepositoryWithElevatedPermissions();
            var userContextService = app.Services.GetService<IUserContextService>();

            var systemUser = await userContextService.GetSystemUserContextAsync();

            var command = new UpdateUserCommand()
            {
                UserId = systemUser.UserId.Value,
                RoleId = systemUser.RoleId,
                FirstName = "Trolo",
                LastName = "Lo"
            };

            await contentRepository
                .Awaiting(r => r.Users().UpdateUserAsync(command))
                .Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        private UpdateUserCommand MapUpdateCommand(AddUserCommand command)
        {
            return new UpdateUserCommand()
            {
                Email = command.Email,
                FirstName = command.FirstName,
                IsEmailConfirmed = false,
                LastName = command.LastName,
                RequirePasswordChange = command.RequirePasswordChange,
                RoleCode = command.RoleCode,
                UserId = command.OutputUserId,
                Username = command.Username
            };
        }
    }
}