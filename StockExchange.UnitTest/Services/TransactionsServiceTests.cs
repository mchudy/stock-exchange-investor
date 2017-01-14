using FluentAssertions;
using Moq;
using StockExchange.Business.ErrorHandling;
using StockExchange.Business.Exceptions;
using StockExchange.Business.Models.Transaction;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Business.Services;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace StockExchange.UnitTest.Services
{
    public class TransactionsServiceTests
    {
        private readonly ITransactionsService _service;
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ITransactionsRepository> _transactionRepository = new Mock<ITransactionsRepository>();

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
            _userRepository.Setup(u => u.GetUserWithTransactions(userId))
                .Returns(System.Threading.Tasks.Task.FromResult(_user));
        }

        [Fact]
        public void Should_throw_if_user_does_not_exist()
        {
            var dto = new UserTransactionDto { UserId = 4 };
            Func<System.Threading.Tasks.Task> act = async () => await _service.AddTransaction(dto);
            act.ShouldThrow<BusinessException>().Where(e => e.Status == ErrorStatus.DataNotFound);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_update_user_budget_when_selling()
        {
            var dto = new UserTransactionDto
            {
                UserId = userId,
                Quantity = -2,
                Price = 5,
                CompanyId = ownedCompanyId
            };
            await _service.AddTransaction(dto);
            _user.Budget.Should().Be(110);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_update_user_budget_when_buying()
        {
            var dto = new UserTransactionDto
            {
                UserId = userId,
                Quantity = 2,
                Price = 10,
                CompanyId = notOwnedCompanyId
            };
            await _service.AddTransaction(dto);
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
            Func<System.Threading.Tasks.Task> act = async () => await _service.AddTransaction(dto);
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
            Func<System.Threading.Tasks.Task> act = async () => await _service.AddTransaction(dto);
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
            Func<System.Threading.Tasks.Task> act = async () => await _service.AddTransaction(dto);
            act.ShouldThrow<BusinessException>().Where(e => e.Status == ErrorStatus.BusinessRuleViolation);
            _user.Transactions.Count.Should().Be(1);
        }
    }
}