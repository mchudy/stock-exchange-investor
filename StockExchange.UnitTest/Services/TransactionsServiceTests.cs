using FluentAssertions;
using Moq;
using StockExchange.Business.ErrorHandling;
using StockExchange.Business.Exceptions;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Business.Services;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.Models.Transaction;
using Xunit;

namespace StockExchange.UnitTest.Services
{
    public class TransactionsServiceTests
    {
        private readonly ITransactionsService _service;
        private readonly Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
        private readonly Mock<IRepository<UserTransaction>> _transactionRepository = new Mock<IRepository<UserTransaction>>();

        private const int userId = 1;
        private const int notOwnedCompanyId = 1;
        private const int ownedCompanyId = 2;

        private readonly User _user = new User
        {
            Id = userId,
            Budget = 100,
            Transactions = new List<UserTransaction>
            {
                new UserTransaction
                {
                    CompanyId = ownedCompanyId,
                    Quantity = 5,
                    Price = 10,
                    UserId = userId
                }
            }
        };

        public TransactionsServiceTests()
        {
            _service = new TransactionsService(_userRepository.Object, _transactionRepository.Object);
            _userRepository.Setup(u => u.GetQueryable())
                .Returns(new List<User> { _user }.AsQueryable());
        }

        [Fact]
        public void Should_return_false_if_user_does_not_exist()
        {
            var dto = new UserTransactionDto { UserId = 4 };        
            Action act = () => _service.AddUserTransaction(dto);
            act.ShouldThrow<BusinessException>().Where(e => e.Status == ErrorStatus.DataNotFound);
        }

        [Fact]
        public void Should_update_user_budget_when_selling()
        {
            var dto = new UserTransactionDto
            {
                UserId = userId,
                Quantity = -2,
                Price = 5,
                CompanyId = ownedCompanyId
            };
            _service.AddUserTransaction(dto);
            _user.Budget.Should().Be(110);
        }

        [Fact]
        public void Should_update_user_budget_when_buying()
        {
            var dto = new UserTransactionDto
            {
                UserId = userId,
                Quantity = 2,
                Price = 10,
                CompanyId = notOwnedCompanyId
            };
            _service.AddUserTransaction(dto);
            _user.Budget.Should().Be(80);
        }

        [Fact]
        public void Should_not_sell_if_user_doesnt_have_any_company_stocks()
        {
            var dto = new UserTransactionDto
            {
                UserId = userId,
                Quantity = -2,
                Price = 10,
                CompanyId = notOwnedCompanyId
            };
            Action act = () => _service.AddUserTransaction(dto);
            act.ShouldThrow<BusinessException>().Where(e => e.Status == ErrorStatus.BusinessRuleViolation);
            _user.Transactions.Count.Should().Be(1);
        }

        [Fact]
        public void Should_not_sell_if_user_has_less_stocks_than_declared()
        {
            var dto = new UserTransactionDto
            {
                UserId = userId,
                Quantity = -50,
                Price = 10,
                CompanyId = ownedCompanyId
            };
            Action act = () => _service.AddUserTransaction(dto);
            act.ShouldThrow<BusinessException>().Where(e => e.Status == ErrorStatus.BusinessRuleViolation);
            _user.Transactions.Count.Should().Be(1);
        }

        [Fact]
        public void Should_not_buy_if_user_doesnt_have_required_budget()
        {
            var dto = new UserTransactionDto
            {
                UserId = userId,
                Quantity = 1,
                Price = 1000,
                CompanyId = notOwnedCompanyId
            };
            Action act = () => _service.AddUserTransaction(dto);
            act.ShouldThrow<BusinessException>().Where(e => e.Status == ErrorStatus.BusinessRuleViolation);
            _user.Transactions.Count.Should().Be(1);
        }
    }
}